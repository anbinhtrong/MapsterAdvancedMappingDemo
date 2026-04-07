using MapsterDemo.Models.Offshore;

namespace MapsterDemo.Models.Offshore
{
    /// <summary>
    /// ViewModel nhận kết quả mapping từ RoleOffshoreLocation
    /// </summary>
    public class RoleOffshoreLocationViewModel
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public int OffshoreLocationId { get; set; }
        public bool IsActive { get; set; }
        public string Note { get; set; }

        // ViewModel có property Name với public set → có thể nhận giá trị
        // Nhưng source (RoleOffshoreLocation.Name) là getter-only
        // → Mapster gặp lỗi khi cố map trùng tên
        public string Name { get; set; }

        // Thêm field để hiển thị rõ enum label
        public string LocationDisplayName { get; set; }
        public string LocationEnumName { get; set; }
    }

    /// <summary>
    /// ViewModel dùng cho trường hợp self-mapping (clone object)
    /// Map RoleOffshoreLocation → RoleOffshoreLocation
    /// </summary>
    public class RoleOffshoreLocationDto
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public int OffshoreLocationId { get; set; }
        public bool IsActive { get; set; }
        public string Note { get; set; }
        public string LocationName { get; set; } // Đổi tên để tránh conflict
    }
}
