using System;
using MapsterDemo.BugCase;
using MapsterDemo.Config;
using MapsterDemo.FixedCase;

namespace MapsterReadPropertyDemo
{
    /// <summary>
    /// Demo console app minh họa lỗi Mapster CompileException
    /// và cách khắc phục trong dự án ASP.NET MVC .NET Framework 4.8
    ///
    /// ┌─────────────────────────────────────────────────────┐
    /// │  PHẦN 1: Bug - Employee (getter-only, private set)  │
    /// │  PHẦN 2: Bug - RoleOffshoreLocation (Enum computed) │
    /// │  PHẦN 3: Fix Employee + Request mapping             │
    /// │  PHẦN 4: Fix RoleOffshoreLocation (3 strategies)    │
    /// └─────────────────────────────────────────────────────┘
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            PrintHeader();

            // ════════════════════════════════════════════════════════
            // PHẦN 1: DEMO LỖI - Employee (getter-only + private set)
            // ════════════════════════════════════════════════════════
            PrintSectionHeader("PHẦN 1", "BUG - Employee (getter-only + private set)");
            BugDemoRunner.Run();
            Pause();

            // ════════════════════════════════════════════════════════
            // PHẦN 2: DEMO LỖI - RoleOffshoreLocation (Enum computed)
            // ════════════════════════════════════════════════════════
            PrintSectionHeader("PHẦN 2", "BUG - RoleOffshoreLocation (Computed Enum Property + [NotMapped])");
            OffshoreBugDemoRunner.Run();
            Pause();

            // ════════════════════════════════════════════════════════
            // PHẦN 3: ĐĂNG KÝ CONFIG + FIX Employee & Request
            // ════════════════════════════════════════════════════════
            PrintSectionHeader("PHẦN 3", "FIX - Đăng ký TypeAdapterConfig (giống Global.asax Application_Start)");
            Console.WriteLine("   → Đang chạy MapsterConfig.RegisterMappings() ...");
            Console.WriteLine();
            MapsterConfig.RegisterMappings();
            Console.WriteLine();

            PrintSectionHeader("PHẦN 3b", "FIX - Employee + Request → ViewModel mapping");
            FixedDemoRunner.Run();
            Pause();

            // ════════════════════════════════════════════════════════
            // PHẦN 4: FIX RoleOffshoreLocation - 3 Strategies
            // ════════════════════════════════════════════════════════
            PrintSectionHeader("PHẦN 4", "FIX - RoleOffshoreLocation: 3 Strategies thay thế [NotMapped]");
            Console.WriteLine("   Giải thích từng strategy:");
            OffshoreMapsterConfig.RegisterMappings();

            PrintSectionHeader("PHẦN 4b", "FIX - Demo kết quả thực tế của 3 Strategies");
            OffshoreFixedDemoRunner.Run();
            Pause();

            // ════════════════════════════════════════════════════════
            // TỔNG KẾT
            // ════════════════════════════════════════════════════════
            PrintFinalSummary();

            Console.WriteLine();
            Console.WriteLine("Nhấn phím bất kỳ để thoát...");
            Console.ReadKey();
        }

        static void PrintHeader()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("╔══════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║   MAPSTER DEMO v2 - CompileException: Expression must be    ║");
            Console.WriteLine("║   writeable - ASP.NET MVC .NET Framework 4.8                ║");
            Console.WriteLine("╠══════════════════════════════════════════════════════════════╣");
            Console.WriteLine("║  Covers:                                                     ║");
            Console.WriteLine("║  • Employee.Name (getter-only)                               ║");
            Console.WriteLine("║  • Employee.FullName (private set)                           ║");
            Console.WriteLine("║  • RoleOffshoreLocation.Name (computed tu Enum)              ║");
            Console.WriteLine("║  • AutoMapper [NotMapped] vs Mapster equivalent              ║");
            Console.WriteLine("║  • 3 Strategies xu ly computed Enum property                 ║");
            Console.WriteLine("╚══════════════════════════════════════════════════════════════╝");
            Console.ResetColor();
            Console.WriteLine();
        }

        static void PrintSectionHeader(string part, string title)
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("═══════════════════════════════════════════════════════════════");
            Console.WriteLine($"  {part}: {title}");
            Console.WriteLine("═══════════════════════════════════════════════════════════════");
            Console.ResetColor();
            Console.WriteLine();
        }

        static void Pause()
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("  [Enter de tiep tuc...]");
            Console.ResetColor();
            Console.ReadLine();
        }

        static void PrintFinalSummary()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("╔══════════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                     TONG KET GIAI PHAP                          ║");
            Console.WriteLine("╠════════════════════════════╦═════════════════════════════════════╣");
            Console.WriteLine("║  Van de                    ║  Giai phap Mapster                  ║");
            Console.WriteLine("╠════════════════════════════╬═════════════════════════════════════╣");
            Console.WriteLine("║  getter-only property      ║  .Ignore(dest => dest.Prop)         ║");
            Console.WriteLine("║  private set property      ║  .Ignore(dest => dest.Prop)         ║");
            Console.WriteLine("║  Computed tu Enum (Name)   ║  .Ignore() + .Map(dest, src=>expr)  ║");
            Console.WriteLine("║  [NotMapped] AutoMapper    ║  Khong co equivalent -> dung .Ignore║");
            Console.WriteLine("║  List mapping (foreach)    ║  list.Adapt<List<TDest>>()          ║");
            Console.WriteLine("║  Full object mapping       ║  obj.Adapt<TDest>()                 ║");
            Console.WriteLine("║  Bat loi som               ║  config.Compile() luc startup       ║");
            Console.WriteLine("╠════════════════════════════╩═════════════════════════════════════╣");
            Console.WriteLine("║  3 Strategies cho Computed Enum Property:                        ║");
            Console.WriteLine("║  A) .Ignore()              -> ViewModel.Name = null              ║");
            Console.WriteLine("║  B) .Ignore() + .Map()     -> ViewModel.Name = GetDescription() ║");
            Console.WriteLine("║  C) Doi ten ViewModel      -> LocationName thay vi Name          ║");
            Console.WriteLine("╚══════════════════════════════════════════════════════════════════╝");
            Console.ResetColor();
        }
    }
}
