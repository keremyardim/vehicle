using System;
using System.Collections.Generic;
using Vehicle.Business.ServiceContract;
using Vehicle.DataAccess;
using System.Linq;
using System.Threading.Tasks;
using Vehicle.Business.Utils;
using Microsoft.EntityFrameworkCore;
using Vehicle.ViewModel.Vehicle;
using Microsoft.AspNetCore.Mvc.Rendering;
using Vehicle.Infrastracture;

namespace Vehicle.Business.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly VehicleDbContext _context;
        private readonly IVehicleModelService _modelService;
        private readonly IVehicleBrandService _brandService;
        private readonly ITypeOfVehicleService _typeService;
        public VehicleService(VehicleDbContext context, IVehicleBrandService brandService, ITypeOfVehicleService typeService, IVehicleModelService modelService)
        {
            _context = context;
            _brandService = brandService;
            _typeService = typeService;
            _modelService = modelService;
        }

        public async Task AddRecord(ViewModel.Vehicle.VehicleEditViewModel NewRecord)
        {
                var vehicle = new DomainModel.Vehicle
                {
                    ColorOfVehicle = NewRecord.ColorOfVehicle,
                    ModelYearOfVehicle = NewRecord.ModelYearOfVehicle,
                    NicknameOfVehicle = NewRecord.NicknameOfVehicle,
                    PlateOfVehicle = NewRecord.PlateOfVehicle,
                    State = NewRecord.State.Value,
                    TypeOfVehicleID = NewRecord.TypeOfVehicleID,
                    VehicleModelID = NewRecord.VehicleModelID,
                };
                await _context.Vehicle.AddAsync(vehicle);
                await _context.SaveChangesAsync();
        }
        public async Task DeleteRecord(int RecordId)
        {
            var vehicle = _context.Vehicle.First(f => f.VehicleID == RecordId);
            _context.Vehicle.Remove(vehicle);
            await _context.SaveChangesAsync();
        }

        public async Task<VehicleEditViewModel> GetEditRecordInfo(int RecordId)
        {
            var model = await _context.Vehicle.Where(f => f.VehicleID == RecordId).Select(f =>
                           new VehicleEditViewModel
                           {
                               CreationDate = f.CreationDate,
                               ModifiedDate = f.ModifiedDate,
                               State = f.State,
                               ColorOfVehicle = f.ColorOfVehicle,
                               ModelYearOfVehicle = f.ModelYearOfVehicle,
                               NicknameOfVehicle = f.NicknameOfVehicle,
                               PlateOfVehicle = f.PlateOfVehicle,
                               TypeOfVehicleID = f.TypeOfVehicle.TypeOfVehicleID,
                               VehicleID = f.VehicleID,
                               VehicleModelID = f.VehicleModel.VehicleModelID,
                               VehicleBrandID = f.VehicleModel.VehicleBrand.VehicleBrandID
                           }
                        ).FirstOrDefaultAsync();
            model.TypeOfVehicleList = await _typeService.GetRecordsForDropdown(model.TypeOfVehicleID);
            model.VehicleBrandList = await _brandService.GetRecordsForDropdown(model.VehicleBrandID);
            model.VehicleModelList = await _modelService.GetRecordsForDropdown(model.VehicleModelID, model.VehicleBrandID);
            return model;
        }
        public async Task<VehicleViewModel> GetRecordInfo(int RecordId)
        {
            var model = await _context.Vehicle.Where(f => f.VehicleID == RecordId).Select(f =>
                           new VehicleViewModel
                           {
                               CreationDate = f.CreationDate,
                               ModifiedDate = f.ModifiedDate,
                               State = f.State,
                               ColorOfVehicle = f.ColorOfVehicle,
                               ModelYearOfVehicle = f.ModelYearOfVehicle,
                               NicknameOfVehicle = f.NicknameOfVehicle,
                               PlateOfVehicle = f.PlateOfVehicle,
                               TypeOfVehicleName = f.TypeOfVehicle.TypeName,
                               VehicleID = f.VehicleID.Value,
                               VehicleModelName = f.VehicleModel.VehicleModelName,
                               VehicleBrandName = f.VehicleModel.VehicleBrand.VehicleBrandName
                           }
                        ).FirstOrDefaultAsync();

            return model;
        }
        public async Task<PaginatedList<VehicleViewModel>> GetRecords(string sortOrder, string currentFilter, string searchString, int? page)
        {

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            var vehicle = _context.Vehicle.AsQueryable();

            if (!String.IsNullOrEmpty(searchString))
            {
                vehicle = vehicle.Where(s => s.NicknameOfVehicle.Contains(searchString)
                                       || s.PlateOfVehicle.Contains(searchString)
                                       || s.ModelYearOfVehicle.ToString().Contains(searchString)
                                       || s.VehicleModel.VehicleModelName.Contains(searchString)
                                       || s.TypeOfVehicle.TypeName.Contains(searchString)
                                       );
            }
            switch (sortOrder)
            {
                case "PlateOfVehicle_desc":
                    vehicle = vehicle.OrderByDescending(s => s.PlateOfVehicle);
                    break;
                case "NicknameOfVehicle_desc":
                    vehicle = vehicle.OrderByDescending(s => s.NicknameOfVehicle);
                    break;
                case "ModelName_desc":
                    vehicle = vehicle.OrderByDescending(s => s.VehicleModel.VehicleModelName);
                    break;
                default:
                    vehicle = vehicle.OrderByDescending(s => s.CreationDate);
                    break;
            }
            var listModel = vehicle.Select(f => new VehicleViewModel
            {
                CreationDate = f.CreationDate,
                ModifiedDate = f.ModifiedDate,
                State = f.State,
                ColorOfVehicle = f.ColorOfVehicle,
                ModelYearOfVehicle = f.ModelYearOfVehicle,
                NicknameOfVehicle = f.NicknameOfVehicle,
                PlateOfVehicle = f.PlateOfVehicle,
                VehicleModelName = f.VehicleModel.VehicleModelName,
                TypeOfVehicleName = f.TypeOfVehicle.TypeName,
                VehicleID = f.VehicleID.Value,
            }).AsNoTracking();

            int pageSize = 10;
            return await PaginatedList<VehicleViewModel>.CreateAsync(listModel, page ?? 1, pageSize);
        }
        public async Task<List<VehicleViewModel>> GetRecords()
        {
            var vehicle = _context.Vehicle.AsQueryable();
            var listModel = vehicle.Select(f => new VehicleViewModel
            {
                CreationDate = f.CreationDate,
                ModifiedDate = f.ModifiedDate,
                State = f.State,
                ColorOfVehicle = f.ColorOfVehicle,
                ModelYearOfVehicle = f.ModelYearOfVehicle,
                NicknameOfVehicle = f.NicknameOfVehicle,
                PlateOfVehicle = f.PlateOfVehicle,
                VehicleModelName = f.VehicleModel.VehicleModelName,
                TypeOfVehicleName = f.TypeOfVehicle.TypeName,
                VehicleID = f.VehicleID.Value,
            }).AsNoTracking();

            return await listModel.ToListAsync();
        }
        public async Task<VehicleViewModel> GetVehicleForPaletNumber(string PaletNumber, int? VehicleID = null)
        {
            var model = await _context.Vehicle.Where(f => f.PlateOfVehicle == PaletNumber && (!VehicleID.HasValue || f.VehicleID != VehicleID)).Select(f =>
                           new VehicleViewModel
                           {
                               CreationDate = f.CreationDate,
                               ModifiedDate = f.ModifiedDate,
                               State = f.State,
                               ColorOfVehicle = f.ColorOfVehicle,
                               ModelYearOfVehicle = f.ModelYearOfVehicle,
                               NicknameOfVehicle = f.NicknameOfVehicle,
                               PlateOfVehicle = f.PlateOfVehicle,
                               TypeOfVehicleName = f.TypeOfVehicle.TypeName,
                               VehicleID = f.VehicleID.Value,
                               VehicleModelName = f.VehicleModel.VehicleModelName,
                               VehicleBrandName = f.VehicleModel.VehicleBrand.VehicleBrandName
                           }
                        ).FirstOrDefaultAsync();
            return model;
        }
        public async Task<List<SelectListItem>> GetRecordsForDropdown(int? SelectedID = null)
        {
            var vehicle = _context.Vehicle.Where(f => f.State == Convert.ToInt16(StateEnum.Active)).AsQueryable();

            var listModel = await vehicle.Select(f => new SelectListItem
            {
                Text = f.PlateOfVehicle + " | " + f.NicknameOfVehicle,
                Value = f.VehicleID.ToString(),
                Selected = (SelectedID.HasValue && SelectedID.Value == SelectedID) ? true : false
            }).ToListAsync();

            listModel.Insert(0, new SelectListItem
            {
                Selected = (listModel.Any(f => f.Selected) == false),
                Value = "",
                Text = "Select Options"
            });

            return listModel;
        }
        public async Task UpdateRecord(VehicleEditViewModel UpdateRecord)
        {
            var vehicle = await _context.Vehicle.FirstOrDefaultAsync(f => f.VehicleID == UpdateRecord.VehicleID);

            vehicle.ColorOfVehicle = UpdateRecord.ColorOfVehicle;
            vehicle.ModelYearOfVehicle = UpdateRecord.ModelYearOfVehicle;
            vehicle.NicknameOfVehicle = UpdateRecord.NicknameOfVehicle;
            vehicle.PlateOfVehicle = UpdateRecord.PlateOfVehicle;
            vehicle.State = UpdateRecord.State.Value;
            vehicle.VehicleModelID = UpdateRecord.VehicleModelID;
            vehicle.TypeOfVehicleID = UpdateRecord.TypeOfVehicleID;

            _context.Vehicle.Update(vehicle);
            await _context.SaveChangesAsync();
        }

    }
}
