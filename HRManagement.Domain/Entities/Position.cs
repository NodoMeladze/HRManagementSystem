namespace HRManagement.Domain.Entities
{
    public class Position : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public Guid? ParentId { get; set; }
        public Position? Parent { get; set; }
        public int Level { get; set; }
        public ICollection<Position> Children { get; set; } = [];
        public ICollection<Employee> Employees { get; set; } = [];
    }
}
