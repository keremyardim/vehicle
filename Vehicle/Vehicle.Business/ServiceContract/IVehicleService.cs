using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Vehicle.Business.Utils;
using Vehicle.ViewModel.Vehicle;

namespace Vehicle.Business.ServiceContract
{
    public interface IVehicleService : IBaseContract<ViewModel.Vehicle.VehicleViewModel, Vehicle.ViewModel.Vehicle.VehicleEditViewModel, Int32>
    {
        Task<VehicleViewModel> GetVehicleForPaletNumber(string PaletNumber, int? VehicleID = null);
    }
}
