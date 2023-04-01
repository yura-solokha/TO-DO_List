using System.ComponentModel.DataAnnotations;
using DataAccessLayer.Model;

namespace TO_DO_List.Models.Tasks;

public class SubTaskViewModel
{
    [Required] public string Name { get; set; } = string.Empty;

    public bool IsDone { get; set; } = false;

    [Required] public int Priority { get; set; } = 0;

    [Required] public string Description { get; set; } = string.Empty;

    public DateOnly Deadline { get; set; }

    public int UserId { get; set; } = 1;
    public int ParentId { get; set; }
}