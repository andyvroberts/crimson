
namespace Crimson.Model
{
    public class PriceRecord
    {
        public string? Postcode { get; set; }
        public string? Address { get; set; }
        public string? Price { get; set; }
        public string? Date { get; set; }
        public string? PropertyType { get; set; }
        public string? NewBuild { get; set; }
        public string? Duration { get; set; }
        public string? Locality { get; set; }
        public string? Town { get; set; }
        public string? District { get; set; }
        public string? County { get; set; }

        public override string ToString()
        {
            return $"{Postcode}|{Address}|{Price}|{Date}|{Locality}|{Town}|{District}|{County}|{PropertyType}|{Duration}|{NewBuild}";
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