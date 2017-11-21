using System;
using System.Collections.Generic;
using System.Text;
using Vehicle.ViewModel.Utils;
using Vehicle.ViewModel.VehicleBrand;

namespace Vehicle.ViewModel.VehicleModel
{
    public class VehicleModelViewModel : BaseViewModel
    {
        public int? VehicleModelID { get; set; }
        public string VehicleModelName { get; set; }
        public int? VehicleBrandID { get; set; }
        public string VehicleBrandName { get; set; }
    }
}
