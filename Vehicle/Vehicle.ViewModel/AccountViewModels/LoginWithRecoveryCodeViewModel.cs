using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Vehicle.ViewModel.AccountViewModels
{
    public class LoginWithRecoveryCodeViewModel
    {
            [Required(ErrorMessage = "This Field Required!")]
            [DataType(DataType.Text)]
            [Display(Name = "Recovery Code")]
            public string RecoveryCode { get; set; }
    }
}
