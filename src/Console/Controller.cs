using static System.Console;
using Panadero.Models;

namespace Panadero.UI.Console;

enum TerminalMode { None, Assistant, Baker };

public class Controller 
{
    private View _view;
    private Sys _sys;
    private Dictionary<(string title, TerminalMode mode), Action> _useCases;
    private TerminalMode _mode;
    //private Mapper _mapper;

    public Controller(View view, Sys sys/*, Mapper mapper*/)
    {
        _view = view;
        _sys = sys;
        //_mapper = mapper;
        _mode = TerminalMode.None;
        _useCases = new Dictionary<(string, TerminalMode), Action>() {
            { ("Panadero",                   TerminalMode.None),         SetBakerMode },
            { ("Dependiente",                TerminalMode.None),         SetAssistantMode },

            { ("Añadir producto",            TerminalMode.Baker),        AddProduct },
            { ("Eliminar producto",          TerminalMode.Baker),        RemoveProduct },
            { ("Mostrar producción del día", TerminalMode.Baker),        ShowTodaysProduction },

            { ("Vender producto",            TerminalMode.Assistant),    SellProduct },
            { ("Añadir pedido",              TerminalMode.Assistant),    AddOrder },
            { ("Eliminar pedido",            TerminalMode.Assistant),    RemoveOrder },
            { ("Mostrar ingresos del mes",   TerminalMode.Assistant),    ShowIncome }
        };
    }

    public void Run() 
    {
        while (true) 
        {
            try 
            {
                _view.ClearScreen();

                var menu = getMenuOptions();
                var option = _view.TryGetListItem("Menu", menu, "Selecciona una opcion");
                _view.Show("");

                var useCase = _useCases.FirstOrDefault(k => k.Key.title == option).Value;
                useCase.Invoke();

                _view.ShowAndReturn("Pulsa <Return> para continuar", ConsoleColor.DarkGray);
            }
            catch { return; }
        }
    }

    private List<String> getMenuOptions() => _useCases.Keys
        .Where(t => t.mode == _mode)
        .Select(t => t.title)
        .ToList<String>();

    private void SetAssistantMode() => _mode = TerminalMode.Assistant;
    private void SetBakerMode() => _mode = TerminalMode.Baker;

    private void AddProduct()
    {
        try 
        {
            var product = new Product(
                name:  _view.TryGetInput<string>("Nombre"),
                price: _view.TryGetInput<decimal>("Precio"),
                units: _view.TryGetInput<int>("Unidades")
            );

            _sys.AddOrModifyProduct(product);
        }
        catch (Exception e)
        {
            _view.Show(e.Message, ConsoleColor.DarkRed);
        }
    }

    private void RemoveProduct()
    {}

    private void SellProduct() 
    {
        try
        {
            //var products = _sys.Products;
        }
        catch (Exception e)
        {
            _view.Show($"UC: {e.Message}");
        }
    }

    private void AddOrder()
    {}

    private void ModifyOrder()
    {}

    private void RemoveOrder()
    {}

    private void ShowTodaysProduction()
    {}

    private void ShowIncome()
    {}

    /*
    private void AddMember() 
    {
        try
        {
            var member = new Member(
                id:     Guid.NewGuid(),
                name:   _view.TryGetInput<string>("Nombre"),
                gender: _view.TryGetChar("Sexo", "HM", 'H')
            );

            _manager.AddMember(member);
        }
        catch (Exception e)
        {
            _view.Show($"UC: {e.Message}");
        }
    }

    private void RemoveMember() 
    {
        try
        {
            var members = _manager.Members;
            if (members.Count == 0)
                throw new Exception("No hay ningún socio registrado");

            var member = _view.TryGetListItem(
                "Socios", 
                _mapper.MapMembers(members), 
                "Selecciona un socio"); 
            _manager.RemoveMemberById(member.ID);
        }
        catch (Exception e)
        {
            _view.Show(e.Message, ConsoleColor.DarkRed);
        }
    }

    private void AddPet()
    {
        try
        {
            var species = _mapper.MapSpecies(_manager.Species);
            if (species.Count == 0)
                throw new Exception("No hay ninguna especie registrada.");

            var pet = new Pet(
                id:        Guid.NewGuid(),
                name:      _view.TryGetInput<string>("Nombre"),
                gender:    _view.TryGetChar("Sexo", "HM", 'H'),
                specieID:  _view.TryGetListItem("Especies", species, "Selecciona una especie").ID,
                birthdate: _view.TryGetDate("Fecha de Nacimiento"),
                memberID:  null
            );
            _manager.AddPet(pet);
        }
        catch (Exception e)
        {
            _view.Show(e.Message, ConsoleColor.DarkRed);
        }
    }

    private void RemovePet() 
    {
        try
        {
            var pets = _manager.Pets;
            if (pets.Count == 0)
                throw new Exception("No hay ninguna mascota registrada");

            var members = _manager.Members;
            var species = _manager.Species;
            var pet = _view.TryGetListItem(
                "Mascotas", 
                _mapper.MapPets(pets, members, species) , 
                "Selecciona una mascota"); 
            _manager.RemovePetById(pet.ID);
        }
        catch (Exception e)
        {
            _view.Show(e.Message, ConsoleColor.DarkRed);
        }
    }

    private void AddSpecie()
    {
        try
        {
            var specie = new Specie(
                id:   Guid.NewGuid(),
                name: _view.TryGetInput<string>("Nombre")
            );

            if (_manager.ExistsSpecie(specie.Name))
                throw new Exception("Ya existe una especie con el mismo nombre");

            _manager.AddSpecie(specie);
        }
        catch (Exception e)
        {
            _view.Show(e.Message, ConsoleColor.DarkRed);
        }
    }

    private void RemoveSpecie() 
    {
        try
        {
            var species = _manager.Species;
            if (species.Count == 0)
                throw new Exception("No hay especies registradas");

            var specie = _view.TryGetListItem(
                "Especies", 
                _mapper.MapSpecies(species), 
                "Selecciona una especie"); 

            var pets = _manager.GetPetsWithSpecieID(specie.ID);
            if (pets.Count != 0)
                throw new Exception("Existen mascotas registradas de la especie seleccionada");

            _manager.RemoveSpecieById(specie.ID);
        }
        catch (Exception e)
        {
            _view.Show(e.Message, ConsoleColor.DarkRed);
        }
    }

    private void ShowSpecies() 
    {
        try
        {
            var species = _manager.Species;
            if (species.Count == 0)
                throw new Exception("No hay especies registradas");

            _view.ShowList("Especies", _mapper.MapSpecies(species));
        }
        catch (Exception e)
        {
            _view.Show(e.Message, ConsoleColor.DarkRed);
        }
    }

    private void BuyPet()
    {
        try
        {
            if (_manager.Members.Count == 0)
                throw new Exception("No hay socios registrados");

            var pets = _manager.GetAvailablePets();
            if (pets.Count == 0)
                throw new Exception("No hay mascotas disponibles");

            var members = _manager.Members;
            var species = _manager.Species;
            var pet = _view.TryGetListItem(
                "Mascotas", 
                _mapper.MapPets(pets, members, species), 
                "Selecciona una mascota");
            var member = _view.TryGetListItem(
                "Socios", 
                _mapper.MapMembers(members), 
                "Selecciona un socio");
            _manager.ChangePetOwner(pet.ID, member.ID);
        }
        catch (Exception e)
        {
            _view.Show(e.Message, ConsoleColor.DarkRed);
        }
    }

    private List<PetDTO> sortPets(List<PetDTO> pets) 
        => pets.OrderBy(p => p.Specie).ThenBy(p => p.Age).ToList();

    private void ShowPets() 
    {
        try
        {
            var pets = _manager.Pets;
            if (pets.Count == 0)
                throw new Exception("No hay mascotas registradas");

            var members = _manager.Members;
            var species = _manager.Species;
            var mappedPets = _mapper.MapPets(pets, members, species);
            _view.ShowList("Mascotas", sortPets(mappedPets));
        }
        catch (Exception e)
        {
            _view.Show(e.Message, ConsoleColor.DarkRed);
        }
    }

    private void ShowMembers()
    {
        try
        {
            var members = _mapper.MapMembers(_manager.Members);
            if (members.Count == 0)
                throw new Exception("No hay socios registrados");

            var member = _view.TryGetListItem("Socios", members, "Selecciona un socio");
            var pets = _mapper.MapPets(
                _manager.GetPetsByOwnerID(member.ID), _manager.Members, _manager.Species);
            if (pets.Count == 0)
                throw new Exception($"{member.Name} no tiene ninguna mascota registrada");

            _view.Show("");
            _view.ShowList("Mascotas", sortPets(pets));
        }
        catch (Exception e)
        {
            _view.Show(e.Message, ConsoleColor.DarkRed);
        }
    }
    */
}
