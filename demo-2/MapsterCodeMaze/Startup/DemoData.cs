using MapsterCodeMaze.Models;
using System;
using System.Collections.Generic;

namespace MapsterCodeMaze.Startup
{
    public class DemoData
    {
        public static Person CreatePerson()
        {
            return new Person()
            {
                Title = "Mr.",
                FirstName = "Peter",
                LastName = "Pan",
                DateOfBirth = new DateTime(2000, 1, 1),
                Address = new Address()
                {
                    Country = "Neverland",
                    PostCode = "123N",
                    Street = "Funny Street 2",
                    City = "Neverwood"
                },
                Password = "123456"
            };
        }

        public static List<Person> CreatePeople()
        {
            return new List<Person>()
            {
                new Person()
                {
                    Title = "Mr.",
                    FirstName = "Peter",
                    LastName = "Pan",
                    DateOfBirth = new DateTime(2000, 1, 1),
                    Address = new Address()
                    {
                        Country = "Neverland",
                        PostCode = "123N",
                        Street = "Funny Street 2",
                        City = "Neverwood"
                    },
                    Password = "123456"
                },
                new Person()
                {
                    Title = "Ms.",
                    FirstName = "Wendy",
                    LastName = "Darling",
                    DateOfBirth = new DateTime(1999, 5, 15),
                    Address = new Address()
                    {
                        Country = "England",
                        PostCode = "W1A 1AA",
                        Street = "Kensington Gardens 1",
                        City = "London"
                    },
                    Password = "123456789"
                },
                new Person()
                {
                    Title = "Mr.",
                    FirstName = "James",
                    LastName = "Hook",
                    DateOfBirth = new DateTime(1990, 3, 22),
                    Address = new Address()
                    {
                        Country = "Neverland",
                        PostCode = "456N",
                        Street = "Pirate Cove 7",
                        City = "Skull Island"
                    },
                    Password = "1234567890"
                }
            };
        }
    }
}
