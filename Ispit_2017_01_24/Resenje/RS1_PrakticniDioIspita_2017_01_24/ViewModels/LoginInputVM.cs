using System.ComponentModel.DataAnnotations;

namespace RS1_PrakticniDioIspita_2017_01_24.ViewModels
{
    public class LoginInputVM
    {
        [Required(ErrorMessage = "Morate unijeti username.")]
        public string Username { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Morate unijeti password.")]
        public string Password { get; set; }

        [Display(Name="Remember me?")]
        public bool RememberMe { get; set; }
    }
}