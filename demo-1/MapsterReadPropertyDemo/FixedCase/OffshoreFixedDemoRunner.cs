using System;
using System.Collections.Generic;
using Mapster;
using MapsterDemo.Models.Offshore;

namespace MapsterDemo.FixedCase
{
    /// <summary>
    /// ✅ DEMO FIX: RoleOffshoreLocation với computed property từ Enum
    /// So sánh 3 strategy xử lý [NotMapped] equivalent trong Mapster
    /// </summary>
    public static class OffshoreFixedDemoRunner
    {
        public static void Run()
        {
            Console.WriteLine("╔══════════════════════════════════════════════════════════╗");
            Console.WriteLine("║   ✅  OFFSHORE FIXED - 3 Strategies cho Computed Enum    ║");
            Console.WriteLine("╚══════════════════════════════════════════════════════════╝");
            Console.WriteLine();

            var locations = new List<RoleOffshoreLocation>
            {
                new RoleOffshoreLocation { Id = 1, RoleId = 1, OffshoreLocationId = 1, IsActive = true,  Note = "HN office" },
                new RoleOffshoreLocation { Id = 2, RoleId = 1, OffshoreLocationId = 2, IsActive = true,  Note = "HCM office" },
                new RoleOffshoreLocation { Id = 3, RoleId = 2, OffshoreLocationId = 3, IsActive = false, Note = "DN office" },
                new RoleOffshoreLocation { Id = 4, RoleId = 3, OffshoreLocationId = 4, IsActive = true,  Note = "CT office" },
            };

            Console.WriteLine("📋 Dữ liệu đầu vào (Name là computed getter-only):");
            Console.WriteLine($"   {"Id",-4} {"RoleId",-8} {"OffshoreLocationId",-20} {"Name (computed)",-20} {"IsActive",-10}");
            Console.WriteLine($"   {new string('─', 65)}");
            foreach (var loc in locations)
            {
                Console.WriteLine($"   {loc.Id,-4} {loc.RoleId,-8} {loc.OffshoreLocationId,-20} {loc.Name,-20} {loc.IsActive,-10}");
            }
            Console.WriteLine();

            // ─── STRATEGY A: Ignore → Name = null ───
            RunStrategyA(locations[0]);

            // ─── STRATEGY B: Ignore + Map thủ công (RECOMMENDED) ───
            RunStrategyB(locations);

            // ─── STRATEGY C: Đổi tên field trong DTO ───
            RunStrategyC(locations);

            // ─── Self-mapping clone ───
            RunSelfMapping(locations[1]);

            // ─── So sánh [NotMapped] AutoMapper vs Mapster ───
            PrintAutoMapperVsMapsterComparison();

            Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
        }

        private static void RunStrategyA(RoleOffshoreLocation loc)
        {
            Console.WriteLine("── Strategy A: .Ignore(dest => dest.Name) ──");
            Console.WriteLine("   Kết quả: ViewModel.Name = null (bị bỏ qua hoàn toàn)");
            Console.WriteLine("   Dùng khi: ViewModel không cần Name, hoặc tự set sau.");
            Console.WriteLine();

            try
            {
                // Tạo config tạm để demo Strategy A
                var config = new TypeAdapterConfig();
                config.NewConfig<RoleOffshoreLocation, RoleOffshoreLocationViewModel>()
                    .Ignore(dest => dest.Name)
                    .Ignore(dest => dest.LocationDisplayName)
                    .Ignore(dest => dest.LocationEnumName);

                var vm = loc.Adapt<RoleOffshoreLocationViewModel>(config);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"   ✅ Map thành công:");
                Console.WriteLine($"      ViewModel.Id                : {vm.Id}");
                Console.WriteLine($"      ViewModel.OffshoreLocationId: {vm.OffshoreLocationId}");
                Console.WriteLine($"      ViewModel.Name              : {(vm.Name == null ? "(null - bị Ignore)" : vm.Name)}");
                Console.WriteLine($"      ViewModel.IsActive          : {vm.IsActive}");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"   ❌ Lỗi: {ex.Message}");
                Console.ResetColor();
            }

            Console.WriteLine();
        }

        private static void RunStrategyB(List<RoleOffshoreLocation> locations)
        {
            Console.WriteLine("── Strategy B ✅ RECOMMENDED: Ignore + Map thủ công từ OffshoreLocationId ──");
            Console.WriteLine("   Tương đương [NotMapped] + AfterMap() của AutoMapper");
            Console.WriteLine("   Kết quả: ViewModel.Name = GetDescription() từ Enum");
            Console.WriteLine();

            try
            {
                // Strategy B config - đây là config được đăng ký trong GlobalSettings
                var config = new TypeAdapterConfig();
                config.NewConfig<RoleOffshoreLocation, RoleOffshoreLocationViewModel>()
                    .Ignore(dest => dest.Name)   // ← Ignore để Mapster không tự map src.Name → dest.Name
                    .Map(dest => dest.Name,       // ← Map thủ công: tính từ OffshoreLocationId
                         src => ((OffshoreLocationEnum)src.OffshoreLocationId).GetDescription())
                    .Map(dest => dest.LocationDisplayName,
                         src => ((OffshoreLocationEnum)src.OffshoreLocationId).GetDescription())
                    .Map(dest => dest.LocationEnumName,
                         src => ((OffshoreLocationEnum)src.OffshoreLocationId).ToString());

                var vmList = locations.Adapt<List<RoleOffshoreLocationViewModel>>(config);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"   ✅ Map thành công ({vmList.Count} items):");
                Console.WriteLine();
                Console.WriteLine($"   {"Id",-4} {"OffshoreId",-12} {"Name (mapped)",-20} {"LocationDisplayName",-20} {"LocationEnumName",-15}");
                Console.WriteLine($"   {new string('─', 75)}");
                foreach (var vm in vmList)
                {
                    Console.WriteLine($"   {vm.Id,-4} {vm.OffshoreLocationId,-12} {vm.Name,-20} {vm.LocationDisplayName,-20} {vm.LocationEnumName,-15}");
                }
                Console.ResetColor();

                Console.WriteLine();
                Console.WriteLine("   → Name được tính đúng từ Enum.GetDescription() ✅");
                Console.WriteLine("   → LocationDisplayName = LocationEnumName đều có giá trị ✅");
                Console.WriteLine("   → Không cần foreach ✅");
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"   ❌ Lỗi: {ex.Message}");
                Console.ResetColor();
            }

            Console.WriteLine();
        }

        private static void RunStrategyC(List<RoleOffshoreLocation> locations)
        {
            Console.WriteLine("── Strategy C: Đổi tên field trong DTO (tránh conflict hoàn toàn) ──");
            Console.WriteLine("   Dùng RoleOffshoreLocationDto.LocationName thay vì .Name");
            Console.WriteLine("   → Không cần Ignore vì DTO không có property 'Name'");
            Console.WriteLine();

            try
            {
                var config = new TypeAdapterConfig();
                config.NewConfig<RoleOffshoreLocation, RoleOffshoreLocationDto>()
                    .Map(dest => dest.LocationName,
                         src => ((OffshoreLocationEnum)src.OffshoreLocationId).GetDescription());

                var dtoList = locations.Adapt<List<RoleOffshoreLocationDto>>(config);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"   ✅ Map thành công ({dtoList.Count} items):");
                Console.WriteLine();
                Console.WriteLine($"   {"Id",-4} {"RoleId",-8} {"OffshoreId",-12} {"LocationName",-20} {"IsActive",-10} {"Note",-15}");
                Console.WriteLine($"   {new string('─', 73)}");
                foreach (var dto in dtoList)
                {
                    Console.WriteLine($"   {dto.Id,-4} {dto.RoleId,-8} {dto.OffshoreLocationId,-12} {dto.LocationName,-20} {dto.IsActive,-10} {dto.Note,-15}");
                }
                Console.ResetColor();

                Console.WriteLine();
                Console.WriteLine("   → LocationName map đúng từ Enum ✅");
                Console.WriteLine("   → Không cần Ignore (không có property Name conflict) ✅");
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"   ❌ Lỗi: {ex.Message}");
                Console.ResetColor();
            }

            Console.WriteLine();
        }

        private static void RunSelfMapping(RoleOffshoreLocation loc)
        {
            Console.WriteLine("── Self-mapping: RoleOffshoreLocation → RoleOffshoreLocation (clone) ──");
            Console.WriteLine("   Dùng khi cần clone object trong cùng type");
            Console.WriteLine();

            try
            {
                var config = new TypeAdapterConfig();
                config.NewConfig<RoleOffshoreLocation, RoleOffshoreLocation>()
                    .Ignore(dest => dest.Name); // getter-only → phải Ignore

                var clone = loc.Adapt<RoleOffshoreLocation>(config);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"   ✅ Clone thành công:");
                Console.WriteLine($"      Original  → Id: {loc.Id}, OffshoreLocationId: {loc.OffshoreLocationId}, Name: \"{loc.Name}\"");
                Console.WriteLine($"      Clone     → Id: {clone.Id}, OffshoreLocationId: {clone.OffshoreLocationId}, Name: \"{clone.Name}\"");
                Console.WriteLine($"   → Name được recompute tự động từ OffshoreLocationId (không cần copy) ✅");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"   ❌ Lỗi: {ex.Message}");
                Console.ResetColor();
            }

            Console.WriteLine();
        }

        private static void PrintAutoMapperVsMapsterComparison()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("╔══════════════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║           SO SÁNH: AutoMapper [NotMapped] vs Mapster                ║");
            Console.WriteLine("╠══════════════════════════════════════════════════════════════════════╣");
            Console.ResetColor();

            Console.WriteLine("║                                                                      ║");
            Console.WriteLine("║  AutoMapper:                                                         ║");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("║  [NotMapped]                                                         ║");
            Console.WriteLine("║  public string Name => ((OffshoreLocationEnum)OffshoreLocationId)    ║");
            Console.WriteLine("║                          .GetDescription();                          ║");
            Console.WriteLine("║  // Trong Profile:                                                   ║");
            Console.WriteLine("║  CreateMap<RoleOffshoreLocation, RoleOffshoreLocationViewModel>();   ║");
            Console.WriteLine("║  // → [NotMapped] đủ rồi, không cần ForMember thêm                  ║");
            Console.ResetColor();
            Console.WriteLine("║                                                                      ║");
            Console.WriteLine("║  Mapster (KHÔNG có [NotMapped] tương đương):                        ║");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("║  // Entity KHÔNG thêm attribute gì:                                 ║");
            Console.WriteLine("║  public string Name => ((OffshoreLocationEnum)OffshoreLocationId)    ║");
            Console.WriteLine("║                          .GetDescription();                          ║");
            Console.WriteLine("║  // Trong TypeAdapterConfig:                                         ║");
            Console.WriteLine("║  TypeAdapterConfig<RoleOffshoreLocation,                             ║");
            Console.WriteLine("║                    RoleOffshoreLocationViewModel>.NewConfig()        ║");
            Console.WriteLine("║      .Ignore(dest => dest.Name)          // ← bắt buộc              ║");
            Console.WriteLine("║      .Map(dest => dest.Name,             // ← map thủ công nếu cần  ║");
            Console.WriteLine("║           src => ((OffshoreLocationEnum)src.OffshoreLocationId)      ║");
            Console.WriteLine("║                   .GetDescription());                                ║");
            Console.ResetColor();
            Console.WriteLine("║                                                                      ║");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("╠══════════════════════════════════════════════════════════════════════╣");
            Console.WriteLine("║  Kết luận: Mapster không đọc [NotMapped] → phải cấu hình tường      ║");
            Console.WriteLine("║  minh trong TypeAdapterConfig. Nhưng bù lại, .Map() cho phép        ║");
            Console.WriteLine("║  tính lại giá trị từ bất kỳ expression nào → linh hoạt hơn.         ║");
            Console.WriteLine("╚══════════════════════════════════════════════════════════════════════╝");
            Console.ResetColor();
            Console.WriteLine();
        }
    }
}
