using System.Runtime.CompilerServices;

namespace Program
{
    public enum Sector
    {
        Cleaning,
        Support
    }

    public record Employee(string name, byte age, Sector sector);

    public class Program
    {
        public static async IAsyncEnumerable<Employee> FindAllUsers([EnumeratorCancellation] CancellationToken token)
        {
            List<Employee> employees = new List<Employee>();
            employees.Add(new Employee("Marcos", 56, Sector.Support));
            employees.Add(new Employee("José", 62, Sector.Support));
            employees.Add(new Employee("André", 37, Sector.Cleaning));

            foreach (Employee e in employees)
            {
                int delay = new Random().Next(333, 1000);
                await Task.Delay(delay);
                if (token.IsCancellationRequested)
                {
                    Console.WriteLine("Cancelled Operation");
                    break;
                }
                yield return e;
            }
        }

        public static async Task Main(string[] args)
        {
            CancellationTokenSource source = new CancellationTokenSource();

            source.CancelAfter(1000);
            await foreach (var employee in FindAllUsers(source.Token))
            {
                Console.WriteLine(employee);
            }
        }
    }

}