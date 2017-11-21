using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Vehicle.ViewModel.Utils;

namespace Vehicle.ViewModel.VehicleBrand
{
    public class VehicleBrandEditViewModel : BaseViewModel
    {
        public int VehicleBrandID { get; set; }
        [Required(ErrorMessage = "This Field Required!")]
        public string VehicleBrandName { get; set; }
    }
}
