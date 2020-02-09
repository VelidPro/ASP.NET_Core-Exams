using System.ComponentModel.DataAnnotations;

namespace RS1_Ispit_2017_06_21_v1.ViewModels
{
    public class LoginInputVM
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }

        [Display(Name="Remember me?")]
        public bool RememberMe { get; set; }
    }
}