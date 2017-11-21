using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Vehicle.ViewModel.Utils
{
    public class BaseViewModel
    {
        private short? _state;
        public short? State
        {
            get
            {
                return _state ?? 0;
            }
            set
            {
                _state = value;
            }
        }
        [Display(Name = "State")]
        public string StateName
        {
            get
            {
                return (State.HasValue) ? (State == 1 ? "Active" : "Pasive") : "";
            }
        }
        public DateTime? CreationDate { get; set; }
        [Display(Name = "Creation Date")]
        public string CreationDateString
        {
            get
            {
                return (CreationDate.HasValue) ? CreationDate.Value.ToString("dd.MM.yyyy hh:ss") : "";
            }
        }
        public DateTime? ModifiedDate { get; set; }
        [Display(Name = "Modified Date")]
        public string ModifiedDateString
        {
            get
            {
                return (ModifiedDate.HasValue) ? ModifiedDate.Value.ToString("dd.MM.yyyy hh:ss") : "";
            }
        }
    }
}
