namespace Mascotas.Models;

public class Mapper
{
    public PetDTO MapPet(Pet pet, List<Member> members, List<Specie> species)
    {
        var owner = members.Find(m => m.ID.Equals(pet.MemberID));
        var specie = species.Find(s => s.ID.Equals(pet.SpecieID));
        return new PetDTO(pet.ID, pet.Name, specie.Name, pet.Birthdate, owner?.Name);
    }

    public List<PetDTO> MapPets(List<Pet> pets, List<Member> members, List<Specie> species)
        => pets.Select(pet => MapPet(pet, members, species)).ToList();

    public MemberDTO MapMember(Member member)
        => new MemberDTO(member.ID, member.Name, member.Gender);

    public List<MemberDTO> MapMembers(List<Member> members)
        => members.Select(MapMember).ToList();

    public SpecieDTO MapSpecie(Specie specie) 
        => new SpecieDTO(specie.ID, specie.Name);

    public List<SpecieDTO> MapSpecies(List<Specie> species)
        => species.Select(MapSpecie).ToList();

}
