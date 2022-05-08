using Panadero.Models;

namespace Panadero.Data;

public class ProductRepo : IRepository<Product>
{
    string _file = "data/products.csv";

    public void Save(List<Product> products)
    {
        var header = "Nombre,Precio,Unidades";
        var data   = new List<string>() { header };
        products.ForEach(product => data.Add(product.ToString()));
        File.WriteAllLines(_file, data);
    }

    public List<Product> Read()
        => File.ReadAllLines(_file)
            .Skip(1)
            .Where(line => line.Length > 0)
            .Select(Product.Parse)
            .ToList();
}
