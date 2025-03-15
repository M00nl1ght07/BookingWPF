string currencySymbol;
switch (currency)
{
    case "USD":
        currencySymbol = "$";
        break;
    case "EUR":
        currencySymbol = "€";
        break;
    default:
        currencySymbol = "₽";
        break;
} 