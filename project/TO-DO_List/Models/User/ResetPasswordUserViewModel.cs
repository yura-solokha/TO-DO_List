using System.ComponentModel.DataAnnotations;

namespace TO_DO_List.Models.User
{
    public class ResetPasswordUserViewModel
    {
        [Required]
        public string Login { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
