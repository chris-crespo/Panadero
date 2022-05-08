namespace Panadero.Models;

public class BakerProductDTO
{
    public string Name  { get; }
    public int    Units { get; }

    public BakerProductDTO(string name, int units)
    {
        Name  = name;
        Units = units;
    }

    public override string ToString() => $"{Name}\t {Units} unidades";
}

public class AssistantProductDTO
{
    public string  Name  { get; }
    public decimal Price { get; }

    public AssistantProductDTO(string name, decimal price)
    {
        Name  = name;
        Price = price;
    }

    public override string ToString() => $"{Name} {Price}€";
}
