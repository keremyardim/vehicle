using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Vehicle.Business.Utils;

namespace Vehicle.Business.ServiceContract
{
    public interface IVehicleModelService : IBaseContract<ViewModel.VehicleModel.VehicleModelViewModel, ViewModel.VehicleModel.VehicleModelEditViewModel, Int32>
    {
        //Add spesific contract from IVehicleModelService
        Task<List<SelectListItem>> GetRecordsForDropdown(int? SelectedID = null, int? BrandID = null);
    }
}
