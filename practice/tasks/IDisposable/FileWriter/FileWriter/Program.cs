using System;

namespace Convestudo.Unmanaged
{
    class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Enter any text to log or [q] to exit:");

            var input = Console.ReadLine();
            while (!string.Equals(input, "q", StringComparison.InvariantCultureIgnoreCase))
            {
                var now = DateTime.Now;
                var timestamp = $"{now.Hour}.{now.Minute}.{now.Second}.{now.Millisecond}";
                using (var fileWriter = new FileWriter($"log.txt", CreationDisposition.CreateAlways))
                {
                    fileWriter.WriteLine(input);
                }
                Console.WriteLine($"echo {timestamp}: {input}");

                input = Console.ReadLine();
            }
        }
    }
}
