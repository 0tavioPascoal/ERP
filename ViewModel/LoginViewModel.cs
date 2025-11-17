using System.ComponentModel.DataAnnotations;

namespace ERP.ViewModel {
    public class LoginViewModel {

        [Required(ErrorMessage = "Name")]
        [Display(Name = "Usuario")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        public  string ReturnUrl { get; set; }
    }
}
