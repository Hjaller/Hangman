using Hangman.menu;
using System.Text.Json;
using System.Threading.Tasks;

namespace Hangman
{
    internal class Program
    {
        static string dataFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "words.json");

        static void Main(string[] args)
        {
            ShowMenu(MainMenuOptions);
        }

        static private MenuOption[] MainMenuOptions = new MenuOption[]
        {
            new MenuOption("New game", PlayGame),
            new MenuOption("Generate words", GenerateWordsToFile),
            new MenuOption("Exit", ExitProgram)
        };

        static private void ShowMenu(MenuOption[] menuOptions)
        {
            int selectedIndex = 0;

            while (true)
            {
                Console.Clear();
                Console.WriteLine("Use the arrow keys to navigate, and Enter to select an option:\n");

                for (int i = 0; i < menuOptions.Length; i++)
                {
                    if (i == selectedIndex)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"> {menuOptions[i].OptionText}");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.WriteLine($"  {menuOptions[i].OptionText}");
                    }
                }

                var keyInfo = Console.ReadKey(intercept: true);
                if (keyInfo.Key == ConsoleKey.UpArrow)
                {
                    selectedIndex--;
                    if (selectedIndex < 0) selectedIndex = menuOptions.Length - 1;
                }
                else if (keyInfo.Key == ConsoleKey.DownArrow)
                {
                    selectedIndex++;
                    if (selectedIndex >= menuOptions.Length) selectedIndex = 0;
                }
                else if (keyInfo.Key == ConsoleKey.Enter)
                {
                    var selectedOption = menuOptions[selectedIndex];

                    if (selectedOption != null)
                    {
                        if (selectedOption.SubMenu != null)
                        {
                            ShowMenu(selectedOption.SubMenu);
                        }
                        else if (selectedOption.Action != null)
                        {
                            Console.Clear();
                            selectedOption.Action.Invoke();
                        }
                        else if (selectedOption.OptionText.StartsWith("0"))
                        {
                            return;
                        }
                    }
                }

                if (keyInfo.Key == ConsoleKey.Escape)
                {
                    ExitProgram();
                    return;
                }
            }
        }

        static private void PlayGame()
        {
            do
            {
                string[] words = LoadWordsFromFile();
                string wordToGuess = words[new Random().Next(words.Length)];
                char[] guessedWord = new string('_', wordToGuess.Length).ToCharArray();
                char[] incorrectGuesses = new char[6];
                int incorrectGuessCount = 0;

                while (true)
                {
                    ShowGameStatus(guessedWord, incorrectGuesses, incorrectGuessCount);
                    char guess = GetGuessFromUser();

                    if (wordToGuess.Contains(guess))
                    {
                        for (int i = 0; i < wordToGuess.Length; i++)
                        {
                            if (wordToGuess[i] == guess)
                            {
                                guessedWord[i] = guess;
                            }
                        }
                    }
                    else
                    {
                        incorrectGuesses[incorrectGuessCount++] = guess;
                    }

                    if (new string(guessedWord) == wordToGuess)
                    {
                        ShowGameStatus(guessedWord, incorrectGuesses, incorrectGuessCount);
                        Console.WriteLine($"Congratulations! You've guessed the word: {wordToGuess}");
                        break;
                    }

                    if (incorrectGuessCount >= incorrectGuesses.Length)
                    {
                        ShowGameStatus(guessedWord, incorrectGuesses, incorrectGuessCount);
                        Console.WriteLine($"You've run out of guesses. The word was: {wordToGuess}");
                        break;
                    }
                }
            } while (AskToPlayAgain());
        }

        static private bool AskToPlayAgain()
        {
            Console.WriteLine("Do you want to play again? (y/n)");
            string? choice = Console.ReadLine();
            return choice?.ToLower() == "y";
        }


        static private void ShowGameStatus(char[] guessedWord, char[] incorrectGuesses, int incorrectGuessCount)
        {
            Console.Clear();
            Console.WriteLine(GetHangmanStage(incorrectGuessCount));
            Console.WriteLine("Current word: " + new string(guessedWord));
            Console.WriteLine("Incorrect guesses: " + new string(incorrectGuesses, 0, incorrectGuessCount));
            Console.WriteLine($"Guesses left: {incorrectGuesses.Length - incorrectGuessCount}");
        }

        static private string GetHangmanStage(int stage)
        {
            string[] stages = new string[]
            {
                @"
  +---+
  |   |
      |
      |
      |
      |
=========",
                @"
  +---+
  |   |
  O   |
      |
      |
      |
=========",
                @"
  +---+
  |   |
  O   |
  |   |
      |
      |
=========",
                @"
  +---+
  |   |
  O   |
 /|   |
      |
      |
=========",
                @"
  +---+
  |   |
  O   |
 /|\  |
      |
      |
=========",
                @"
  +---+
  |   |
  O   |
 /|\  |
 /    |
      |
=========",
                @"
  +---+
  |   |
  O   |
 /|\  |
 / \  |
      |
========="
            };

            return stages[stage];
        }

        static private char GetGuessFromUser()
        {
            Console.Write("Enter your guess: ");
            return Console.ReadLine()[0];
        }


        static private string[] LoadWordsFromFile()
        {
            if (File.Exists(dataFilePath))
            {
                return File.ReadAllLines(dataFilePath);
            }
            else
            {
                return new string[]
                {
                    "apple",
                    "banana",
                    "cherry",
                    "date",
                    "elderberry",
                    "fig",
                    "grape",
                    "honeydew"
                };
            }
        }

        static private void GenerateWordsToFile()
        {
            string[] words = new string[]
            {
        // Frugter
        "apple", "banana", "cherry", "date", "elderberry", "fig", "grape", "honeydew",
        "kiwi", "lemon", "mango", "nectarine", "orange", "papaya", "quince", "raspberry",
        "strawberry", "tangerine", "ugli", "vanilla", "watermelon", "xigua", "yellowfruit", "zucchini",
        "apricot", "blackberry", "blueberry", "cantaloupe", "coconut", "cranberry", "currant", "dragonfruit",
        "gooseberry", "guava", "jackfruit", "kumquat", "lime", "lychee", "mandarin", "mulberry",
        "olive", "passionfruit", "peach", "pear", "persimmon", "pineapple", "plum", "pomegranate",
        "starfruit", "tamarind", "tomato", "ugni", "yuzu",

        // Dyr
        "cat", "dog", "elephant", "giraffe", "hippopotamus", "kangaroo", "lion", "monkey",
        "panda", "penguin", "rabbit", "rhinoceros", "shark", "tiger", "whale", "zebra",
        "alligator", "bear", "cheetah", "dolphin", "eagle", "flamingo", "goat", "horse",
        "iguana", "jaguar", "koala", "lemur", "meerkat", "newt", "octopus", "parrot",
        "quail", "reindeer", "seal", "turkey", "vulture", "wolf", "yak",

        // Ting
        "airplane", "bicycle", "car", "drum", "elevator", "fork", "guitar", "helicopter",
        "icecream", "jacket", "kettle", "lamp", "microwave", "notebook", "oven", "piano",
        "quilt", "refrigerator", "scooter", "table", "umbrella", "violin", "watch", "xylophone",
        "yacht", "zipper"
            };

            File.WriteAllLines(dataFilePath, words);
            Console.WriteLine("Words have been generated and saved to the file.");
            Console.WriteLine("Press any key to return to the menu.");
            Console.ReadKey();
        }

        static private void ExitProgram()
        {
            Console.WriteLine("The program is exiting. Goodbye!");
            Environment.Exit(0);
        }
    }
}