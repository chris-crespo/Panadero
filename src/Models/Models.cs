namespace Panadero.Models;

public class Product
{
    public string  Name  { get; }
    public decimal Price { get; }
    public int     Units { get; set; }

    public Product(string name, decimal price, int units)
    {
        Name  = name;
        Price = price;
        Units = units;
    }

    public static Product Parse(string str)
    {
        var fields = str.Split(",");
        return new Product(
            name: fields[0],
            price: Decimal.Parse(fields[1]),
            units: Int32.Parse(fields[2])
        );
    }

    public override string ToString() => $"{Name},{Price},{Units}";
}
