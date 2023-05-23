namespace Crimson.Model;

public class PriceSet
{
    public string SetValue { get; set; }
    public List<PropertyDetails> Properties { get; }

    public PriceSet (string setValue)
    {
        SetValue = setValue;
        Properties = new();
    }

    public void AddPriceToSet(PriceRecord rec)
    {
        var existingProp = Properties.SingleOrDefault(x => x.Address == rec.Address);

        if (existingProp != null)
        {
            PropertyPrice newPrice = new PropertyPrice(rec.Price, rec.Date);

            existingProp.Prices.Add(newPrice);
        }
        else
        {
            PropertyDetails newProp = new PropertyDetails(
                rec.Address,
                rec.Town,
                rec.Locality ?? rec.District ?? rec.County,
                rec.Duration + rec.NewBuild + rec.PropertyType
            );

            PropertyPrice newPrice = new PropertyPrice(rec.Price, rec.Date);
            newProp.Prices.Add(newPrice);

            Properties.Add(newProp);
        }
    }
}