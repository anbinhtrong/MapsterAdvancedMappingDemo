using Mapster;
using MapsterCodeMaze.Models;
using MapsterCodeMaze.Startup;
using System.Collections.Generic;

namespace MapsterCodeMaze.Mappings
{
    public static class MappingFunctions
    {
        private static readonly Person _person = DemoData.CreatePerson();
        private static readonly ICollection<Person> _people = DemoData.CreatePeople();

        public static PersonDto MapPersonToNewDto()
        {
            var personDto = _person.Adapt<PersonDto>();

            return personDto;
        }
    }
}
