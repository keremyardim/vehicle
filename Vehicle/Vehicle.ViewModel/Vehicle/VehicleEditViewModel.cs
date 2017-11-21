using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Vehicle.ViewModel.TypeOfVehicle;
using Vehicle.ViewModel.Utils;
using Vehicle.ViewModel.VehicleBrand;
using Vehicle.ViewModel.VehicleModel;

namespace Vehicle.ViewModel.Vehicle
{
    public class VehicleEditViewModel : BaseViewModel
    {
        public VehicleEditViewModel()
        {
            ModelYearOfVehicle = 1990;
        }
        public int? VehicleID { get; set; }
        [Required(ErrorMessage = "This Field Required!")]
        [RegularExpression(@"\d{2}\s?[A-Z]{1,3}\s?\d{2,4}", ErrorMessage = "Entered Plate format is not valid.(XX XX XXXX)")]
        [Display(Name = "Plate Of Vehicle")]
        public string PlateOfVehicle { get; set; }
        [Required(ErrorMessage = "This Field Required!")]
        [Display(Name = "Nickname Of Vehicle")]
        public string NicknameOfVehicle { get; set; }
        [Range(1990, 2017)]
        [Required(ErrorMessage = "This Field Required!")]
        [Display(Name = "ModelYear Of Vehicle")]
        public int ModelYearOfVehicle { get; set; }
        [Required(ErrorMessage = "This Field Required!")]
        [Display(Name = "Color Of Vehicle")]
        public string ColorOfVehicle { get; set; }

        [Required(ErrorMessage = "This Field Required!")]
        [Display(Name = "Type Of Vehicle")]
        public int TypeOfVehicleID { get; set; }
        public List<SelectListItem> TypeOfVehicleList { get; set; }

        [Required(ErrorMessage = "This Field Required!")]
        [Display(Name = "Vehicle Brand")]
        public int VehicleBrandID { get; set; }
        public List<SelectListItem> VehicleBrandList { get; set; }

        [Required(ErrorMessage = "This Field Required!")]
        [Display(Name = "Vehicle Model")]
        public int VehicleModelID { get; set; }
        public List<SelectListItem> VehicleModelList { get; set; }

    }
}
