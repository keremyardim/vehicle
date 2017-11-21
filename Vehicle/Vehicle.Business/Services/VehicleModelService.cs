using System;
using System.Threading.Tasks;
using Vehicle.Business.ServiceContract;
using Vehicle.Business.Utils;
using Vehicle.DataAccess;
using Vehicle.ViewModel.VehicleModel;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Vehicle.ViewModel.VehicleBrand;
using System.Collections.Generic;
using Vehicle.Infrastracture;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Vehicle.Business.Services
{
    public class VehicleModelService : IVehicleModelService
    {
        private readonly VehicleDbContext _context;
        private readonly IVehicleBrandService _brandService;
        public VehicleModelService(VehicleDbContext context, IVehicleBrandService brandService)
        {
            _context = context;
            _brandService = brandService;
        }
        public async Task AddRecord(VehicleModelEditViewModel NewRecord)
        {
            var vehicleModel = new DomainModel.VehicleModel
            {
                VehicleModelName = NewRecord.VehicleModelName,
                VehicleBrandID = NewRecord.VehicleBrandID,
                State = NewRecord.State.Value
            };
            await _context.VehicleModel.AddAsync(vehicleModel);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteRecord(int RecordId)
        {
            var vehicleModel = _context.VehicleModel.First(f => f.VehicleModelID == RecordId);
            _context.VehicleModel.Remove(vehicleModel);
            await _context.SaveChangesAsync();
        }

        public async Task<VehicleModelEditViewModel> GetEditRecordInfo(int RecordId)
        {
            var model = await _context.VehicleModel.Where(f => f.VehicleModelID == RecordId).Select(f =>
                           new VehicleModelEditViewModel
                           {
                               CreationDate = f.CreationDate,
                               ModifiedDate = f.ModifiedDate,
                               State = f.State,
                               VehicleModelName = f.VehicleModelName,
                               VehicleModelID = f.VehicleModelID,
                               VehicleBrandID = f.VehicleBrandID
                           }
                        ).FirstOrDefaultAsync();
            model.VehicleBrandList = await _brandService.GetRecordsForDropdown(model.VehicleBrandID);
            return model;
        }

        public async Task<VehicleModelViewModel> GetRecordInfo(int RecordId)
        {
            return await
                 _context.VehicleModel.Where(f => f.VehicleModelID == RecordId).Select(f =>
                     new VehicleModelViewModel
                     {
                         CreationDate = f.CreationDate,
                         ModifiedDate = f.ModifiedDate,
                         State = f.State,
                         VehicleModelID = f.VehicleModelID,
                         VehicleModelName = f.VehicleModelName,
                         VehicleBrandName = f.VehicleBrand.VehicleBrandName,
                         VehicleBrandID = f.VehicleBrand.VehicleBrandID
                     }
                 ).FirstOrDefaultAsync();

        }
        public async Task<PaginatedList<VehicleModelViewModel>> GetRecords(string sortOrder, string currentFilter, string searchString, int? page)
        {
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            var vehicleModel = _context.VehicleModel.AsQueryable();

            if (!String.IsNullOrEmpty(searchString))
            {
                vehicleModel = vehicleModel.Where(s => s.VehicleModelName.Contains(searchString)|| s.VehicleBrand.VehicleBrandName.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "ModelName_desc":
                    vehicleModel = vehicleModel.OrderByDescending(s => s.VehicleModelName);
                    break;
                case "BrandName_desc":
                    vehicleModel = vehicleModel.OrderByDescending(s => s.VehicleBrand.VehicleBrandName);
                    break;
                default:
                    vehicleModel = vehicleModel.OrderByDescending(s => s.CreationDate);
                    break;
            }
            var listModel = vehicleModel.Select(f => new VehicleModelViewModel
            {
                CreationDate = f.CreationDate,
                ModifiedDate = f.ModifiedDate,
                State = f.State,
                VehicleModelID = f.VehicleModelID,
                VehicleModelName = f.VehicleModelName,
                VehicleBrandID = f.VehicleBrandID,
                VehicleBrandName = f.VehicleBrand.VehicleBrandName
            }).AsNoTracking();

            int pageSize = 10;
            return await PaginatedList<VehicleModelViewModel>.CreateAsync(listModel, page ?? 1, pageSize);
        }
        public async Task<List<VehicleModelViewModel>> GetRecords()
        {

            var vehicleBrand = _context.VehicleModel.AsQueryable();

            var listModel = vehicleBrand.Select(f => new VehicleModelViewModel
            {
                CreationDate = f.CreationDate,
                ModifiedDate = f.ModifiedDate,
                State = f.State,
                VehicleModelID = f.VehicleModelID,
                VehicleModelName = f.VehicleModelName,
                VehicleBrandID = f.VehicleBrand.VehicleBrandID
            }).AsNoTracking();

            return await listModel.ToListAsync();
        }
        public async Task<List<SelectListItem>> GetRecordsForDropdown(int? SelectedID = null)
        {
            var vehicleModel = _context.VehicleModel.Where(f => f.State == Convert.ToInt16(StateEnum.Active)).AsQueryable();


            var listModel = await vehicleModel.Select(f => new SelectListItem
            {
                Text = f.VehicleModelName,
                Value = f.VehicleModelID.ToString(),
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
        public async Task<List<SelectListItem>> GetRecordsForDropdown(int? SelectedID = null, int? BrandID = null)
        {
            var vehicleModel = _context.VehicleModel.Where(f => f.State == Convert.ToInt16(StateEnum.Active)).AsQueryable();
            if (BrandID.HasValue)
            {
                vehicleModel = vehicleModel.Where(f => f.VehicleBrand.VehicleBrandID == BrandID);
            }

            var listModel = await vehicleModel.Select(f => new SelectListItem
            {
                Text = f.VehicleModelName,
                Value = f.VehicleModelID.ToString(),
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
        public async Task UpdateRecord(VehicleModelEditViewModel UpdateRecord)
        {
            var vehicleModel = await _context.VehicleModel.FirstOrDefaultAsync(f => f.VehicleModelID == UpdateRecord.VehicleModelID);

            vehicleModel.State = UpdateRecord.State.Value;
            vehicleModel.VehicleBrandID = UpdateRecord.VehicleBrandID;
            vehicleModel.VehicleModelName = UpdateRecord.VehicleModelName;

            _context.VehicleModel.Update(vehicleModel);
            await _context.SaveChangesAsync();
        }


    }
}
