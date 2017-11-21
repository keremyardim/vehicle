using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Vehicle.DomainModel.Utils;
using Model = Vehicle.DomainModel;

namespace Vehicle.DomainModel
{
    [Table("Vehicle")]
    public class Vehicle : BaseDomainModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? VehicleID { get; set; }
        [Required]
        public string PlateOfVehicle { get; set; }
        [Required]
        public string NicknameOfVehicle { get; set; }
        [Range(1990,2017)]
        [Required]
        public int ModelYearOfVehicle { get; set; }
        [Required]
        public string ColorOfVehicle { get; set; }
        [Required]
        public int TypeOfVehicleID { get; set; }
        public TypeOfVehicle TypeOfVehicle { get; set; }
        [Required]
        public int VehicleModelID { get; set; }
        public VehicleModel VehicleModel { get; set; }

    }

}
