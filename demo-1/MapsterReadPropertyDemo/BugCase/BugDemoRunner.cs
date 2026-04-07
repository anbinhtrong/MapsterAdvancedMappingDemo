using System;
using Mapster;
using MapsterDemo.Models;

namespace MapsterDemo.BugCase
{
    /// <summary>
    /// ❌ DEMO LỖI: Sử dụng Mapster với config mặc định
    /// 
    /// Lỗi sẽ xảy ra tại runtime khi Mapster compile expression tree:
    ///   CompileException: Error while compiling
    ///   Expression must be writeable (Parameter name: left)
    ///
    /// Nguyên nhân: Mapster cố gán giá trị vào:
    ///   1. Employee.Name      → getter-only, không có setter
    ///   2. Employee.FullName  → private set, không thể gán từ bên ngoài
    ///   3. RequestDetail.TotalPrice → getter-only
    /// </summary>
    public static class BugDemoRunner
    {
        public static void Run()
        {
            Console.WriteLine("╔══════════════════════════════════════════════════════╗");
            Console.WriteLine("║           ❌  BUG CASE - Mapster Default Config      ║");
            Console.WriteLine("╚══════════════════════════════════════════════════════╝");
            Console.WriteLine();

            // Reset về config mặc định để demo lỗi
            // (Không cần clear trong version này)


            var request = SeedData.CreateSampleRequest();

            Console.WriteLine("📋 Dữ liệu đầu vào:");
            Console.WriteLine($"   Request: [{request.RequestCode}] {request.Title}");
            Console.WriteLine($"   RequestDetails count: {request.RequestDetails.Count}");
            Console.WriteLine();

            // ─── DEMO LỖI 1: Map Employee → Employee (self-mapping) ───
            Console.WriteLine("── Test 1: Map Employee → Employee (self-mapping) ──");
            try
            {
                var emp = request.RequestDetails[0].Employee;
                Console.WriteLine($"   Source: {emp.FirstName} {emp.LastName}");
                Console.WriteLine($"   Source.Name (getter-only): \"{emp.Name}\"");
                Console.WriteLine($"   Source.FullName (private set): \"{emp.FullName}\"");
                Console.WriteLine();
                Console.WriteLine("   → Đang gọi emp.Adapt<Employee>() ...");

                // ❌ Lỗi xảy ra ở đây
                var cloned = emp.Adapt<Employee>();

                Console.WriteLine($"   ✅ Không lỗi (unexpected): {cloned.Name}");
            }
            catch (Exception ex)
            {
                PrintError("Map Employee → Employee", ex);
            }

            Console.WriteLine();

            // ─── DEMO LỖI 2: Map RequestDetail → RequestDetailViewViewModel ───
            Console.WriteLine("── Test 2: Map RequestDetail → RequestDetailViewViewModel ──");
            try
            {
                var detail = request.RequestDetails[0];
                Console.WriteLine($"   Source.TotalPrice (getter-only): {detail.TotalPrice:C}");
                Console.WriteLine();
                Console.WriteLine("   → Đang gọi detail.Adapt<RequestDetailViewViewModel>() ...");

                // ❌ Lỗi có thể xảy ra do nested Employee mapping
                var vm = detail.Adapt<RequestDetailViewViewModel>();

                Console.WriteLine($"   ✅ Không lỗi (unexpected): {vm.Description}");
            }
            catch (Exception ex)
            {
                PrintError("Map RequestDetail → RequestDetailViewViewModel", ex);
            }

            Console.WriteLine();

            // ─── DEMO LỖI 3: Map Request → RequestViewModel (full) ───
            Console.WriteLine("── Test 3: Map Request → RequestViewModel (full mapping) ──");
            try
            {
                Console.WriteLine("   → Đang gọi request.Adapt<RequestViewModel>() ...");

                // ❌ Lỗi cascade từ nested mapping
                var vm = request.Adapt<RequestViewModel>();

                Console.WriteLine($"   ✅ Không lỗi (unexpected): {vm.RequestCode}");
            }
            catch (Exception ex)
            {
                PrintError("Map Request → RequestViewModel", ex);
            }

            Console.WriteLine();
            Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
            Console.WriteLine("💡 Kết luận: Mapster mặc định cố gán vào readonly/private");
            Console.WriteLine("   properties → sinh ra Expression tree không hợp lệ");
            Console.WriteLine("   → CompileException lúc runtime");
            Console.WriteLine();
        }

        private static void PrintError(string testName, Exception ex)
        {
            var inner = ex.InnerException ?? ex;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"   ❌ LỖI tại [{testName}]:");
            Console.WriteLine($"      Exception: {ex.GetType().Name}");
            Console.WriteLine($"      Message  : {ex.Message}");
            if (ex.InnerException != null)
            {
                Console.WriteLine($"      Inner    : {ex.InnerException.Message}");
            }
            Console.ResetColor();
        }
    }
}
