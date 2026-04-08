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

        // Static constructor to configure Mapster on startup
        static MappingFunctions()
        {
            ConfigureMapster();
        }

        private static void ConfigureMapster()
        {
            // ===== CÁCH 1: Cấu hình GLOBAL (Global Settings) =====
            // IgnoreNonMapped(true): Bỏ qua tất cả properties không được mapping
            // Sử dụng khi bạn muốn áp dụng cho tất cả mapping trong ứng dụng
            TypeAdapterConfig.GlobalSettings.Default.IgnoreNonMapped(true);
            TypeAdapterConfig<Person, PersonDto>.NewConfig()
                .Map(dest => dest.FirstName, src => src.FirstName)
                .Map(dest => dest.LastName, src => src.LastName);

            // ===== CÁCH 2: Cấu hình CỤ THỂ (Specific Mapping) =====
            // Chỉ bỏ qua các properties cụ thể
            // TypeAdapterConfig<Person, PersonDto>.NewConfig()
            //     .Ignore(dest => dest.Password)
            //     .Ignore(dest => dest.DateOfBirth);
        }

        public static PersonDto MapPersonToNewDto()
        {
            var personDto = _person.Adapt<PersonDto>();

            return personDto;
        }
    }
}
