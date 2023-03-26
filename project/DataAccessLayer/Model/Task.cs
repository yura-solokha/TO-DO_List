
namespace DataAccessLayer.Model
{
    public partial class Task : EntityBase
    {
        public string Name { get; set; } = null!;
        public bool IsDone { get; set; }
        public int Priority { get; set; }
        public int CategoryId { get; set; }
        public int ParentId { get; set; }

        public virtual Category? Category { get; set; }
    }
}
