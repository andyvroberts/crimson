namespace Crimson.Model;

public class PostcodeSet
{
    public string Postcode { get; set; } = String.Empty;
    public List<PropertyDetails> Properties { get; set; } = new();

    public void AddPriceToPostcodeSet(PriceRecord rec)
    {
        var thisProp = Properties.SingleOrDefault(x => x.Address == rec.Address);

        if (thisProp != null)
        {
            PropertyPrice newPrice = new();
            newPrice.Price = rec.Price;
            newPrice.PriceDate = rec.Date;

            thisProp.Prices.Add(newPrice);
        }
        else
        {
            PropertyDetails newProp = new();
            PropertyPrice newPrice = new();

            newPrice.Price = rec.Price;
            newPrice.PriceDate = rec.Date;

            newProp.Address = rec.Address;
            newProp.Town = rec.Town;
            newProp.Locality = rec.Locality ?? rec.District ?? rec.County;
            newProp.Prices.Add(newPrice);

            Properties.Add(newProp);
        }
    }
}