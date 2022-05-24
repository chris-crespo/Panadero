using Panadero.Models;
using Panadero.Data;

namespace Panadero;

public class Sys
{
    ProductRepo ProductRepo;
    OrderRepo OrderRepo;
    SaleRepo SaleRepo;

    public List<Product> Products { get; }
    public List<Order> Orders { get; }
    public List<Sale> Sales { get; }

    public Sys(ProductRepo productRepo, OrderRepo orderRepo, SaleRepo saleRepo)
    {
        ProductRepo = productRepo;
        OrderRepo = orderRepo;
        SaleRepo = saleRepo;

        Products = productRepo.Read();
        Orders = orderRepo.Read();
        Sales = saleRepo.Read();
    }

    public void RemoveProduct(string name) 
    {
        Products.RemoveAll(p => p.Name == name);
        ProductRepo.Save(Products);
    }

    public void AddOrModifyProduct(Product product)
    {
        Products.RemoveAll(p => p.Name == product.Name);
        Products.Add(product);
        ProductRepo.Save(Products);
    }

    public decimal SellProduct(string name, int units) 
    {
        var product = Products.Find(p => p.Name == name);
        var price = product!.Price * units;
        var sale = new Sale(name, units, price, DateTime.Now);

        Sales.Add(sale);

        return product!.Price * units;
    }

    public void AddOrder(Order order) 
    {
        Orders.Add(order);
        OrderRepo.Save(Orders);
    }

    public void RemoveOrdersWithId(Guid id) 
    {
        Orders.RemoveAll(order => order.Id == id);
        OrderRepo.Save(Orders);
    }
}
