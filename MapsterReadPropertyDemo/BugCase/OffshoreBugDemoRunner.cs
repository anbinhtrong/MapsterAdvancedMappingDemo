using System;
using Mapster;
using MapsterDemo.Models.Offshore;

namespace MapsterDemo.BugCase
{
    /// <summary>
    /// ❌ DEMO LỖI: RoleOffshoreLocation với computed property từ Enum
    ///
    /// Kịch bản thực tế:
    ///   public string Name => ((OffshoreLocationEnum)OffshoreLocationId).GetDescription();
    ///
    /// AutoMapper: dùng [NotMapped] → tự động bỏ qua
    /// Mapster (default): KHÔNG đọc [NotMapped] → vẫn cố map → CompileException
    /// </summary>
    public static class OffshoreBugDemoRunner
    {
        public static void Run()
        {
            Console.WriteLine("╔══════════════════════════════════════════════════════════╗");
            Console.WriteLine("║   ❌  OFFSHORE BUG CASE - Computed Property từ Enum     ║");
            Console.WriteLine("╚══════════════════════════════════════════════════════════╝");
            Console.WriteLine();
            Console.WriteLine("  Scenario: public string Name => ((OffshoreLocationEnum)OffshoreLocationId)");
            Console.WriteLine("                                    .GetDescription();");
            Console.WriteLine();

            // Reset về default config để demo lỗi
            // (Không cần clear trong version này)


            var location = new RoleOffshoreLocation
            {
                Id = 10,
                RoleId = 3,
                OffshoreLocationId = 2, // HoChiMinh
                IsActive = true,
                Note = "Văn phòng chính"
            };

            Console.WriteLine("📋 Dữ liệu đầu vào:");
            Console.WriteLine($"   Id                 : {location.Id}");
            Console.WriteLine($"   RoleId             : {location.RoleId}");
            Console.WriteLine($"   OffshoreLocationId : {location.OffshoreLocationId}");
            Console.WriteLine($"   Name (computed)    : \"{location.Name}\"   ← getter-only, không có setter");
            Console.WriteLine($"   IsActive           : {location.IsActive}");
            Console.WriteLine();

            // ─── TEST 1: [NotMapped] với AutoMapper (giả lập) ───
            Console.WriteLine("── So sánh 1: [NotMapped] trong AutoMapper ──");
            Console.WriteLine("   Code AutoMapper cũ:");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("   // Entity:");
            Console.WriteLine("   [NotMapped]");
            Console.WriteLine("   public string Name => ((OffshoreLocationEnum)OffshoreLocationId).GetDescription();");
            Console.WriteLine();
            Console.WriteLine("   // AutoMapper Profile:");
            Console.WriteLine("   // Không cần cấu hình gì thêm - [NotMapped] đủ để AutoMapper bỏ qua");
            Console.WriteLine("   CreateMap<RoleOffshoreLocation, RoleOffshoreLocationViewModel>();");
            Console.ResetColor();
            Console.WriteLine();

            var locationWithAttr = new RoleOffshoreLocationAutoMapper
            {
                Id = 10, RoleId = 3, OffshoreLocationId = 2, IsActive = true, Note = "Văn phòng chính"
            };
            Console.WriteLine($"   [NotMapped] Name property value: \"{locationWithAttr.Name}\"");
            Console.WriteLine("   → AutoMapper thấy [NotMapped] → bỏ qua → KHÔNG lỗi ✅");
            Console.WriteLine();

            // ─── TEST 2: Mapster DEFAULT - KHÔNG đọc [NotMapped] ───
            Console.WriteLine("── So sánh 2: Mapster DEFAULT config (KHÔNG đọc [NotMapped]) ──");
            Console.WriteLine("   Mapster không quan tâm [NotMapped] attribute.");
            Console.WriteLine("   Nó vẫn cố map tất cả property có cùng tên → LỖI.");
            Console.WriteLine();

            // Bao bọc trong class cha
            var wrapper = new RoleOffshoreLocationAutoMapperWrapper
            {
                Id = 1,
                WrapperName = "Test Wrapper",
                Location = locationWithAttr
            };

            // Test map sang RoleOffshoreLocationWrapper với object chứa nested mapping
            Console.WriteLine("   Test: wrapper.Adapt<RoleOffshoreLocationWrapper>() (mapping có chứa nested object)");
            try
            {
                var cloned = wrapper.Adapt<RoleOffshoreLocationWrapper>();
                Console.WriteLine($"   Unexpected success: {cloned.Location.Name}");
            }
            catch (Exception ex)
            {
                PrintError("Map ParentRoleOffshoreLocation (Nested Mapping)", ex);
            }

            Console.WriteLine();

            // Test map sang ViewModel (Name có public set ở destination)
            Console.WriteLine("   Test: location.Adapt<RoleOffshoreLocationViewModel>()");
            Console.WriteLine("   (Destination.Name có public set, nhưng Source.Name là getter-only)");
            Console.WriteLine("   → Mapster tạo expression tree để gán Source.Name = ... → LỖI");
            try
            {
                var vm = location.Adapt<RoleOffshoreLocationViewModel>();
                Console.WriteLine($"   Unexpected success: {vm.Name}");
            }
            catch (Exception ex)
            {
                PrintError("Map → RoleOffshoreLocationViewModel", ex);
            }

            Console.WriteLine();
            Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
            Console.WriteLine("  💡 Root cause: Mapster không phân biệt source/destination");
            Console.WriteLine("     khi tìm property trùng tên - nó cố tạo assignment expression");
            Console.WriteLine("     cho cả hai chiều, kể cả property getter-only của SOURCE.");
            Console.WriteLine();
        }

        private static void PrintError(string label, Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"   ❌ LỖI [{label}]:");
            Console.WriteLine($"      {ex.GetType().Name}: {ex.Message}");
            if (ex.InnerException != null)
                Console.WriteLine($"      Inner: {ex.InnerException.Message}");
            Console.ResetColor();
        }
    }
}
