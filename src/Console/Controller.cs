using static System.Console;
using Panadero.Models;
using Panadero.Extensions;

namespace Panadero.UI.Console;

enum TerminalMode { None, Assistant, Baker };

public class Controller 
{
    private View _view;
    private Sys _sys;
    private Dictionary<(string title, TerminalMode mode), Action> _useCases;
    private TerminalMode _mode;
    private Mapper _mapper;

    public Controller(View view, Sys sys, Mapper mapper)
    {
        _view = view;
        _sys = sys;
        _mapper = mapper;
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

    private void handled(Action fn, Action<Exception> handler) 
    {
        try 
        {
            fn();
        }
        catch (Exception e)
        {
            handler(e);
        }
    }

    private void defaultHandler(Exception e) => _view.Show($"UC: {e.Message}");
    private void withDefaultHandler(Action fn) => handled(fn, defaultHandler);

    private void AddProduct() => withDefaultHandler(() => {
        var product = new Product(
            Name:  _view.TryGetInput<string>("Nombre"),
            Price: _view.TryGetInput<decimal>("Precio"),
            Units: _view.TryGetInput<int>("Unidades")
        );

        _sys.AddOrModifyProduct(product);
    });


    private T tryGetProduct<T>(Func<Product, T> map) => _view.TryGetListItem(
        "Productos",
        _sys.Products.Select(map).ToList(),
        "Selecciona un producto"
    );

    private void RemoveProduct() 
        => withDefaultHandler(() => _sys.RemoveProduct(tryGetProduct(p => p.Name)));

    private void ShowTodaysProduction() => withDefaultHandler(() => {
        var orderedProducts = _sys.Orders
            .Where(order => order.DeliverDate == null || order.DeliverDate?.Date == DateTime.Today)
            .SelectMany(order => order.Products);

        var all = _sys.Products.Concat(orderedProducts)
            .Select(_mapper.MapProductForBaker)
            .ToList();

        _view.ShowList("Producción", all);
    });

    private void SellProduct() => withDefaultHandler(() => {
        var total = 0M;

        while (true) 
        {
            var product = tryGetProduct(_mapper.MapProductForAssistant);
            var units = _view.TryGetInput<int>("Unidades");

            var price = _sys.SellProduct(product.Name, units);
            total += price;

            var c = _view.TryGetChar("Añadir otro producto (S/N)", "SN", 'S');
            if (c == 'N') break;
        }

        WriteLine($"Total: {total}€");
    });

    private void AddOrder() => withDefaultHandler(() => {
        var client = _view.TryGetInput<string>("Cliente");
        var products = new List<Product>();
        
        while (true)
        {
            var product = new Product(
                Name: _view.TryGetInput<string>("Producto"),
                Units: _view.TryGetInput<int>("Unidades"),
                Price: _view.TryGetInput<decimal>("Precio")
            );

            products.Add(product);

            var c = _view.TryGetChar("Añadir otro producto (S/N)", "SN", 'S');
            if (c == 'N') break;
        }

        var id = Guid.NewGuid();
        var orderDate = DateTime.Now;

        var daily = _view.TryGetChar("Diario (S/N)", "SN", 'S');
        DateTime? deliverDate = daily == 'N'
            ? _view.TryGetDate("Fecha de entrega")
            : null;

        WriteLine($"Total: {products.Sum(product => product.Price * product.Units)}€");
        var order = new Order(id, client, products, orderDate, deliverDate);
        _sys.AddOrder(order);
    });

    private void RemoveOrder() => withDefaultHandler(() => {
        var orders = _sys.Orders
            .Select(_mapper.MapOrder)
            .ToList();

        if (orders.Count == 0)
            _view.Show("No hay pedidos registrados"); 
        else {
            var order = _view.TryGetListItem("Pedidos", orders, "Selecciona un pedido");
            _sys.RemoveOrdersWithId(order.Id);
        }
    });

    private bool deliveredThisMonth(Order order)    
    {
        var (thisYear, thisMonth, thisDay) = DateTime.Now;

        // DeliverDate nunca debería se null si antes descartamos los pedidos diarios.
        var (year, month, day) = order.DeliverDate ?? DateTime.Now;

        return year == thisYear && month == thisMonth && day <= thisDay;
    }

    private IEnumerable<Sale> salesFromOrder(Order order) 
        => order.Products.Select(product => new Sale(
            product.Name,
            product.Units,
            product.Price * product.Units,
            order.DeliverDate ?? DateTime.Now
        ));

    private void ShowIncome() => withDefaultHandler(() => {
        var (thisYear, thisMonth, thisDay) = DateTime.Now;
        var sales = _sys.Sales
            .Where(sale => sale.Date.Year == thisYear && sale.Date.Month == thisMonth);

        var dailyOrders = _sys.Orders
            .Where(order => order.DeliverDate == null)
            .SelectMany(salesFromOrder);

        var nonDailyOrders = _sys.Orders
            .Where(order => order.DeliverDate != null)
            .Where(deliveredThisMonth)
            .SelectMany(salesFromOrder);

        var all = sales.Concat(dailyOrders).Concat(nonDailyOrders)
            .Select(_mapper.MapSale)
            .ToList();

        if (all.Count() == 0)
            _view.Show("Aún no se ha registrado ningún ingreso");
        else 
        {
            _view.ShowList("Ingresos del mes", all);
            _view.Show($"Total: {all.Sum(sale => sale.Price)}");
        }
    });
}
