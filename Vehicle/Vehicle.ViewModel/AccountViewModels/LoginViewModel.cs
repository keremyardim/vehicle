﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Vehicle.ViewModel.AccountViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "This Field Required!")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "This Field Required!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}
