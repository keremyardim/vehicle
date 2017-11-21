using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Vehicle.DomainModel.Utils;

namespace Vehicle.DomainModel
{
    [Table("VehicleModel")]
    public class VehicleModel : BaseDomainModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int VehicleModelID { get; set; }
        [Required]
        public string VehicleModelName { get; set; }
        [Required]
        public int VehicleBrandID { get; set; }
        public VehicleBrand VehicleBrand { get; set; }
        public ICollection<Vehicle> Vehicles { get; set; }
    }
}
