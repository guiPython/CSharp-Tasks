using System.Linq;

namespace Program;

class Program
{
    public static List<string[]> ReadCsv()
    {
        var content = new List<string[]>();
        content.Add(new string[] { "Jorge", "Machado" });
        content.Add(new string[] { "Edson", "Jair" });
        content.Add(new string[] { "Michael", "Carvalho" });

        var random = new Random();
        var n = random.Next(0, 2);
        if (n == 1)
        {
            throw new ArgumentException("Ocorreu um erro.");
        }
        else
        {
            return content;
        }
    }

    public static void GetNameUser(string[] register)
    {
        Console.WriteLine(register[0]);
    }

    public static Task GetInformation(CancellationToken token)
    {
        var readTask = Task.Run(() => ReadCsv(), token);

        var process = readTask.ContinueWith((t) =>
        {
            Console.WriteLine("Houve uma falha na leitura do arquivo.");
            var e = t.Exception?.InnerException;
            if (e != null) throw e;
        }, token, TaskContinuationOptions.OnlyOnFaulted, TaskScheduler.Current);

        process = readTask.ContinueWith((t) =>
        {
            var result = t.Result;
            for (int i = 0; i < result.Count(); i++)
            {
                Thread.Sleep(1000);
                if (token.IsCancellationRequested) break;
                GetNameUser(result.ElementAt(i));
            }
        }, token, TaskContinuationOptions.NotOnFaulted, TaskScheduler.Current);

        process = process.ContinueWith(_ =>
        {
            if (token.IsCancellationRequested)
            {
                Console.WriteLine("Tarefa Cancelada");
            }
            else Console.WriteLine("Tarefa finalizada.");
        });

        return process;
    }

    static void Main(string[] args)
    {
        CancellationTokenSource source = new CancellationTokenSource();
        var token = source.Token;
        var task = GetInformation(token);
        Console.WriteLine("Pressione ESC para sair do programa");

        Console.WriteLine("Lendo arquivo...");
        Console.WriteLine("Deseja cancelar a operação? (Pressione tecla Y)");
        if (Console.ReadKey(true).Key == ConsoleKey.Y)
        {
            source.Cancel();
        }
        Console.WriteLine();

        Task.WaitAny(task);
    }
}

