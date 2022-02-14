using System;


namespace Program;


// Simular leitura de um csv grande com funcionarios
class Program
{
    public static List<Employee> employees = new List<Employee>();
    public enum Sector
    {
        Cleaning, Support
    }

    public record struct Employee
    {
        public String name { get; init; }
        public int age { get; init; }
        public Sector sector { get; init; }
    }

    public static List<string?> ReadQueries()
    {
        var queries = new List<string?>();
        while (true)
        {
            if (queries.Count() > 0)
            {
                Console.Write("Lista de nomes a consultar:");
                queries.ForEach(n => Console.Write($" {n}"));
                Console.WriteLine();
            }
            Console.Write("Digite o nome do funcionario que voce deseja encontrar || 1 para sair: ");
            var result = Console.ReadLine();
            if (result == "1") break;
            queries.Add(result);
            Console.Clear();
        }

        return queries;
    }

    public static Task<Employee> FindEmployee(string? name)
    {
        return Task.Run(() =>
        {
            var randomTime = new Random().Next(500, 2000);
            Thread.Sleep(randomTime);
            return employees.Find(e => e.name == name);
        });
    }

    public static async Task ExecuteQueries(List<string?> queries)
    {
        if (queries.Count() > 0)
        {
            var tasks = new List<Task<Employee>>();
            queries.ForEach(name =>
            {
                tasks.Add(FindEmployee(name));
            });

            var employeesResult = await Task.WhenAll(tasks);

            Console.WriteLine("Resultados encontrados de maneira assincrona UHULL:");
            employeesResult.ToList().ForEach(e => Console.WriteLine(e));
        }
    }

    public static async Task Main(string[] args)
    {
        employees.Add(new Employee { name = "Marcos", age = 43, sector = Sector.Support });
        employees.Add(new Employee { name = "André", age = 53, sector = Sector.Cleaning });
        employees.Add(new Employee { name = "Alan", age = 63, sector = Sector.Support });
        employees.Add(new Employee { name = "José", age = 13, sector = Sector.Cleaning });

        var queries = ReadQueries();

        await ExecuteQueries(queries);
        Console.WriteLine("FIM");
    }
}
