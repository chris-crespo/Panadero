using System.Linq;
using Panadero.Models;

namespace Panadero.Data;

public class SaleRepo : IRepository<Sale>
{
    string _file = "data/sales.csv";

    public void Save(List<Sale> sales)
    {
        var header = "Name,Units,Price,Date";
        File.WriteAllLines(_file, sales.Select(p => p.ToString()).Prepend(header));
    }

    public List<Sale> Read()
        => File.ReadAllLines(_file)
            .Skip(1)
            .Where(line => line.Length > 0)
            .Select(Sale.Parse)
            .ToList();
}
