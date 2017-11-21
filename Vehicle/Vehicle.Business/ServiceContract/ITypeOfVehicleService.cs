using System;
using System.Collections.Generic;
using System.Text;
using Vehicle.Business.Utils;

namespace Vehicle.Business.ServiceContract
{
    public interface ITypeOfVehicleService : 
        IBaseContract<ViewModel.TypeOfVehicle.TypeOfVehicleViewModel, ViewModel.TypeOfVehicle.TypeOfVehicleEditViewModel, Int32>
    {
        //Add spesific contract from ITypeOfVehicleService
    }
}
