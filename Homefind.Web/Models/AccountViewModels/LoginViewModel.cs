using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Homefind.Web.Models.PropertyViewModels;

namespace Homefind.Web.Models.AccountViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public LoginViewModel()
        {
            Errors = new List<string>();
        }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }

        public List<string> Errors { get; set; }
    }
}
