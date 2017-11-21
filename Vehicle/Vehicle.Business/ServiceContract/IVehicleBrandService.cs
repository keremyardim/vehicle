using System;
using System.Collections.Generic;
using System.Text;
using Vehicle.Business.Utils;

namespace Vehicle.Business.ServiceContract
{
    public interface IVehicleBrandService : IBaseContract<ViewModel.VehicleBrand.VehicleBrandViewModel, ViewModel.VehicleBrand.VehicleBrandEditViewModel, Int32>
    {
    }
}
