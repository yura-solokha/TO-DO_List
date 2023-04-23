using System.ComponentModel.DataAnnotations;

namespace TO_DO_List.Models.Tasks;

public class SubTaskViewModel
{
    [Required(ErrorMessage = "Введіть назву")]
    [StringLength(200, MinimumLength = 1, ErrorMessage = "Максимум 200 символів")]
    public string Name { get; set; } = string.Empty;

    public bool IsDone { get; set; } = false;

    [Required(ErrorMessage = "Введіть пріоритет")]
    [Range(0, 1)]
    public int Priority { get; set; } = 0;

    [Required(ErrorMessage = "Введіть опис")]
    [StringLength(500, MinimumLength = 1, ErrorMessage = "Максимум 500 символів")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Оберіть дедлайн")]
    [DataType(DataType.Date)]
    public DateOnly Deadline { get; set; }

    public int UserId { get; set; }
    public int ParentId { get; set; }
}