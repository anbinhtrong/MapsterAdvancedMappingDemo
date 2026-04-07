using System.ComponentModel.DataAnnotations.Schema;
using MapsterDemo.Models.Offshore;

namespace MapsterDemo.Models.Offshore
{
    /// <summary>
    /// Domain model phản ánh đúng class thực tế của user:
    ///
    ///   public string Name => ((OffshoreLocationEnum)OffshoreLocationId).GetDescription();
    ///
    /// Đây là expression-bodied property (C# 6+), tương đương getter-only.
    /// Mapster KHÔNG thể gán vào đây → CompileException.
    ///
    /// Với AutoMapper: dùng [NotMapped] hoặc [IgnoreMap] để bỏ qua.
    /// Với Mapster: phải dùng .Ignore() trong TypeAdapterConfig.
    /// </summary>
    public class RoleOffshoreLocation : EntityBase
    {
        public int RoleId { get; set; }
        public int OffshoreLocationId { get; set; }
        public bool IsActive { get; set; }
        public string Note { get; set; }

        // ❌ NGUYÊN NHÂN LỖI CHÍNH:
        // Expression-bodied property = getter-only
        // Không có setter → Mapster tạo ra expression gán không hợp lệ
        // → CompileException: "Expression must be writeable (Parameter name: left)"
        //
        // Với AutoMapper: thêm [NotMapped] hoặc ForMember Ignore là xong
        // Với Mapster: KHÔNG có attribute tương đương [NotMapped],
        //              phải cấu hình trong TypeAdapterConfig
        public string Name => ((OffshoreLocationEnum)OffshoreLocationId).GetDescription();
    }

    /// <summary>
    /// Class cha chứa RoleOffshoreLocation bên trong, 
    /// dùng để demo lỗi lan truyền khi map object lồng nhau (nested mapping)
    /// </summary>
    public class RoleOffshoreLocationWrapper
    {
        public int Id { get; set; }
        public string WrapperName { get; set; }
        public RoleOffshoreLocation Location { get; set; }
    }

    /// <summary>
    /// Phiên bản AutoMapper: thêm [NotMapped] để AutoMapper tự bỏ qua
    /// (Đây là cách cũ, chỉ để so sánh)
    /// </summary>
    public class RoleOffshoreLocationAutoMapper : EntityBase
    {
        public int RoleId { get; set; }
        public int OffshoreLocationId { get; set; }
        public bool IsActive { get; set; }
        public string Note { get; set; }

        // ✅ AutoMapper: [NotMapped] báo AutoMapper bỏ qua property này
        // khi map sang/từ class này trong cùng tên property
        [NotMapped]
        public string Name => ((OffshoreLocationEnum)OffshoreLocationId).GetDescription();

        // Lưu ý: [NotMapped] là từ System.ComponentModel.DataAnnotations.Schema
        // AutoMapper đọc attribute này và tự động bỏ qua
        // Mapster KHÔNG đọc [NotMapped] → không có tác dụng
    }

    /// <summary>
    /// Class cha chứa RoleOffshoreLocationAutoMapper bên trong
    /// </summary>
    public class RoleOffshoreLocationAutoMapperWrapper
    {
        public int Id { get; set; }
        public string WrapperName { get; set; }
        public RoleOffshoreLocationAutoMapper Location { get; set; }
    }
}
