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
        n = 0;
        if (n == 1)
        {
            throw new ArgumentException("Ocorreu um erro.");
        }
        else return content;
    }

    public static void GetNameUser(string[] register)
    {
        Console.WriteLine(register[0]);
    }

    public static Task GetInformation()
    {
        var readTask = Task.Run(() => ReadCsv());

        var process = readTask.ContinueWith((t) =>
        {
            Console.WriteLine("Houve uma falha na leitura do arquivo.");
            var e = t.Exception?.InnerException;
            if (e != null) throw e;
        }, TaskContinuationOptions.OnlyOnFaulted);

        process = readTask.ContinueWith((t) =>
        {
            var result = t.Result;
            Thread.Sleep(3000);
            result.ForEach(register => GetNameUser(register));
        }, TaskContinuationOptions.NotOnFaulted);

        process = process.ContinueWith(_ => Console.WriteLine("Tarefa finalizada."));

        return process;
    }

    static void Main(string[] args)
    {
        var task = GetInformation();

        Console.WriteLine("Lendo arquivo...");
        Task.WaitAny(task);
    }
}