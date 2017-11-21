using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Vehicle.ViewModel.Utils;

namespace Vehicle.ViewModel.VehicleModel
{
    public class VehicleModelEditViewModel : BaseViewModel
    {
        public int VehicleModelID { get; set; }
        [Required(ErrorMessage = "This Field Required!")]
        public string VehicleModelName { get; set; }
        [Required(ErrorMessage = "This Field Required!")]
        public int VehicleBrandID { get; set; }
        public List<SelectListItem> VehicleBrandList { get; set; }

    }
}
