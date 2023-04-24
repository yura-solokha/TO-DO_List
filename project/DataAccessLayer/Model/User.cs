namespace DataAccessLayer.Model;

using Microsoft.AspNetCore.Identity;

public class User : IdentityUser<int>, EntityBase
{
    public string LastName { get; set; } = string.Empty;

    public string FirstName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    public virtual ICollection<Task> Tasks { get; } = new List<Task>();

    public override string ToString()
    {
        return $"{nameof(LastName)}: {LastName}, {nameof(FirstName)}: {FirstName}";
    }
}