using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Vehicle.ViewModel.Utils;

namespace Vehicle.ViewModel.Vehicle
{
    public class VehicleViewModel : BaseViewModel
    {
        public int VehicleID { get; set; }
        [Display(Name = "Plate Of Vehicle")]
        public string PlateOfVehicle { get; set; }
        [Display(Name = "Nickname Of Vehicle")]
        public string NicknameOfVehicle { get; set; }
        [Display(Name = "Model Year Of Vehicle")]
        public int ModelYearOfVehicle { get; set; }
        [Display(Name = "Color Of Vehicle")]
        public string ColorOfVehicle { get; set; }
        [Display(Name = "Type Of Vehicle Name")]
        public string TypeOfVehicleName { get; set; }
        [Display(Name = "Vehicle Brand Name")]
        public string VehicleBrandName { get; set; }
        [Display(Name = "Vehicle Model Name")]
        public string VehicleModelName { get; set; }
    }
}
