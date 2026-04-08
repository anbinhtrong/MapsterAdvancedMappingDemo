using Mapster;
using MapsterCodeMaze.Mappings;
using MapsterCodeMaze.Models;
using System;
using System.Text;

namespace MapsterCodeMaze
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            // Demo: Hiển thị so sánh IgnoreNonMapped vs Ignore
            MapsterConfigDemo.DemoIgnoreNonMapped();

            Console.WriteLine("\n═══════════════════════════════════════════════════\n");

            var person = MappingFunctions.MapPersonToNewDto();
            DebugMapster.DebugMapping<Person, PersonDto>();
            PrintPerson(person);
        }

        static void PrintPerson(PersonDto person)
        {
            Console.WriteLine("=== Person Information ===");
            Console.WriteLine($"Title: {person.Title}");
            Console.WriteLine($"Name: {person.FirstName} {person.LastName}");
            Console.WriteLine($"Date of Birth: {person.DateOfBirth:yyyy-MM-dd}");
            Console.WriteLine($"Password: {person.Password}");
            Console.WriteLine("===========================");
        }
    }
}
