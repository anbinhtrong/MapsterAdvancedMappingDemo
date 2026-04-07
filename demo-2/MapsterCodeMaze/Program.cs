using MapsterCodeMaze.Mappings;
using MapsterCodeMaze.Models;
using System;

namespace MapsterCodeMaze
{
    internal class Program
    {
        static void Main(string[] args)
        {
            DebugMapster.DebugMapping<Person, PersonDto>();

            var person = MappingFunctions.MapPersonToNewDto();
            PrintPerson(person);
        }

        static void PrintPerson(PersonDto person)
        {
            Console.WriteLine("=== Person Information ===");
            Console.WriteLine($"Title: {person.Title}");
            Console.WriteLine($"Name: {person.FirstName} {person.LastName}");
            Console.WriteLine($"Date of Birth: {person.DateOfBirth:yyyy-MM-dd}");
            Console.WriteLine("===========================");
        }
    }
}
