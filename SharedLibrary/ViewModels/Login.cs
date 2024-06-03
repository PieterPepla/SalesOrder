using System.ComponentModel.DataAnnotations;

namespace SharedLibrary.ViewModels
{
    public class Login
    {
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}
