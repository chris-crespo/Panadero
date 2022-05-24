namespace Panadero.Models;

public record Product(string Name, decimal Price, int Units)
{
    public static Product Parse(string str)
    {
        var fields = str.Split(",");
        return new Product(
            Name: fields[0],
            Price: decimal.Parse(fields[1]),
            Units: int.Parse(fields[2])
        );
    }

    public override string ToString() => $"{Name},{Price},{Units}";
}

public record Order(Guid Id, string Client, List<Product> Products, DateTime OrderDate, DateTime? DeliverDate)
{
    public static Order Parse((string, List<string>) order)
    {
        var (id, lines) = order;
        var fields = lines[0].Split(",");
        var client = fields[0];
        var orderDate = DateTime.Parse(fields[4]);
        DateTime? deliverDate = fields[5].Length > 0 ? DateTime.Parse(fields[5]) : null;

        return new Order(
            Id: Guid.Parse(id),
            Client: client,
            Products: lines
                .Select(line => line.Split(","))
                .Select(fields => new Product(
                    Name: fields[1], 
                    Price: decimal.Parse(fields[2]), 
                    Units: int.Parse(fields[3])
                ))
                .ToList(),
            OrderDate: orderDate,
            DeliverDate: deliverDate
        );
    }

    public override string ToString() 
        => string.Join("\n", 
            Products.Select(product 
                => $"{Id},{Client},{product},{OrderDate},{DeliverDate?.ToString() ?? ""}"));
}

public record Sale(string Product, int Units, decimal Price, DateTime Date)
{
    public static Sale Parse(string str)
    {
        var fields = str.Split(",");
        return new Sale(
            Product: fields[0],
            Units: int.Parse(fields[1]),
            Price: decimal.Parse(fields[2]),
            Date: DateTime.Parse(fields[3])
        );
    }

    public override string ToString() => $"{Product},{Units},{Price},{Date}";
}
