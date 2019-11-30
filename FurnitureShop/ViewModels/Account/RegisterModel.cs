using System.ComponentModel.DataAnnotations;


namespace FurnitureShop.ViewModels.Account
{
    public class RegisterModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Не указано имя")]
        [DataType(DataType.Text)]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Не указан Email")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Введите настоящий адрес.")]
        public string Email { get; set; }

        [DataType(DataType.PhoneNumber)]
        [RegularExpression("^(?!0+$)(\\+\\d{1,3}[- ]?)?(?!0+$)\\d{10,15}$", ErrorMessage = "Введите настоящий телефон.")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Не указан пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пароль введен неверно")]
        public string ConfirmPassword { get; set; }
    }
}
