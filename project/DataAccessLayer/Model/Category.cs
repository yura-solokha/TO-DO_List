
namespace DataAccessLayer.Model
{
    public partial class Category : EntityBase
    {
        public string Name { get; set; } = null!;

        public virtual ICollection<Task> Tasks { get; } = new List<Task>();
    }
}
