using static System.ConsoleColor;
using Crimson.Core;

namespace Crimson
{
    public class CrimsonConsole 
    {
        private readonly ICrimson _crimson;
        private bool _remainInConsole = true;

        public CrimsonConsole(ICrimson crimson)
        {
            _crimson = crimson;
        }

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
                                string _scanStartsWith = InputScanString();
                                _crimson.Run(_scanStartsWith);
                                break;
                            case 2:
                                Console.WriteLine($"You choose to load everything");
                                break;
                        }
                    }
                    else 
                    {
                        //Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine($"Please enter a valid option.  You entered [{_opt}]");
                    }
                    Console.ForegroundColor = lastTextColour;
                }
            }
        }

        private static string InputScanString()
        {
            var _scanInput = string.Empty;

            Console.WriteLine("Enter a Scan value: ");
            var _temp = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(_temp))
                _scanInput = _temp;

            return _scanInput;
        }
    }
}