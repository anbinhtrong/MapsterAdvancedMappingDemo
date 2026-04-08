using Mapster;
using MapsterCodeMaze.Models;
using System;

namespace MapsterCodeMaze.Mappings
{
    /// <summary>
    /// Demo: So sánh các cách cấu hình Mapster
    /// </summary>
    internal class MapsterConfigDemo
    {
        public static void DemoIgnoreNonMapped()
        {
            Console.WriteLine("\n===== DEMO: IgnoreNonMapped vs Ignore =====\n");

            // Dữ liệu test
            var person = new Person
            {
                Title = "Mr.",
                FirstName = "John",
                LastName = "Doe",
                DateOfBirth = new DateTime(1990, 1, 1),
                Password = "SecurePassword123",
                Address = new Address { Street = "123 Main St", City = "Boston" }
            };

            // ===== CÁCH 1: GLOBAL SETTINGS - IgnoreNonMapped(true) =====
            Console.WriteLine("CÁCH 1: TypeAdapterConfig.GlobalSettings.Default.IgnoreNonMapped(true)");
            Console.WriteLine("Hiệu ứng: Bỏ qua tất cả properties KHÔNG CÓ trong PersonDto\n");

            // Giả sử đã cấu hình:
            // TypeAdapterConfig.GlobalSettings.Default.IgnoreNonMapped(true);
            
            Console.WriteLine("✓ Password (không có trong PersonDto) → Bỏ qua");
            Console.WriteLine("✓ Address (không có trong PersonDto) → Bỏ qua");
            Console.WriteLine("✓ Title (có trong PersonDto) → Được map");
            Console.WriteLine("✓ FirstName (có trong PersonDto) → Được map");
            Console.WriteLine("✓ LastName (có trong PersonDto) → Được map");
            Console.WriteLine("✓ DateOfBirth (có trong PersonDto) → Được map\n");

            // ===== CÁCH 2: SPECIFIC MAPPING - Ignore() cụ thể =====
            Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━\n");
            Console.WriteLine("CÁCH 2: TypeAdapterConfig<Person, PersonDto>.NewConfig().Ignore()");
            Console.WriteLine("Hiệu ứng: Chỉ bỏ qua các properties được chỉ định\n");

            Console.WriteLine("Ví dụ cấu hình:");
            Console.WriteLine("  TypeAdapterConfig<Person, PersonDto>.NewConfig()");
            Console.WriteLine("    .Ignore(dest => dest.Password)");
            Console.WriteLine("    .Ignore(dest => dest.DateOfBirth);\n");

            Console.WriteLine("Kết quả mapping:");
            Console.WriteLine("✓ Password → Bỏ qua (được chỉ định)");
            Console.WriteLine("✓ DateOfBirth → Bỏ qua (được chỉ định)");
            Console.WriteLine("✓ Title → Được map");
            Console.WriteLine("✓ FirstName → Được map");
            Console.WriteLine("✓ LastName → Được map");
            Console.WriteLine("✗ Address → Cảnh báo (không có trong PersonDto, không được ignore)\n");

            // ===== SO SÁNH =====
            Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━\n");
            PrintComparison();
        }

        private static void PrintComparison()
        {
            Console.WriteLine("📊 BẢNG SO SÁNH:\n");
            Console.WriteLine("┌─────────────────────────┬──────────────────┬──────────────────┐");
            Console.WriteLine("│ Tiêu chí                │ IgnoreNonMapped  │ Ignore()         │");
            Console.WriteLine("├─────────────────────────┼──────────────────┼──────────────────┤");
            Console.WriteLine("│ Phạm vi                 │ GLOBAL           │ CỤ THỂ           │");
            Console.WriteLine("│ Áp dụng cho             │ Tất cả mapping   │ 1 mapping pair   │");
            Console.WriteLine("│ Bỏ qua properties       │ Không có trong   │ Được chỉ định    │");
            Console.WriteLine("│                         │ Destination      │                  │");
            Console.WriteLine("│ Cảnh báo unmapped       │ Không            │ Có (nếu enabled) │");
            Console.WriteLine("│ Trường hợp dùng         │ Simple DTO       │ Bảo mật, phức    │");
            Console.WriteLine("│                         │ mapping          │ tạp              │");
            Console.WriteLine("└─────────────────────────┴──────────────────┴──────────────────┘\n");

            Console.WriteLine("💡 KHI NÀO DÙNG GÌ:\n");
            Console.WriteLine("1️⃣  Dùng IgnoreNonMapped(true):");
            Console.WriteLine("   • Mapping từ Entity → DTO (standard scenario)");
            Console.WriteLine("   • Chỉ map những properties cần thiết");
            Console.WriteLine("   • Bỏ qua Navigation properties, internal properties\n");

            Console.WriteLine("2️⃣  Dùng .Ignore():");
            Console.WriteLine("   • Cần bỏ qua nhạy cảm (Password, Token, Secret)");
            Console.WriteLine("   • Mapping phức tạp cần điều khiển chi tiết");
            Console.WriteLine("   • Muốn mapping có cảnh báo về unmapped properties\n");
        }
    }
}
