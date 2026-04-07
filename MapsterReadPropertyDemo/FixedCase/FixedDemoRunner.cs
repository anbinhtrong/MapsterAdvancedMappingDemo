using System;
using System.Collections.Generic;
using Mapster;
using MapsterDemo.Models;

namespace MapsterDemo.FixedCase
{
    /// <summary>
    /// ✅ DEMO FIX: Mapster với TypeAdapterConfig đúng cách
    /// </summary>
    public static class FixedDemoRunner
    {
        public static void Run()
        {
            Console.WriteLine("╔══════════════════════════════════════════════════════╗");
            Console.WriteLine("║         ✅  FIXED CASE - Mapster Correct Config      ║");
            Console.WriteLine("╚══════════════════════════════════════════════════════╝");
            Console.WriteLine();

            var request = SeedData.CreateSampleRequest();

            // ─── TEST 1: Employee → Employee (self-mapping) ───
            Console.WriteLine("── Test 1: Employee → Employee (self-mapping / clone) ──");
            try
            {
                var emp = request.RequestDetails[0].Employee;
                Console.WriteLine($"   Source  → Name: \"{emp.Name}\" | FullName: \"{emp.FullName}\"");

                var cloned = emp.Adapt<Employee>();

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"   Cloned  → FirstName: \"{cloned.FirstName}\" | LastName: \"{cloned.LastName}\"");
                Console.WriteLine($"             Name (recomputed): \"{cloned.Name}\"");
                Console.WriteLine($"             FullName (ignored, giữ default): \"{cloned.FullName ?? "(null)"}\"");
                Console.ResetColor();
                Console.WriteLine("   → Lưu ý: FullName bị Ignore nên không được copy,");
                Console.WriteLine("     nhưng Name được tính lại từ FirstName + LastName ✅");
            }
            catch (Exception ex)
            {
                PrintError(ex);
            }

            Console.WriteLine();

            // ─── TEST 2: Employee → EmployeeViewModel ───
            Console.WriteLine("── Test 2: Employee → EmployeeViewModel ──");
            try
            {
                var emp = request.RequestDetails[0].Employee;
                var vm = emp.Adapt<EmployeeViewModel>();

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"   ViewModel.Id          : {vm.Id}");
                Console.WriteLine($"   ViewModel.FirstName   : {vm.FirstName}");
                Console.WriteLine($"   ViewModel.LastName    : {vm.LastName}");
                Console.WriteLine($"   ViewModel.Department  : {vm.Department}");
                Console.WriteLine($"   ViewModel.DisplayName : {vm.DisplayName}");
                Console.ResetColor();
                Console.WriteLine("   → DisplayName được map thủ công từ expression ✅");
            }
            catch (Exception ex)
            {
                PrintError(ex);
            }

            Console.WriteLine();

            // ─── TEST 3: RequestDetail → RequestDetailViewViewModel ───
            Console.WriteLine("── Test 3: RequestDetail → RequestDetailViewViewModel ──");
            try
            {
                var detail = request.RequestDetails[0];
                Console.WriteLine($"   Source → Qty: {detail.Quantity} | UnitPrice: {detail.UnitPrice:C} | TotalPrice (getter-only): {detail.TotalPrice:C}");

                var vm = detail.Adapt<RequestDetailViewViewModel>();

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"   ViewModel.Description : {vm.Description}");
                Console.WriteLine($"   ViewModel.Quantity    : {vm.Quantity}");
                Console.WriteLine($"   ViewModel.UnitPrice   : {vm.UnitPrice:C}");
                Console.WriteLine($"   ViewModel.TotalPrice  : {vm.TotalPrice:C}");
                Console.WriteLine($"   ViewModel.Employee.DisplayName: {vm.Employee?.DisplayName}");
                Console.ResetColor();
                Console.WriteLine("   → Nested Employee tự động map sang EmployeeViewModel ✅");
            }
            catch (Exception ex)
            {
                PrintError(ex);
            }

            Console.WriteLine();

            // ─── TEST 4: Map List<RequestDetail> - KHÔNG cần foreach ───
            Console.WriteLine("── Test 4: Map List<RequestDetail> → List<RequestDetailViewViewModel> (không foreach) ──");
            try
            {
                // ❌ Cách cũ AutoMapper / manual:
                // var list = new List<RequestDetailViewViewModel>();
                // foreach (var detail in request.RequestDetails)
                //     list.Add(Mapper.Map<RequestDetailViewViewModel>(detail));

                // ✅ Cách mới với Mapster:
                var list = request.RequestDetails.Adapt<List<RequestDetailViewViewModel>>();

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"   Source count : {request.RequestDetails.Count}");
                Console.WriteLine($"   Result count : {list.Count}");
                foreach (var item in list)
                {
                    Console.WriteLine($"   → [{item.Id}] {item.Description} | {item.Quantity} x {item.UnitPrice:C} = {item.TotalPrice:C} | By: {item.Employee?.DisplayName}");
                }
                Console.ResetColor();
                Console.WriteLine("   → Mapster tự xử lý toàn bộ collection ✅");
            }
            catch (Exception ex)
            {
                PrintError(ex);
            }

            Console.WriteLine();

            // ─── TEST 5: Request → RequestViewModel (full, bao gồm nested list) ───
            Console.WriteLine("── Test 5: Request → RequestViewModel (full mapping với nested List) ──");
            try
            {
                var vm = request.Adapt<RequestViewModel>();

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"   ViewModel.Id           : {vm.Id}");
                Console.WriteLine($"   ViewModel.RequestCode  : {vm.RequestCode}");
                Console.WriteLine($"   ViewModel.Title        : {vm.Title}");
                Console.WriteLine($"   ViewModel.Status       : {vm.Status}");
                Console.WriteLine($"   ViewModel.RequestDetailModels.Count: {vm.RequestDetailModels?.Count}");
                Console.WriteLine();
                Console.WriteLine("   Chi tiết RequestDetailModels:");
                foreach (var detail in vm.RequestDetailModels)
                {
                    Console.WriteLine($"   → [{detail.Id}] {detail.Description,-25} TotalPrice: {detail.TotalPrice,10:C}  Employee: {detail.Employee?.DisplayName}");
                }
                Console.ResetColor();
                Console.WriteLine();
                Console.WriteLine("   → List<RequestDetail> → List<RequestDetailViewViewModel> tự động ✅");
                Console.WriteLine("   → KHÔNG cần foreach thủ công ✅");
            }
            catch (Exception ex)
            {
                PrintError(ex);
            }

            Console.WriteLine();
            Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
        }

        private static void PrintError(Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"   ❌ LỖI (không mong đợi): {ex.GetType().Name}: {ex.Message}");
            Console.ResetColor();
        }
    }
}
