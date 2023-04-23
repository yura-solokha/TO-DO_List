namespace DataAccessLayer.Model;

public class Task : EntityBase
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public bool IsDone { get; set; }

    public int Priority { get; set; }

    public string Description { get; set; } = string.Empty;

    public DateOnly Deadline { get; set; }

    public int? CategoryId { get; set; }

    public int? ParentId { get; set; }

    public int UserId { get; set; }

    public virtual User User { get; set; }

    public virtual Category? Category { get; set; }

    public virtual Task? ParentTask { get; set; }

    public virtual ICollection<Task>? Subtasks { get; set; }

    public override string ToString()
    {
        return
            $"{nameof(Name)}: {Name}, {nameof(IsDone)}: {IsDone}, {nameof(Priority)}: {Priority}, {nameof(Description)}: {Description}, {nameof(Deadline)}: {Deadline}, {nameof(CategoryId)}: {CategoryId}, {nameof(ParentId)}: {ParentId}, {nameof(UserId)}: {UserId}, {nameof(User)}: {User}, {nameof(Category)}: {Category}, {nameof(ParentTask)}: {ParentTask}, {nameof(Subtasks)}: {Subtasks}";
    }
}