#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
namespace HeyGPT.Core.Models;

// Model for the SampleDataService. Replace with your own model.
public class SampleOrderDetail
{
    public long ProductID
    {
        get; set;
    }

    public string ProductName
    {
        get; set;
    }

    public int Quantity
    {
        get; set;
    }

    public double Discount
    {
        get; set;
    }

    public string QuantityPerUnit
    {
        get; set;
    }

    public double UnitPrice
    {
        get; set;
    }

    public string CategoryName
    {
        get; set;
    }

    public string CategoryDescription
    {
        get; set;
    }

    public double Total
    {
        get; set;
    }

    public string ShortDescription => $"Product ID: {ProductID} - {ProductName}";
}
