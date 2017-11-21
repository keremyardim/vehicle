using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Vehicle.DomainModel.Utils;
using Model = Vehicle.DomainModel;

namespace Vehicle.DomainModel
{
    [Table("TypeOfVehicle")]
    public class TypeOfVehicle : BaseDomainModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TypeOfVehicleID { get; set; }
        [Required]
        public string TypeName { get; set; }
        public ICollection<Vehicle> Vehicle { get; set; }

    }
}
