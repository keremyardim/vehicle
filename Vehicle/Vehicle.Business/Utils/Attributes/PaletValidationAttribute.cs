using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Vehicle.Business.ServiceContract;

namespace Vehicle.Business.Utils.Attributes
{
    public class PaletValidationAttribute : ValidationAttribute, IClientModelValidator
    {
        private readonly IVehicleService _vehicleService;
        public PaletValidationAttribute(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }
        private int _year;

        public PaletValidationAttribute(int Year)
        {
            _year = Year;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var vehicle = _vehicleService.GetVehicleForPaletNumber(value.ToString());

            if (vehicle != null)
            {
                return new ValidationResult("This Vehicle Plate number already exists!");
            }

            return ValidationResult.Success;
        }

        public void AddValidation(ClientModelValidationContext context)
        {
        }
    }
}
