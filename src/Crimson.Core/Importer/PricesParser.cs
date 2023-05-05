using System.Text;
using Crimson.Model;

namespace Crimson.Core.Importer
{
    public class PricesParser : IPricesParser
    {
        public PriceRecord? ConvertCsvLine(string line)
        {
            PriceRecord price = new();
            var nextLine = DecodeCsvLine(line);

            if (line != null)
            {
                if (string.IsNullOrEmpty(nextLine[3]))
                {
                    return null;
                }
                else
                {
                    StringBuilder address = new();
                    if (!string.IsNullOrWhiteSpace(nextLine[7])) address.Append(nextLine[7] + ' ');
                    if (!string.IsNullOrWhiteSpace(nextLine[8])) address.Append(nextLine[8] + ' ');
                    if (!string.IsNullOrWhiteSpace(nextLine[9])) address.Append(nextLine[9]);

                    price.Outcode = nextLine[3].ToUpper().Split(' ')[0];
                    price.Postcode = nextLine[3].ToUpper();
                    price.Address = address.ToString().TrimEnd();
                    price.Price = nextLine[1];
                    price.Date = nextLine[2].Substring(0, 10);
                    price.PropertyType = nextLine[4];
                    price.NewBuild = nextLine[5];
                    price.Duration = nextLine[6];
                    price.Locality = nextLine[10];
                    price.Town = nextLine[11];
                    price.District = nextLine[12];
                    price.County = nextLine[13];
                }
            }
            return price;
        }


        private List<string> DecodeCsvLine(string csvLine)
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