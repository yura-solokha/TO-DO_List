namespace DataAccessLayer.Model;

public class Category : EntityBase
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public virtual ICollection<Task> Tasks { get; } = new List<Task>();

    public override string ToString()
    {
        return $"{nameof(Name)}: {Name}";
    }
}