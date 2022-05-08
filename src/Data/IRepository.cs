namespace Panadero.Data;

public interface IRepository<T>
{
    void Save(List<T> items);
    List<T> Read();
}
