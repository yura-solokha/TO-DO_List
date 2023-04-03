using System.Collections.Generic;

namespace DataAccessLayer.Model;

public class User : EntityBase
{
    public string LastName { get; set; } = string.Empty;

    public string FirstName { get; set; } = string.Empty;

    public string Login { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public virtual ICollection<Task> Tasks { get; } = new List<Task>();
    
    public override string ToString()
    {
        return $"{nameof(LastName)}: {LastName}, {nameof(FirstName)}: {FirstName}, {nameof(Login)}: {Login}";
    }
}