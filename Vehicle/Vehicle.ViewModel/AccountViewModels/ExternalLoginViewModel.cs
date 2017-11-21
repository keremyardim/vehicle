using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Vehicle.ViewModel.AccountViewModels
{
    public class ExternalLoginViewModel
    {
        [Required(ErrorMessage = "This Field Required!")]
        [EmailAddress]
        public string Email { get; set; }
    }
}
