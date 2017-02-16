using System.ComponentModel.DataAnnotations;

namespace Arc.ActiveDirectory.Web.Model
{
    public class ResetPasswordModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string OldPassword { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [Compare("Password")]
        public string RepeatPassword { get; set; }
    }
}