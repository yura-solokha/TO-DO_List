using System.ComponentModel.DataAnnotations;

namespace TO_DO_List.Models
{
    public class LoginUserViewModel
    {
        [Required]
        public string Login { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

    }
}
