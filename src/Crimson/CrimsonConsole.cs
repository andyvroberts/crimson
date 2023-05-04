using static System.ConsoleColor;

namespace Crimson
{
    public class CrimsonConsole 
    {
        private bool _remainInConsole = true;

        public void Run()
        {
            int actionChoice = 0;
            var lastTextColour = Console.ForegroundColor;

            while (_remainInConsole)
            {
                Console.WriteLine("1.   Scan by StartsWith");
                Console.WriteLine("2.   Load All");
                Console.WriteLine("X.   Exit");

                string? _opt = Console.ReadLine();

                if (_opt is not null && _opt.ToUpper() == "X")
                {
                    _remainInConsole = false;
                }
                else 
                {
                    if (int.TryParse(_opt, out actionChoice))
                    {
                        switch (actionChoice)
                        {
                            case 1:
                                Console.WriteLine($"You choose to scan with a string");
                                break;
                            case 2:
                                Console.WriteLine($"You choose to load everything");
                                break;
                        }
                    }
                    else 
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine($"Please enter a valid option.  You entered [{_opt}]");
                    }
                    Console.ForegroundColor = lastTextColour;
                }
            }
        }
    }
}