namespace Panadero.Models;

public class Mapper
{
    public BakerProductDTO MapProductForBaker(Product product)
        => new BakerProductDTO(product.Name, product.Units);

    public AssistantProductDTO MapProductForAssistant(Product product)
        => new AssistantProductDTO(product.Name, product.Price);
}