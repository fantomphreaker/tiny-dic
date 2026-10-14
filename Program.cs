using System.Collections.Concurrent;
namespace TinyDic;
class Program
{
    private static readonly ConcurrentDictionary<string, string> _store = new();
    
    static void Main(string[] args)
    {
        Console.WriteLine("Tiny Dic started. Type commands (e.g., SET name alice, GET name).");

        // The READ, EVAL, PRINT  LOOP- REPL
        while(true)
        {
            Console.WriteLine("127.0.0.1:6379> ");
            string input = Console.ReadLine();
            if (string.IsNullOrEmpty(input))
                continue;

            // Split the input into parts: "SET mykey myvalue" -> ["SET", "mykey", "myvalue"]
            string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            string command = parts[0].ToUpper();

            switch(command)
            {
                case "SET":
                    // method to handle SET.
                    HandleSet(parts);
                    break;
                case "GET":
                    // method to handle GET.
                    HandleGet(parts);
                    break;
                case "DEL":
                    HandleDel(parts);
                    break;
                case "QUIT":
                case "EXIT":
                    return;
                default:
                    Console.WriteLine($"(error) ERR unknown command '{command}'");
                    break;
            }
        }

    }

    private static void HandleSet(string[] parts)
    {
        if (parts.Length < 3)
        {
            Console.WriteLine("(error) ERR wrong number of arguments for 'set' command");
            return;
        }

        string key = parts[1];
        string value = string.Join(" ", parts[2..]);

        _store[key] = value;
        Console.WriteLine("OK");
    }

    private static void HandleGet(string[] parts)
    {
        if (parts.Length < 2)
        {
            Console.WriteLine("(error) ERR wrong number of arguments for 'get' command");
            return;
        }

        string key = parts[1];

        if (_store.TryGetValue(key, out string value))
        {

            Console.WriteLine($"\"{value}\"");
        }
        else
        {
            Console.WriteLine("(nil)");
        }
    }

    private static void HandleDel(string[] parts)
    {
        if (parts.Length < 2)
        {
            Console.WriteLine("(error) ERR wrong number of arguments for 'del' command");
            return;
        }

        string key = parts[1];

        // TryRemove returns true if the key existed and was deleted
        if (_store.TryRemove(key, out _))
        {
            Console.WriteLine("(integer) 1");
        }
        else
        {
            Console.WriteLine("(integer) 0");
        }
    }
}