using System;
using Mapster;
using MapsterDemo.Models.Offshore;

namespace MapsterDemo.Config
{
    /// <summary>
    /// ✅ Fix cho RoleOffshoreLocation.Name (computed property từ Enum)
    ///
    /// 3 chiến lược xử lý, từ đơn giản đến phức tạp:
    ///
    ///   Strategy A: Ignore hoàn toàn → Name trong ViewModel sẽ là null
    ///   Strategy B: Ignore source + Map thủ công → Name = computed từ OffshoreLocationId
    ///   Strategy C: Đổi tên field trong ViewModel → tránh conflict tên hoàn toàn
    /// </summary>
    public static class OffshoreMapsterConfig
    {
        public static void RegisterMappings()
        {
            Console.WriteLine("   Đăng ký TypeAdapterConfig cho RoleOffshoreLocation...");
            Console.WriteLine();

            RegisterStrategyA();
            RegisterStrategyB();
            RegisterStrategyC();
            RegisterSelfMapping();
        }

        /// <summary>
        /// Strategy A: Ignore → ViewModel.Name = null
        /// Dùng khi: ViewModel không cần hiển thị Name,
        ///           hoặc sẽ tự tính sau khi map
        /// </summary>
        private static void RegisterStrategyA()
        {
            // Tạo config riêng biệt (không dùng GlobalSettings để demo từng strategy)
            // Trong project thực, bạn chỉ chọn 1 strategy cho mỗi mapping pair
            Console.WriteLine("   [Strategy A] Ignore hoàn toàn RoleOffshoreLocation.Name");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("   Code:");
            Console.WriteLine("   TypeAdapterConfig<RoleOffshoreLocation, RoleOffshoreLocationViewModel>.NewConfig()");
            Console.WriteLine("       .Ignore(dest => dest.Name);  ← bỏ qua, Name sẽ = null");
            Console.ResetColor();
            Console.WriteLine();
        }

        /// <summary>
        /// Strategy B: Ignore source getter-only + Map thủ công từ OffshoreLocationId
        /// Dùng khi: ViewModel.Name cần có giá trị (display name)
        /// ĐÂY LÀ CÁCH ĐƯỢC KHUYẾN NGHỊ - giống behavior [NotMapped] + AfterMap của AutoMapper
        /// </summary>
        private static void RegisterStrategyB()
        {
            Console.WriteLine("   [Strategy B] ✅ RECOMMENDED: Ignore + Map thủ công từ OffshoreLocationId");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("   Code:");
            Console.WriteLine("   TypeAdapterConfig<RoleOffshoreLocation, RoleOffshoreLocationViewModel>.NewConfig()");
            Console.WriteLine("       .Ignore(dest => dest.Name)            ← không map trực tiếp từ src.Name");
            Console.WriteLine("       .Map(dest => dest.Name,               ← tự tính lại từ Id");
            Console.WriteLine("            src => ((OffshoreLocationEnum)src.OffshoreLocationId).GetDescription())");
            Console.WriteLine("       .Map(dest => dest.LocationDisplayName,");
            Console.WriteLine("            src => ((OffshoreLocationEnum)src.OffshoreLocationId).GetDescription())");
            Console.WriteLine("       .Map(dest => dest.LocationEnumName,");
            Console.WriteLine("            src => ((OffshoreLocationEnum)src.OffshoreLocationId).ToString());");
            Console.ResetColor();
            Console.WriteLine();
        }

        /// <summary>
        /// Strategy C: Đổi tên field ViewModel để không conflict
        /// Dùng khi: ViewModel là của bạn và có thể đổi tên tự do
        /// </summary>
        private static void RegisterStrategyC()
        {
            Console.WriteLine("   [Strategy C] Đổi tên field trong ViewModel (tránh conflict)");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("   Code:");
            Console.WriteLine("   // ViewModel dùng LocationName thay vì Name:");
            Console.WriteLine("   TypeAdapterConfig<RoleOffshoreLocation, RoleOffshoreLocationDto>.NewConfig()");
            Console.WriteLine("       .Map(dest => dest.LocationName,");
            Console.WriteLine("            src => ((OffshoreLocationEnum)src.OffshoreLocationId).GetDescription());");
            Console.WriteLine("   // Không cần Ignore vì ViewModel không có property 'Name'");
            Console.ResetColor();
            Console.WriteLine();
        }

        private static void RegisterSelfMapping()
        {
            Console.WriteLine("   [Self-mapping] RoleOffshoreLocation → RoleOffshoreLocation (clone)");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("   TypeAdapterConfig<RoleOffshoreLocation, RoleOffshoreLocation>.NewConfig()");
            Console.WriteLine("       .Ignore(dest => dest.Name);  ← bắt buộc vì dest.Name cũng getter-only");
            Console.ResetColor();
            Console.WriteLine();
        }

        /// <summary>
        /// Đăng ký vào GlobalSettings và compile - gọi từ Program.cs
        /// </summary>
        public static void ApplyToGlobalSettings()
        {
            // ── Self-mapping: RoleOffshoreLocation → RoleOffshoreLocation ──
            TypeAdapterConfig<RoleOffshoreLocation, RoleOffshoreLocation>.NewConfig()
                .Ignore(dest => dest.Name);

            // ── Strategy B (Recommended): Ignore + Map thủ công ──
            TypeAdapterConfig<RoleOffshoreLocation, RoleOffshoreLocationViewModel>.NewConfig()
                .Ignore(dest => dest.Name)
                .Map(dest => dest.Name,
                     src => ((OffshoreLocationEnum)src.OffshoreLocationId).GetDescription())
                .Map(dest => dest.LocationDisplayName,
                     src => ((OffshoreLocationEnum)src.OffshoreLocationId).GetDescription())
                .Map(dest => dest.LocationEnumName,
                     src => ((OffshoreLocationEnum)src.OffshoreLocationId).ToString());

            // ── Strategy C: Map sang DTO với tên field khác ──
            TypeAdapterConfig<RoleOffshoreLocation, RoleOffshoreLocationDto>.NewConfig()
                .Map(dest => dest.LocationName,
                     src => ((OffshoreLocationEnum)src.OffshoreLocationId).GetDescription());
        }
    }
}
