using System;
using System.ComponentModel;
using System.Reflection;

namespace MapsterDemo.Models.Offshore
{
    /// <summary>
    /// Enum giả lập OffshoreLocationEnum trong project thực tế
    /// Dùng [Description] attribute để lưu display name
    /// </summary>
    public enum OffshoreLocationEnum
    {
        [Description("Chưa xác định")]
        Unknown = 0,

        [Description("Hà Nội")]
        Hanoi = 1,

        [Description("Hồ Chí Minh")]
        HoChiMinh = 2,

        [Description("Đà Nẵng")]
        DaNang = 3,

        [Description("Cần Thơ")]
        CanTho = 4,

        [Description("Hải Phòng")]
        HaiPhong = 5
    }

    /// <summary>
    /// Extension method GetDescription() - pattern phổ biến trong legacy .NET project
    /// Đọc [Description] attribute từ enum value
    /// </summary>
    public static class EnumExtensions
    {
        public static string GetDescription(this Enum value)
        {
            if (value == null) return string.Empty;

            FieldInfo field = value.GetType().GetField(value.ToString());
            if (field == null) return value.ToString();

            DescriptionAttribute attr = field.GetCustomAttribute<DescriptionAttribute>();
            return attr != null ? attr.Description : value.ToString();
        }
    }
}
