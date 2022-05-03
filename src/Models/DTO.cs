namespace Mascotas.Models;

public class MemberDTO
{
    public Guid   ID     { get; }
    public string Name   { get; }
    public char   Gender { get; }

    public MemberDTO(Guid id, string name, char gender) 
    {
        ID     = id;
        Name   = name;
        Gender = gender;
    }

    public override string ToString() => $"{Name}, {Gender}";
}

public class PetDTO
{
    public   Guid     ID      { get; }
    public   string   Name    { get; }
    public   string   Specie  { get; }
    public   int      Age     { get; }
    public   string?  Member  { get; }

    public PetDTO(Guid id, string name, string specie, DateTime birthdate, string? member)
    {
        ID        = id;
        Name      = name;
        Specie    = specie;
        Age       = DateTime.Today.Year - birthdate.Year;
        Member    = member;
    }

    public override string ToString()
        => $"{Name}, {Specie}, {Age} años, ({Member ?? "Sin dueño"})";
}

public class SpecieDTO
{
    public Guid   ID   { get; }
    public string Name { get; }

    public SpecieDTO(Guid id, string name)
    {
        ID   = id;
        Name = name;
    }

    public override string ToString() => Name;
}
