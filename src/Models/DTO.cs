namespace Panadero.Models;

public record BakerProductDTO(string Name, int Units) 
{
    public override string ToString() => $"{Name} - {Units} unidades";
}

public record AssistantProductDTO(string Name, decimal Price)
{
    public override string ToString() => $"{Name} {Price}€";
}

public record OrderDTO(Guid Id, string Client, DateTime OrderDate)
{
    public override string ToString() => $"{Client} - {OrderDate}";
}

public record SaleDTO(string Product, int Units, decimal Price)
{
    public override string ToString() => $"{Product} ({Units}) {Price}€";
}
