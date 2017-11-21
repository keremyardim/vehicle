using System;
using System.Threading.Tasks;
using Vehicle.Business.ServiceContract;
using Vehicle.Business.Utils;
using Vehicle.DataAccess;
using Vehicle.ViewModel.VehicleBrand;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Vehicle.Infrastracture;

namespace Vehicle.Business.Services
{
    public class VehicleBrandService : IVehicleBrandService
    {
        private readonly VehicleDbContext _context;
        public VehicleBrandService(VehicleDbContext context)
        {
            _context = context;
        }
        public async Task AddRecord(VehicleBrandEditViewModel NewRecord)
        {
            var vehicleBrand = new DomainModel.VehicleBrand
            {
                VehicleBrandName = NewRecord.VehicleBrandName,
                State = NewRecord.State.Value
            };
            await _context.VehicleBrand.AddAsync(vehicleBrand);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteRecord(int RecordId)
        {
            var vehicleBrand = _context.VehicleBrand.First(f => f.VehicleBrandID == RecordId);
            _context.VehicleBrand.Remove(vehicleBrand);
            await _context.SaveChangesAsync();
        }
        public async Task<VehicleBrandEditViewModel> GetEditRecordInfo(int RecordId)
        {
            var model = await _context.VehicleBrand.Where(f => f.VehicleBrandID == RecordId).Select(f =>
                           new VehicleBrandEditViewModel
                           {
                               CreationDate = f.CreationDate,
                               ModifiedDate = f.ModifiedDate,
                               State = f.State,
                               VehicleBrandName = f.VehicleBrandName,
                               VehicleBrandID = f.VehicleBrandID
                           }
                        ).FirstOrDefaultAsync();
            return model;
        }
        public async Task<VehicleBrandViewModel> GetRecordInfo(int RecordId)
        {
            return await
                 _context.VehicleBrand.Where(f => f.VehicleBrandID == RecordId).Select(f =>
                     new VehicleBrandViewModel
                     {
                         CreationDate = f.CreationDate,
                         ModifiedDate = f.ModifiedDate,
                         State = f.State,
                         VehicleBrandID = f.VehicleBrandID,
                         VehicleBrandName = f.VehicleBrandName
                     }
                 ).FirstOrDefaultAsync();
        }
        public async Task<PaginatedList<VehicleBrandViewModel>> GetRecords(string sortOrder, string currentFilter, string searchString, int? page)
        {
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            var vehicleBrand = _context.VehicleBrand.AsQueryable();

            if (!String.IsNullOrEmpty(searchString))
            {
                vehicleBrand = vehicleBrand.Where(s => s.VehicleBrandName.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "BrandName_desc":
                    vehicleBrand = vehicleBrand.OrderByDescending(s => s.VehicleBrandName);
                    break;
                default:
                    vehicleBrand = vehicleBrand.OrderBy(s => s.VehicleBrandName);
                    break;
            }
            var listModel = vehicleBrand.Select(f => new VehicleBrandViewModel
            {
                CreationDate = f.CreationDate,
                ModifiedDate = f.ModifiedDate,
                State = f.State,
                VehicleBrandName = f.VehicleBrandName,
                VehicleBrandID = f.VehicleBrandID
            }).AsNoTracking();

            int pageSize = 10;
            return await PaginatedList<VehicleBrandViewModel>.CreateAsync(listModel, page ?? 1, pageSize);
        }
        public async Task<List<VehicleBrandViewModel>> GetRecords()
        {
            var vehicleType = _context.VehicleBrand.AsQueryable();

            var listModel = vehicleType.Select(f => new VehicleBrandViewModel
            {
                CreationDate = f.CreationDate,
                ModifiedDate = f.ModifiedDate,
                State = f.State,
                VehicleBrandID = f.VehicleBrandID,
                VehicleBrandName = f.VehicleBrandName
            }).AsNoTracking();

            return await listModel.ToListAsync();
        }
        public async Task<List<SelectListItem>> GetRecordsForDropdown(int? SelectedID = null)
        {
            var vehicleBrand = _context.VehicleBrand.Where(f => f.State == Convert.ToInt16(StateEnum.Active)).AsQueryable();
            var listModel = await vehicleBrand.Select(f => new SelectListItem
            {
                Text = f.VehicleBrandName,
                Value = f.VehicleBrandID.ToString(),
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
        public async Task UpdateRecord(VehicleBrandEditViewModel UpdateRecord)
        {
            var vehicleBrand = await _context.VehicleBrand.FirstOrDefaultAsync(f => f.VehicleBrandID == UpdateRecord.VehicleBrandID);

            vehicleBrand.VehicleBrandName = UpdateRecord.VehicleBrandName;
            vehicleBrand.State = UpdateRecord.State.Value;
            _context.VehicleBrand.Update(vehicleBrand);
            await _context.SaveChangesAsync();
        }


    }
}
