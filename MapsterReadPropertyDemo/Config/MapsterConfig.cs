using System;
using Mapster;
using MapsterDemo.Models;

namespace MapsterDemo.Config
{
    /// <summary>
    /// ✅ GIẢI PHÁP: TypeAdapterConfig đúng cách cho .NET Framework 4.8
    ///
    /// Chiến lược:
    ///   1. Bỏ qua tường minh các property getter-only và private set
    ///   2. Map computed fields thủ công từ source expressions
    ///   3. Đăng ký mapping cho nested types để Mapster tự xử lý List
    ///   4. Gọi Compile() để bắt lỗi sớm lúc khởi động app
    /// </summary>
    public static class MapsterConfig
    {
        public static void RegisterMappings()
        {
            var config = TypeAdapterConfig.GlobalSettings;

            // ──────────────────────────────────────────────────
            // FIX 1: Employee → Employee (self-mapping / clone)
            // Bỏ qua các property không thể ghi
            // ──────────────────────────────────────────────────
            TypeAdapterConfig<Employee, Employee>.NewConfig()
                .Ignore(dest => dest.Name)      // getter-only → không có setter
                .Ignore(dest => dest.FullName);  // private set → không ghi được từ ngoài

            // ──────────────────────────────────────────────────
            // FIX 2: Employee → EmployeeViewModel
            // Map computed fields bằng expression thủ công
            // ──────────────────────────────────────────────────
            TypeAdapterConfig<Employee, EmployeeViewModel>.NewConfig()
                .Map(dest => dest.DisplayName,
                     src => string.Format("{0} {1}", src.FirstName, src.LastName));
            // Lưu ý: EmployeeViewModel.DisplayName là public set → OK
            // Mapster tự copy FirstName, LastName, Department, Id

            // ──────────────────────────────────────────────────
            // FIX 3: RequestDetail → RequestDetailViewViewModel
            // Bỏ qua TotalPrice getter-only trên source
            // (Mapster không cố gán ngược lại source, nhưng nếu
            //  destination cũng có computed prop thì cần Ignore)
            // ──────────────────────────────────────────────────
            TypeAdapterConfig<RequestDetail, RequestDetailViewViewModel>.NewConfig()
                .Map(dest => dest.TotalPrice,
                     src => src.Quantity * src.UnitPrice)
                // Map nested Employee → EmployeeViewModel tự động
                // vì đã khai báo TypeAdapterConfig<Employee, EmployeeViewModel> ở trên
                .Map(dest => dest.Employee, src => src.Employee);

            // ──────────────────────────────────────────────────
            // FIX 4: Request → RequestViewModel
            // Map List<RequestDetail> → List<RequestDetailViewViewModel>
            // KHÔNG cần foreach - Mapster tự xử lý collection
            // ──────────────────────────────────────────────────
            TypeAdapterConfig<Request, RequestViewModel>.NewConfig()
                .Map(dest => dest.RequestDetailModels, src => src.RequestDetails);
            // Mapster tự convert từng element dựa vào config đã đăng ký ở FIX 3

            // ──────────────────────────────────────────────────
            // QUAN TRỌNG: Compile để bắt lỗi ngay lúc khởi động
            // Thay vì bị lỗi CompileException lúc map thật lần đầu
            // ──────────────────────────────────────────────────
            try
            {
                config.Compile();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("   ✅ TypeAdapterConfig.Compile() thành công - Không có lỗi cấu hình");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"   ❌ Compile thất bại: {ex.Message}");
                Console.ResetColor();
                throw; // Re-throw để app không khởi động với config lỗi
            }
        }
    }
}
