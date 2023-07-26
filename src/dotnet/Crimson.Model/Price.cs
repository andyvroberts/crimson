using System.Text.Json.Serialization; 

namespace Crimson.Model
{
    public class PriceRecord
    {
        public string Outcode { get; set; } = String.Empty;
        public string Postcode { get; set; } = String.Empty;
        public string Address { get; set; } = String.Empty;
        public string Price { get; set; } = String.Empty;
        public string Date { get; set; } = String.Empty;
        public string PropertyType { get; set; } = String.Empty;
        public string NewBuild { get; set; } = String.Empty;
        public string Duration { get; set; } = String.Empty;
        public string Locality { get; set; } = String.Empty;
        public string Town { get; set; } = String.Empty;
        public string District { get; set; } = String.Empty;
        public string County { get; set; } = String.Empty;

        public override string ToString()
        {
            return $"{Outcode}|{Postcode}|{Address}|{Price}|{Date}|{Locality}|{Town}|{District}|{County}|{PropertyType}|{Duration}|{NewBuild}";
        }
    }

    public class PropertyPrice
    {
        [JsonPropertyName("pr")]
        public string Price { get; }
        [JsonPropertyName("pd")]
        public string PriceDate { get; }

        public PropertyPrice(string price, string date)
        {
            Price = price;
            PriceDate = date;
        }
    }

    public class PropertyDetails
    {
        [JsonIgnore]
        public string Address { get; }
        [JsonPropertyName("at")]
        public string Town { get; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("al")]
        public string Locality { get; }
        [JsonPropertyName("af")]
        public string Flags { get; } 
        [JsonPropertyName("ap")]
        public List<PropertyPrice> Prices { get; set; }

        public PropertyDetails(string address, string town, string locality, string flags)
        {
            Address = address;
            Town = town;
            Locality = locality;
            Flags = flags;
            Prices = new();
        }
    }

    public class UkLandCsvRecord
    {
        public string? Key { get; set; }
        public string? Price { get; set; }
        public string? PriceDate { get; set; }
        public string? Postcode { get; set; }
        public string? PropertyType { get; set; }
        public string? NewBuild { get; set; }
        public string? Duration { get; set; }
        public string? Paon { get; set; }
        public string? Saon { get; set; }
        public string? Street { get; set; }
        public string? Locality { get; set; }
        public string? Town { get; set; }
        public string? District { get; set; }
        public string? County { get; set; }
        public string? PPDCategory { get; set; }
        public string? RecordStatus { get; set; }
    }

}