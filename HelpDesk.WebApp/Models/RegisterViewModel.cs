using HelpDesk.WebApp.Resources;
using System.ComponentModel.DataAnnotations;

namespace HelpDesk.WebApp.Models
{
    public class RegisterViewModel
    {
        /*[Required]
        [Display(Name = "Label_FM", ResourceType = typeof(Resource))]
        public string FM { get; set; }

        [Required]
        [Display(Name = "Label_IM", ResourceType = typeof(Resource))]
        public string IM { get; set; }

        [Required]
        [Display(Name = "Label_OT", ResourceType = typeof(Resource))]
        public string OT { get; set; }*/

        [Required]
        [EmailAddress]
        [Display(Name = "Label_Email", ResourceType = typeof(Resource))]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessageResourceName = "ErrorMessage_StringLengthConstraint", MinimumLength = 5, ErrorMessageResourceType = typeof(Resource))]
        [DataType(DataType.Password)]
        [Display(Name = "Label_Password", ResourceType = typeof(Resource))]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Label_ConfirmPassword", ResourceType = typeof(Resource))]
        [Compare("Password", ErrorMessageResourceName = "ErrorMessage_ConfirmPassword", ErrorMessageResourceType = typeof(Resource))]
        public string ConfirmPassword { get; set; }
    }
}
