using System.ComponentModel.DataAnnotations;

namespace Blazor.Data
{
    public class ResetPassword
    {
        [Required(ErrorMessage = "التوكن مطلوب")]
        public string Token { get; set; }

        [Required(ErrorMessage = "كلمة المرور مطلوبة")]
        [MinLength(8, ErrorMessage = "يجب أن تكون كلمة المرور 8 أحرف على الأقل")]
        public string Password { get; set; }
    }
}
