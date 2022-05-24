namespace Panadero.Models;

public class Mapper
{
    public BakerProductDTO MapProductForBaker(Product product)
        => new BakerProductDTO(product.Name, product.Units);

    public AssistantProductDTO MapProductForAssistant(Product product)
        => new AssistantProductDTO(product.Name, product.Price);

    public OrderDTO MapOrder(Order order)
        => new OrderDTO(order.Id, order.Client, order.OrderDate);

    public SaleDTO MapSale(Sale sale)
        => new SaleDTO(sale.Product, sale.Units, sale.Price);
}
