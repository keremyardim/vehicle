using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Vehicle.ViewModel.Utils;

namespace Vehicle.ViewModel.TypeOfVehicle
{
    public class TypeOfVehicleEditViewModel : BaseViewModel
    {
        public int TypeOfVehicleID { get; set; }
        [Required(ErrorMessage = "This Field Required!")]
        public string TypeName { get; set; }
    }
}
