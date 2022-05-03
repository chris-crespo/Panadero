using Mascotas.Models;

namespace Mascotas.Data;

public class SpecieRepo : IRepository<Specie>
{
    string _file = "data/species.csv";

    public void Save(List<Specie> species)
    {
        var header = "ID,Nombre";
        var data   = new List<string>() { header };
        species.ForEach(specie => data.Add(specie.ToString()));
        File.WriteAllLines(_file, data);
    }

    public List<Specie> Read()
        => File.ReadAllLines(_file)
            .Skip(1)
            .Where(line => line.Length > 0)
            .Select(Specie.Parse)
            .ToList();
}
