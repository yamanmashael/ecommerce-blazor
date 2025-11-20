using System.ComponentModel.DataAnnotations;

namespace Blazor.Data
{
    public class Register
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Surname is required")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
        public string Password { get; set; }
    }

    public class GoogleLoginDto
    {
        public string IdToken { get; set; }
    }



    public class forgotPassword
    {
        public string Email { get; set; }
    }
    public class Login
    {
        public String UserName { get; set; }
        public String Password { get; set; }
    }
    public class LoginResponse
    {
        public string Token { get; set; }
        public long TokenExpired { get; set; }
        public string RefreshToken { get; set; }
        public string Role { get; set; }

    }

    public class RefreshTokenRequestDto
    {
        public string RefreshToken { get; set; }
    }

    public class UpdatePassword
    {
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }

    }
    public class AccountDto
    {
        public string Name { get; set; }


        public string Email { get; set; }
    }

    public class UpdateProfile
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }

    }

}
