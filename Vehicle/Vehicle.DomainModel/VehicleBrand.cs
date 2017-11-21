using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Vehicle.DomainModel.Utils;
using Model = Vehicle.DomainModel;

namespace Vehicle.DomainModel
{
    [Table("VehicleBrand")]
    public class VehicleBrand : BaseDomainModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int VehicleBrandID { get; set; }
        [Required]
        public string VehicleBrandName { get; set; }
        public ICollection<VehicleModel> VehicleModels { get; set; }
    }
}
