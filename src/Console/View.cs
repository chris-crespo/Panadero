using System.ComponentModel;
using static System.Console;

namespace Panadero.UI.Console;

public class View 
{
    const string CANCELINPUT = "fin";

    public void ClearScreen() => Clear(); 

    public void Show(Object obj, ConsoleColor color = ConsoleColor.White) 
    {
        ForegroundColor = color;
        WriteLine(obj.ToString());
        ForegroundColor = ConsoleColor.White;
    }

    public void ShowAndReturn(Object obj, ConsoleColor color = ConsoleColor.White) 
    {
        ForegroundColor = color;
        Write(obj.ToString() + " ");
        ForegroundColor = ConsoleColor.White;
        ReadLine();
    }

    public void ShowList<T>(string title, List<T> items)
    {
        Show(title, ConsoleColor.Red);
        WriteLine();
        for (int i = 0; i < items.Count; i++)
            WriteLine($"  {i + 1,3:###}.- {items[i].ToString()}");
        WriteLine();
    }

    public T TryGetInput<T>(string prompt) 
    {
        var msg = prompt.Trim() + ": ";
        while (true)
        {
            Write(msg);

            var input = ReadLine();
            if (input.ToLower().Trim() == CANCELINPUT)
                throw new Exception("Entrada cancelada por el usuario.");

            try 
            {
                return (T)TypeDescriptor
                    .GetConverter(typeof(T))
                    .ConvertFromString(input);
            }
            catch (Exception)
            {
                if (input != "")
                    Show($"Error: '{input}' no permitido", 
                         ConsoleColor.DarkRed);
            }
        }
    }   

    public char TryGetChar(string prompt, string options, char def = 'H')
    {
        var msg = $"{prompt.Trim()} ({def}): ";
        while (true)
        {
            Write(msg);
            var input = ReadLine();
            if (input.ToLower().Trim() == CANCELINPUT)
                throw new Exception("Entrada cancelada");
            if (input == "")
                input = def.ToString();

            try
            {
                if (input.Length != 1) 
                    throw new Exception();

                var c = input.ToUpper()[0];
                if (!options.Contains(c))
                    throw new Exception();

                return c;
            }
            catch (Exception)
            {
                Show($"Error: '{input}' no se encuentra entre las opciones {options}", ConsoleColor.DarkRed);
            }
        }
    }

    public DateTime TryGetDate(string prompt)
    {
        var msg = $"{prompt} (mm/dd/yyyy): ";
        while (true)
        {
            Write(msg); 

            var input = ReadLine();
            if (input.ToLower().Trim() == CANCELINPUT)
                throw new Exception("Entrada cancelada");

            try
            {
                var date = DateTime.Parse(input);
                if (date.Date <= DateTime.Now.Date)
                    throw new Exception();

                return date;
            }
            catch (Exception)
            {
                if (input != "")
                    Show($"Error: '{input}' no una fecha válida", ConsoleColor.DarkRed);
            }
        }
    }

    public int TryGetIntInRange(int lower, int upper, string prompt)
    {
        var input = int.MaxValue;
        while (lower > input || input > upper)
        {
            try
            {
                input = TryGetInput<int>(prompt);
            }
            catch (Exception)
            {
                throw;
            }
        }

        return input;
    }

    public T TryGetListItem<T>(string title, List<T> items, string prompt)
    {
        ShowList(title, items);
        try 
        {
            var input = TryGetIntInRange(1, items.Count, prompt);
            return items[input - 1];
        }
        catch (Exception)
        {
            throw;
        }
    }
}
