namespace MapsterDemo.Models
{
    /// <summary>
    /// Class Employee gốc gây ra lỗi Mapster CompileException:
    ///   - Name: chỉ có getter (getter-only) → Mapster không thể ghi
    ///   - FullName: private set → Mapster không thể ghi từ bên ngoài
    /// </summary>
    public class Employee : EntityBase
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Department { get; set; }

        // ❌ NGUYÊN NHÂN LỖI 1: private set
        // Mapster cố gán giá trị nhưng không có quyền truy cập
        public string FullName { get; private set; }

        // ❌ NGUYÊN NHÂN LỖI 2: getter-only (không có setter)
        // Mapster sinh ra expression để gán nhưng không thể ghi → CompileException
        public string Name
        {
            get { return string.Format("{0} {1}", FirstName, LastName); }
        }

        // Constructor để thiết lập FullName (đây là cách đúng trong domain model)
        public Employee() { }

        public Employee(string firstName, string lastName, string department)
        {
            FirstName = firstName;
            LastName = lastName;
            Department = department;
            FullName = string.Format("{0} {1}", firstName, lastName);
        }
    }
}
