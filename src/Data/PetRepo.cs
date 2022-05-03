using Mascotas.Models;

namespace Mascotas.Data;

public class PetRepo : IRepository<Pet>
{
    string _file = "data/pets.csv";

    public void Save(List<Pet> pets) 
    {
        var header = "ID,Nombre,Sexo,Especie,Nacimiento,Dueño";
        var data   = new List<string>() { header };
        pets.ForEach(pet => data.Add(pet.ToString())); 
        File.WriteAllLines(_file, data);
    }

    public List<Pet> Read()
        => File.ReadAllLines(_file)
            .Skip(1)
            .Where(line => line.Length > 0)
            .Select(Pet.Parse)
            .ToList();
}
