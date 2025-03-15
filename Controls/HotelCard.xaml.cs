public decimal OriginalPrice { get; private set; }

public void UpdatePrice(decimal price, string currency)
{
    string currencySymbol = currency switch
    {
        "USD" => "$",
        "EUR" => "€",
        "GBP" => "£",
        _ => "₽"
    };

    PriceTextBlock.Text = $"От {price} {currencySymbol} за ночь";
} 