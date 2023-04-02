using System.ComponentModel.DataAnnotations;

namespace TO_DO_List.Models.User
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Введіть імʼя.")]
        [RegularExpression("^[А-Я][-А-я]+",
        ErrorMessage = "Імʼя має містити мінімум 1 символ.")]
        [StringLength(30, MinimumLength = 1)]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Введіть прізвище.")]
        [RegularExpression("^[А-Я][-А-я]+",
            ErrorMessage = "Прізвище має містити мінімум 1 символ.")]
        [StringLength(30, MinimumLength = 1)]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Введіть логін.")]
        [RegularExpression("^(?=.*[A-Za-z])(d*)[A-Za-z\\d]{3,32}$",
            ErrorMessage = "Логін має містити мінімум 3 символи.")]
        [StringLength(32, MinimumLength = 3)]
        public string Login { get; set; } = string.Empty;

        [Required(ErrorMessage = "Введіть пароль.")]
        [DataType(DataType.Password)]
        [RegularExpression("^(?=.*[A-Za-z])(?=.*\\d)[A-Za-z\\d]{3,32}$",
            ErrorMessage = "Пароль має містити мінімум 3 символи.")]
        [StringLength(32, MinimumLength = 3)]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Введіть підтвердження паролю.")]
        [DataType(DataType.Password)]
        [RegularExpression("^(?=.*[A-Za-z])(?=.*\\d)[A-Za-z\\d]{3,32}$",
            ErrorMessage = "Пароль має містити мінімум 3 символи.")]
        [StringLength(32, MinimumLength = 3)]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}