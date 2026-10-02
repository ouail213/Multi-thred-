using System.Net;
using System.Net.Sockets;

namespace TaskC;

class Program
{
    const string Host = "127.0.0.1";
    const int Port = 5000;

    static void Main(string[] args)
    {
        try
        {
            if (args.Length == 1 && args[0] == "server")
                RunServer();
            else if (args.Length == 1 && args[0] == "client")
                RunClient();
            else
                Console.WriteLine("Use: dotnet run -- server OR dotnet run -- client");
        }
        catch (SocketException ex)
        {
            Console.WriteLine($"Network error: {ex.Message}");
        }
        catch (IOException ex)
        {
            Console.WriteLine($"Connection error: {ex.Message}");
        }
    }

    static void RunServer()
    {
        var listener = new TcpListener(IPAddress.Parse(Host), Port);
        listener.Start();
        Console.WriteLine($"Server listening on {Host}:{Port}. Press Ctrl+C to stop.");
        while (true)
        {
            Console.WriteLine("Waiting...");
            TcpClient client = listener.AcceptTcpClient();
            Console.WriteLine("Client connected!");

            // Give each client a separate thread and keep accepting connections.
            Thread thread = new Thread(() => HandleClient(client));
            thread.IsBackground = true;
            thread.Start();
        }
    }

    static void HandleClient(TcpClient client)
    {
        int threadId = Environment.CurrentManagedThreadId;
        Console.WriteLine($"Client connected on thread {threadId}.");
        try
        {
            using (client)
            using (var reader = new StreamReader(client.GetStream()))
            using (var writer = new StreamWriter(client.GetStream()) { AutoFlush = true })
            {
                string? request;
                while ((request = reader.ReadLine()) != null)
                {
                    request = request.Trim();
                    string response = request.ToUpperInvariant() switch
                    {
                        "PING" => "PONG",
                        "HELLO" => $"Hello! You are served by thread {threadId}.",
                        "TIME" => $"Server time: {DateTime.Now:yyyy-MM-dd HH:mm:ss}",
                        "QUIT" => "Goodbye!",
                        _ => $"ECHO: {request}"
                    };
                    writer.WriteLine(response);
                    if (request.Equals("QUIT", StringComparison.OrdinalIgnoreCase))
                        break;
                }
            }
        }
        catch (IOException ex)
        {
            Console.WriteLine($"Thread {threadId}: {ex.Message}");
        }
        finally
        {
            Console.WriteLine($"Client disconnected from thread {threadId}.");
        }
    }

    static void RunClient()
    {
        using var client = new TcpClient(Host, Port);
        using var reader = new StreamReader(client.GetStream());
        using var writer = new StreamWriter(client.GetStream()) { AutoFlush = true };
        Console.WriteLine("Connected. Type PING, HELLO, TIME, QUIT, or any message.");
        while (true)
        {
            Console.Write("> ");
            string? input = Console.ReadLine();
            if (input == null)
                break;
            input = input.Trim();
            writer.WriteLine(input);
            string? response = reader.ReadLine();
            if (response == null)
                break;
            Console.WriteLine(response);
            if (input.Equals("QUIT", StringComparison.OrdinalIgnoreCase))
                break;
        }
    }
}
