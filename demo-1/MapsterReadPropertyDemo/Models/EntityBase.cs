namespace MapsterDemo.Models
{
    /// <summary>
    /// Base entity giả lập EntityBase trong project thực tế
    /// </summary>
    public abstract class EntityBase
    {
        public int Id { get; set; }
        public System.DateTime CreatedAt { get; set; }
        public System.DateTime? UpdatedAt { get; set; }
    }
}
