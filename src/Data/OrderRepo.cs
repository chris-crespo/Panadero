using System.Linq;
using Panadero.Models;

namespace Panadero.Data;

public class OrderRepo : IRepository<Order>
{
    string _file = "data/orders.csv";

    public void Save(List<Order> orders)
    {
        var header = "Id,Client,Nombre,Precio,Unidades,OrderDate,DeliverDate";
        File.WriteAllLines(_file, orders.Select(p => p.ToString()).Prepend(header));
    }

    public List<Order> Read()
        => File.ReadAllLines(_file)
            .Skip(1)
            .Where(line => line.Length > 0)
            .Select(line => line.Split(",", 2))
            .GroupBy(arr => arr[0], arr => arr[1])
            .Select(g => (g.Key, g.ToList()))
            .Select(Order.Parse)
            .ToList();
}
