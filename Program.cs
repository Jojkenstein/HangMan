using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HangMan
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string HangWord = RandomWord();
            HangWord = HangWord.ToUpper();

            char[] GuessLetters = new char[HangWord.Length]; // array för korrekta bokstavsgissningar
            GuessLetters = Enumerable.Repeat('_', HangWord.Length).ToArray();

            string d = ", "; // Stringbuilder divider
            StringBuilder WrongLetters = new StringBuilder(d);
            StringBuilder WrongWords = new StringBuilder(d);

            int MaxGuesses = 10;
            bool GameOn = true;
            bool Win = false;
            int GuessesLeft = MaxGuesses;

            while (GameOn)
            {
                try
                {
                    HangManLogo();
                    HangManASCII(GuessesLeft);
                    //Console.WriteLine();
                    Console.WriteLine("Guesses left: " + GuessesLeft);
                    if (WrongWords.Length > d.Length)
                    {
                        Console.WriteLine("Wrong words: " + WrongWords.ToString().Remove((WrongWords.Length - d.Length), d.Length).Substring(2));
                    }
                    else
                    {
                        //Console.WriteLine("Wrong word guesses will be printed on this row.");
                        Console.WriteLine();
                    }
                    if (WrongLetters.Length > d.Length)
                    {
                        Console.WriteLine("Wrong letters: " + WrongLetters.ToString().Remove((WrongLetters.Length - d.Length), d.Length).Substring(2));
                    }
                    else
                    {
                        //Console.WriteLine("Wrong letter guesses will be printed on this row.");
                        Console.WriteLine();
                    }                   
                    Console.WriteLine();
                    Console.Write("Correct letter guesses:\t");
                    Console.WriteLine(GuessLetters);
                    //Console.WriteLine();
                    Console.Write("Guess a letter/word:\t");
                    string guess = Console.ReadLine();
                    guess = guess.ToUpper();

                    if (guess.Length > 1) // --- KOD FÖR ORD ---
                    {
                        if (guess.Length != HangWord.Length)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("\nThe hidden word has " + HangWord.Length + " letters.\n\nPlease retry!");
                            Console.ResetColor();
                            Console.ReadKey();
                            GuessesLeft++;
                        }
                        else if (guess == HangWord)
                        {
                            Win = true;
                        }
                        else if (WrongWords.ToString().IndexOf(guess + d) > -1)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("\nThis is an already guessed word.\n\nPlease retry!");
                            Console.ResetColor();
                            Console.ReadKey();
                            GuessesLeft++;
                        }
                        else
                        {
                            WrongWords.Append($"{guess + d}");
                        }
                    }
                    else // --- KOD FÖR BOKSTÄVER ---
                    {
                        int index;
                        string tempWord = HangWord; // behövs för att finna samma bokstav mer än en gång
                        if ((GuessLetters.Contains(guess[0])) || (WrongLetters.ToString().IndexOf(guess[0]) > -1))  // Om bokstaven använts tidigare
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("\nThis is an already guessed letter.\n\nPlease retry!");
                            Console.ResetColor();
                            GuessesLeft++;
                            Console.ReadKey();
                        }
                        else if (tempWord.IndexOf(guess[0]) > -1) // sök bokstavsindex
                        {
                            do
                            {
                                index = tempWord.IndexOf(guess[0]); //input char, men string har längd 1 så borde funka
                                if (index > -1)
                                {
                                    GuessLetters[index] = guess[0];
                                    tempWord = tempWord.Replace(guess[0], '_');
                                    tempWord = tempWord.Remove(index + 1, HangWord.Length - index - 1);
                                    tempWord = String.Concat(tempWord, HangWord.Substring(index + 1));
                                }
                            }
                            while (index > -1);
                            if (!GuessLetters.Contains('_'))
                            {
                                Win = true;
                            }
                        }
                        else
                        {
                            WrongLetters.Append($"{guess + d}");
                        }
                    }
                    GuessesLeft--;
                    Console.WriteLine();
                    if (Win == true || (GuessesLeft < 1))
                    {
                        GameOn = false;
                    }
                    Console.Clear();
                }
                catch (System.IndexOutOfRangeException)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nYou must enter at least ONE letter!\n\nPlease try again.");
                    Console.ResetColor();
                    Console.ReadKey();
                    Console.Clear();
                }
            }
            // - - - RESULTAT! - - -
            if (!Win)
            {
                GameOverLogo();
                HangManASCII(GuessesLeft);
            }
            else
            {
                ManSavedLogo();
                HangManASCII(GuessesLeft+1); // +1 speciellt för att gubben inte ska se hängd ut om man gissar rätt på sista försöket
                Console.WriteLine("\nGuesses used: " + (MaxGuesses - GuessesLeft));
            }
            Console.WriteLine("\nHidden word:\t\t" + HangWord);
            Console.WriteLine();
            Console.Write("Correct letter guesses:\t");
            Console.WriteLine(GuessLetters);
            Console.WriteLine();
            if (WrongLetters.Length > d.Length)
            {
                Console.WriteLine("Incorrect letters:\t" + WrongLetters.ToString().Remove((WrongLetters.Length - d.Length), d.Length).Substring(2));
            }
            else
            {
                Console.WriteLine("No incorrect letter guesses where made!");
            }
            Console.WriteLine();
            if (WrongWords.Length > d.Length)
            {
                Console.WriteLine("Incorrect words:\t" + WrongWords.ToString().Remove((WrongWords.Length - d.Length), d.Length).Substring(2));
            }
            else
            {
                Console.Write("No incorrect word guesses where made");
                if (Win) { Console.Write("!\n"); } else { Console.Write(".\n"); }
            }
            Console.ReadKey();
        }
        private static void HangManLogo()
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("\t\t\t-------------------------");
            Console.WriteLine("\t\t\t----- H A N G M A N -----");
            Console.WriteLine("\t\t\t-------------------------");
            Console.ResetColor();
        }
        private static void GameOverLogo()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\t\t\t-------------------------");
            Console.WriteLine("\t\t\t--- G A M E   O V E R ---");
            Console.WriteLine("\t\t\t-------------------------");
            Console.ResetColor();
        }
        private static void ManSavedLogo()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\t\t\t-------------------------");
            Console.WriteLine("\t\t\t--- M A N   S A V E D ---");
            Console.WriteLine("\t\t\t-------------------------");
            Console.ResetColor();
        }
        static void HangManASCII(int countdown)
        {
            switch (countdown)
            {
                case 0:
                    List<string> ascii0 = new List<string>
                    {
                    "  _______",
                    "  |/     |",
                    "  |      o",
                    "  |     /|\\",
                    "  |     / \\",
                    " _|_"
                    };
                    ascii0.ForEach(Console.WriteLine);
                    break;
                case 1:
                    List<string> ascii1 = new List<string>
                    {
                    "  _______",
                    "  |/     |",
                    "  |      o",
                    "  |     /|\\",
                    "  |     /",
                    " _|_"
                    };
                    ascii1.ForEach(Console.WriteLine);
                    break;
                case 2:
                    List<string> ascii2 = new List<string>
                    {
                    "  _______",
                    "  |/     |",
                    "  |      o",
                    "  |     /|\\",
                    "  |",
                    " _|_"
                    };
                    ascii2.ForEach(Console.WriteLine);
                    break;
                case 3:
                    List<string> ascii3 = new List<string>
                    {
                    "  _______",
                    "  |/     |",
                    "  |      o",
                    "  |     /|",
                    "  |",
                    " _|_"
                    };
                    ascii3.ForEach(Console.WriteLine);
                    break;
                case 4:
                    List<string> ascii4 = new List<string>
                    {
                    "  _______",
                    "  |/     |",
                    "  |      o",
                    "  |      |",
                    "  |",
                    " _|_"
                    };
                    ascii4.ForEach(Console.WriteLine);
                    break;
                case 5:
                    List<string> ascii5 = new List<string>
                    {
                    "  _______",
                    "  |/     |",
                    "  |      o",
                    "  |",
                    "  |",
                    " _|_"
                    };
                    ascii5.ForEach(Console.WriteLine);
                    break;
                case 6:
                    List<string> ascii6 = new List<string>
                    {
                    "  _______",
                    "  |/     |",
                    "  |",
                    "  |",
                    "  |",
                    " _|_"
                    };
                    ascii6.ForEach(Console.WriteLine);
                    break;
                case 7:
                    List<string> ascii7 = new List<string>
                    {
                    "  _______",
                    "  |/",
                    "  |",
                    "  |",
                    "  |",
                    " _|_"
                    };
                    ascii7.ForEach(Console.WriteLine);
                    break;
                case 8:
                    List<string> ascii8 = new List<string>
                    {
                    "  _",
                    "  |",
                    "  |",
                    "  |",
                    "  |",
                    " _|_"
                    };
                    ascii8.ForEach(Console.WriteLine);
                    break;
                case 9:
                    List<string> ascii9 = new List<string>
                    {
                    " ",
                    " ",
                    " ",
                    " ",
                    " ",
                    " _|_"
                    };
                    ascii9.ForEach(Console.WriteLine);
                    break;
                default:
                    List<string> ascii = new List<string>
                    {
                    " ",
                    " ",
                    " ",
                    " ",
                    " ",
                    " "
                    };
                    ascii.ForEach(Console.WriteLine);
                    break;
            }
        }
        private static string RandomWord() // 100 ord från https://randomwordgenerator.com/ OBS! Brittisk engelska. T.ex. "instal" istället för "install"
        {
            List<string> WordList = new List<string> {
                    "motif",
                    "version",
                    "crystal",
                    "analysis",
                    "resort",
                    "stress",
                    "second",
                    "object",
                    "exploit",
                    "bill",
                    "hay",
                    "mass",
                    "migration",
                    "resolution",
                    "blank",
                    "question",
                    "knife",
                    "stereotype",
                    "tasty",
                    "harmful",
                    "boat",
                    "satellite",
                    "flex",
                    "gutter",
                    "commitment",
                    "victory",
                    "glow",
                    "convenience",
                    "mud",
                    "celebration",
                    "moment",
                    "proclaim",
                    "walk",
                    "negligence",
                    "debt",
                    "honor",
                    "experience",
                    "try",
                    "burn",
                    "flawed",
                    "hard",
                    "capture",
                    "stool",
                    "realize",
                    "spy",
                    "flower",
                    "perform",
                    "bounce",
                    "drug",
                    "throne",
                    "law","" +
                    "scatter","" +
                    "breakfast",
                    "fixture",
                    "still",
                    "embryo",
					"gap",
					"rabbit",
					"mother",
					"ankle",
					"question",
					"fail",
					"superintendent",
					"scandal",
					"enlarge",
					"assumption",
					"cart",
					"charge",
					"dome",
					"quotation",
					"hate",
					"tract",
					"ferry",
					"wriggle",
					"multimedia",
					"curtain",
					"attitude",
					"gown",
					"location",
					"helpless",
					"damage",
					"scale",
					"haircut",
					"railcar",
					"undertake",
					"freighter",
					"thought",
					"minority",
					"art",
					"mathematics",
					"mole",
					"car",
					"interrupt",
					"calorie",
					"instal",
					"respect",
					"demonstrator",
					"operational",
					"magnetic",
					"trail"
                };
            Random rand = new Random();
            int WordNr = rand.Next(0, WordList.Count);

            string HangWord = WordList[WordNr];
            return HangWord;
        }
    }
}
