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

    public static Task<Employee> FindEmployee(string? name, CancellationToken token)
    {
        return Task.Run(() =>
        {
            var randomTime = new Random().Next(3000, 5000);
            Thread.Sleep(randomTime);
            if (token.IsCancellationRequested)
            {
                throw new OperationCanceledException();
            }
            return employees.Find(e => e.name == name);
        });
    }

    public static async Task ExecuteQueries(List<string?> queries, CancellationTokenSource source)
    {
        if (queries.Count() > 0)
        {
            var tasks = new List<Task<Employee>>();
            queries.ForEach(name =>
            {
                tasks.Add(FindEmployee(name, source.Token));
            });

            var maxDelay = Task.Delay(3000);
            var employeesResult = Task.WhenAll(tasks);

            var completedTask = await Task.WhenAny(maxDelay, employeesResult);

            if (completedTask == employeesResult)
            {
                Console.WriteLine("Resultados encontrados de maneira assincrona UHULL:");
                employeesResult.Result.ToList().ForEach(e => Console.WriteLine(e));
            }
            else
            {
                source.Cancel();
                Console.WriteLine("Queries Canceladas");
            }
        }
    }

    public static void Main(string[] args)
    {
        employees.Add(new Employee { name = "Marcos", age = 43, sector = Sector.Support });
        employees.Add(new Employee { name = "André", age = 53, sector = Sector.Cleaning });
        employees.Add(new Employee { name = "Alan", age = 63, sector = Sector.Support });
        employees.Add(new Employee { name = "José", age = 13, sector = Sector.Cleaning });

        var queries = ReadQueries();

        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        var task = ExecuteQueries(queries, cancellationTokenSource);
        Task.WaitAll(task);
        Console.WriteLine("FIM");
    }
}
