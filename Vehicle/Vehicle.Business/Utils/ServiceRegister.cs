﻿using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Vehicle.Business.ServiceContract;
using Vehicle.Business.Services;

namespace Vehicle.Business.Utils
{
    public static class ServiceRegister
    {
        public static void RegisterDomainService(this IServiceCollection service)
        {
            service.AddTransient<ITypeOfVehicleService, TypeOfVehicleService>();
            service.AddTransient<IVehicleService, VehicleService>();
            service.AddTransient<IVehicleBrandService, VehicleBrandService>();
            service.AddTransient<IVehicleModelService, VehicleModelService>();
        }
    }
}
