using System;
using System.Collections.Generic;
using MapsterDemo.Models;

/// <summary>
/// Dữ liệu mẫu dùng chung cho BugCase và FixedCase
/// </summary>
public static class SeedData
{
    public static Request CreateSampleRequest()
    {
        var emp1 = new Employee("Nguyen", "Van An", "IT Department");
        var emp2 = new Employee("Tran", "Thi Bich", "HR Department");
        var emp3 = new Employee("Le", "Minh Duc", "Finance Department");

        return new Request
        {
            Id = 1,
            RequestCode = "REQ-2024-001",
            Title = "Mua thiết bị văn phòng Q4/2024",
            Status = "Pending",
            CreatedAt = new DateTime(2024, 10, 15),
            RequestDetails = new List<RequestDetail>
            {
                new RequestDetail
                {
                    Id = 1,
                    Description = "Laptop Dell XPS 15",
                    Quantity = 3,
                    UnitPrice = 35_000_000m,
                    CreatedAt = new DateTime(2024, 10, 15),
                    Employee = emp1
                },
                new RequestDetail
                {
                    Id = 2,
                    Description = "Màn hình LG 27\" 4K",
                    Quantity = 5,
                    UnitPrice = 12_500_000m,
                    CreatedAt = new DateTime(2024, 10, 15),
                    Employee = emp2
                },
                new RequestDetail
                {
                    Id = 3,
                    Description = "Bàn phím cơ Keychron K2",
                    Quantity = 10,
                    UnitPrice = 2_800_000m,
                    CreatedAt = new DateTime(2024, 10, 16),
                    Employee = emp3
                }
            }
        };
    }
}
