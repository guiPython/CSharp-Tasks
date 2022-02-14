namespace Program;

class Program
{
    public static List<string[]> ReadCsv(string path)
    {
        var content = new List<string[]>();
        content.Add(new string[] { "Jorge", "Machado" });
        content.Add(new string[] { "Edson", "Jair" });
        content.Add(new string[] { "Michael", "Carvalho" });
        return content;
    }

    public static void GetNameUser(string[] register) => Console.WriteLine(register[0]);

    public static void GetLastNameUser(string[] register) => Console.WriteLine(register[1]);

    public static void GetFullNameUser(string[] register) => Console.WriteLine(register[0] + ' ' + register[1]);

    public static async Task GetInformation(int option, string path)
    {
        var readTask = Task.Factory.StartNew(() => ReadCsv(path));
        var result = await readTask;

        Thread.Sleep(3000);
        if (option == 1) result.ForEach(register => GetNameUser(register));
        else if (option == 2) result.ForEach(register => GetLastNameUser(register));
        else if (option == 3) result.ForEach(register => GetFullNameUser(register));
    }

    static async Task Main(string[] args)
    {
        int option;

        Console.WriteLine("1- Listar Nomes dos Usuarios\n2- Listar Sobrenomes dos Usuarios\n3- Listar Tudo");
        if (!int.TryParse(Console.ReadLine(), out option))
        {
            while (!int.TryParse(Console.ReadLine(), out option)) continue;
        }

        await GetInformation(option, "");

        System.Console.WriteLine("Arquivo Lido");
    }
}