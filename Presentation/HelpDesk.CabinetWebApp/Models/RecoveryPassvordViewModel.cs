using System.ComponentModel.DataAnnotations;
using HelpDesk.CabinetWebApp.Resources;

namespace HelpDesk.CabinetWebApp.Models
{
    public class RecoveryPassvordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Label_Email", ResourceType = typeof(Resource))]
        public string Email { get; set; }

        /// <summary>
        /// Сообщение отправлено
        /// </summary>
        public bool IsSend { get; set; }
    }
}
