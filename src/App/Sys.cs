using Panadero.Models;
using Panadero.Data;

namespace Panadero;

public class Sys
{
    ProductRepo ProductRepo;

    public List<Product> Products { get; }

    public Sys(ProductRepo productRepo)
    {
        ProductRepo = productRepo;

        Products = productRepo.Read();
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
    /*
    public void AddMember(Member m) 
    {
        Members.Add(m);
        MemberRepo.Save(Members);
    }

    public void RemoveMemberById(Guid id)
    {
        Members.RemoveAll(m => m.ID.Equals(id));
        Pets.RemoveAll(pet => pet.MemberID.Equals(id));
        MemberRepo.Save(Members);
        PetRepo.Save(Pets);
    }

    public void AddPet(Pet p)
    {
        Pets.Add(p);
        PetRepo.Save(Pets);
    }

    public void RemovePetById(Guid id)
    {
        Pets.RemoveAll(p => p.ID.Equals(id));
        PetRepo.Save(Pets);
    }

    public void AddSpecie(Specie s)
    {
        Species.Add(s);
        SpecieRepo.Save(Species);
    }

    public void RemoveSpecieById(Guid id)
    {
        Species.RemoveAll(s => s.ID.Equals(id));
        SpecieRepo.Save(Species);
    }

    public bool ExistsSpecie(string name) {
        return Species.Find(s => s.Name.Equals(name)) != null;
    }

    public List<Pet> GetPetsWithSpecieID(Guid id)
        => Pets.Where(pet => pet.SpecieID.Equals(id)).ToList();

    public List<Pet> GetAvailablePets()
        => Pets.Where(pet => pet.MemberID == null).ToList();

    public List<Pet> GetPetsByOwnerID(Guid id)
        => Pets.FindAll(pet => pet.MemberID.Equals(id));

    public void ChangePetOwner(Guid petID, Guid ownerID)
    {
        Pets.Find(p => p.ID.Equals(petID)).MemberID = ownerID;
        PetRepo.Save(Pets);
    }
    */
}
