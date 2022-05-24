using Panadero.Models;
using Panadero.Data;

namespace Panadero;

public class Sys
{
    ProductRepo ProductRepo;
    OrderRepo OrderRepo;

    public List<Product> Products { get; }
    public List<Order> Orders { get; }

    public Sys(ProductRepo productRepo, OrderRepo orderRepo)
    {
        ProductRepo = productRepo;
        OrderRepo = orderRepo;

        Products = productRepo.Read();
        Orders = orderRepo.Read();
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
        return product!.Price * units;
    }

    public void AddOrder(Order order) 
    {
        Orders.Add(order);
        OrderRepo.Save(Orders);
    }
}
