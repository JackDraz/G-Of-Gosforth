using System;

namespace Greggs.Products.Api.Models;

public class Product
{
    public string Name { get; set; }
    public decimal PriceInPounds { get; set; }
    public ProductEUR ProductGBPtoEUR()
    {
        return new ProductEUR
        {
            Name = this.Name,
            PriceInPounds = this.PriceInPounds,
            PriceInEuros = Decimal.Round(this.PriceInPounds * ProductEUR.CURRENT_EU_EXCHANGE_RATE, 2, MidpointRounding.ToEven)
        };
    }
}

// New subclass added as solution given current the assignment parameters and prospective nature of EU interest. 
// Would suggest this data would eventually be stored and retrieved as appropriate within a database represented by ProductAccess.
public class ProductEUR : Product
{
    internal static readonly decimal CURRENT_EU_EXCHANGE_RATE = 1.11m;
    public decimal PriceInEuros { get; set; }
}   