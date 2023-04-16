using System.Text;
using Crimson.Models;

namespace Crimson.Infra.PricesReader
{
    public class PricesParser : IPricesParser
    {
        public UkLandCsvRecord ConvertCsvLine(string line)
        {
            UkLandCsvRecord price = new();
            var nextLine = Convert(line);

            if (line != null)
            {
                price.Price = nextLine[1];
                price.PriceDate = nextLine[2].Substring(0, 10);
                price.Postcode = nextLine[3];
                price.PropertyType = nextLine[4];
                price.NewBuild = nextLine[5];
                price.Duration = nextLine[6];
                price.Paon = nextLine[7];
                price.Saon = nextLine[8];
                price.Street = nextLine[9];
                price.Locality = nextLine[10];
                price.Town = nextLine[11];
                price.District = nextLine[12];
                price.County = nextLine[13];
            }

            return price;
        }

        private List<string> Convert(string csvLine)
        {
            List<string> lines = new();
            StringBuilder column = new();
            bool withinCol = false;
            int quoteDistance = 0;
            int columnIndex = 0;

            const char comma = ',';
            const char doubleQuote = '"';

            foreach (Char c in csvLine.ToCharArray())
            {
                if (c == doubleQuote)
                {
                    if (withinCol)
                    {
                        // if already within a column and this is a double quote then it is the closing quote.
                        withinCol = false;
                        quoteDistance = 0;
                    }
                    else
                    {
                        // if not already within a column then this is the first double quote of a new column.
                        withinCol = true;
                        column = new StringBuilder();
                    }
                }
                else
                {
                    if (withinCol)
                    {
                        // any other character, including a comma that is within a column is a component of the column value.
                        column.Append(c);
                        quoteDistance++;
                    }
                    else if (c == comma)
                    {
                        // reached the end of a field as the comma is not within the column.
                        lines.Add(column.ToString());
                        columnIndex++;
                    }
                }
            }
            // if the char array is exhausted but the final text segment had no closing double quote then add it to the record.
            if (column.Length > 0)
                lines.Add(column.ToString());

            return lines;
        }
    }
}