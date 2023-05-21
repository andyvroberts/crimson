using static System.ConsoleColor;
using System.Diagnostics;
using Crimson.Core;
using Crimson;

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

        public async Task RunAsync()
        {
            int actionChoice = 0;
            var lastTextColour = Console.ForegroundColor;

            while (_remainInConsole)
            {
                Stopwatch priceTimer = new();

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
                        var _scanStartsWith = string.Empty;

                        priceTimer.Start();
                        switch (actionChoice)
                        {
                            case 1:
                                Console.WriteLine($"You choose to scan with a string");
                                _crimson.Run(InputScanString());
                                break;
                            case 2:
                                Console.WriteLine($"You choose to load everything");
                                await _crimson.RunAsync();
                                break;
                        }
                        priceTimer.Stop();
                        Console.WriteLine($"Loader ran in {priceTimer.Elapsed} elapsed.");
                    }
                    else
                    {
                        //Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine($"Please enter a valid option.  You entered [{_opt}]");
                        //Console.ForegroundColor = lastTextColour;
                    }
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