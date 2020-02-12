using System.ComponentModel.DataAnnotations;

namespace Ispit_2017_02_15.ViewModels
{
    public class LoginInputVM
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}