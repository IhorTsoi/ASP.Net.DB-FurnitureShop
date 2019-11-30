using System.ComponentModel.DataAnnotations;




namespace FurnitureShop.ViewModels.Account
{
    public class LoginModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Не указан Email.")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Введите настоящий адрес.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Не указан пароль.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
