using Mascotas.Models;

namespace Mascotas.Data;

public class MemberRepo : IRepository<Member>
{
    string _file = "data/members.csv";

    public void Save(List<Member> members)
    {
        var header = "ID,Nombre,Sexo";
        var data   = new List<string>() { header };
        members.ForEach(member => data.Add(member.ToString()));
        File.WriteAllLines(_file, data);
    }

    public List<Member> Read()
        => File.ReadAllLines(_file)
            .Skip(1)
            .Where(line => line.Length > 0)
            .Select(Member.Parse)
            .ToList();
}
