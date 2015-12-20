using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace TRPO_08_1.Models
{
    public class RegistrationViewModel
    {
        [DisplayName("Электронная почта")]
        public string Email { get; set; }
        [DisplayName("Пароль")]
        public string Password { get; set; }
        [DisplayName("Повторите пароль")]
        public string PasswordRepeat { get; set; }
        [DisplayName("Имя")]
        public string Name { get; set; }
        [DisplayName("Адрес")]
        public string Address { get; set; }
        [DisplayName("Телефон")]
        public string Phone { get; set; }
    }
}