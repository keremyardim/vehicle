using System;
using System.Threading.Tasks;
using Vehicle.Business.ServiceContract;
using Vehicle.Business.Utils;
using Vehicle.DataAccess;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Vehicle.ViewModel.TypeOfVehicle;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Vehicle.Infrastracture;

namespace Vehicle.Business.Services
{
    public class TypeOfVehicleService : ITypeOfVehicleService
    {
        private readonly VehicleDbContext _context;
        public TypeOfVehicleService(VehicleDbContext context)
        {
            _context = context;
        }
        public async Task AddRecord(TypeOfVehicleEditViewModel NewRecord)
        {
            var vehicleType = new DomainModel.TypeOfVehicle
            {
                TypeName = NewRecord.TypeName,
                State = NewRecord.State.Value
            };
            await _context.TypeOfVehicle.AddAsync(vehicleType);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteRecord(int RecordId)
        {
            var vehicleType = _context.TypeOfVehicle.First(f => f.TypeOfVehicleID == RecordId);
            _context.TypeOfVehicle.Remove(vehicleType);
            await _context.SaveChangesAsync();
        }

        public async Task<TypeOfVehicleEditViewModel> GetEditRecordInfo(int RecordId)
        {
            var model = await _context.TypeOfVehicle.Where(f => f.TypeOfVehicleID == RecordId).Select(f =>
                           new TypeOfVehicleEditViewModel
                           {
                               CreationDate = f.CreationDate,
                               ModifiedDate = f.ModifiedDate,
                               State = f.State,
                               TypeOfVehicleID = f.TypeOfVehicleID,
                               TypeName = f.TypeName
                           }
                        ).FirstOrDefaultAsync();
            return model;
        }

        public async Task<TypeOfVehicleViewModel> GetRecordInfo(int RecordId)
        {
            return await
                 _context.TypeOfVehicle.Where(f => f.TypeOfVehicleID == RecordId).Select(f =>
                     new TypeOfVehicleViewModel
                     {
                         CreationDate = f.CreationDate,
                         ModifiedDate = f.ModifiedDate,
                         State = f.State,
                         TypeName = f.TypeName,
                         TypeOfVehicleID = f.TypeOfVehicleID
                     }
                 ).FirstOrDefaultAsync();
        }
        public async Task<PaginatedList<TypeOfVehicleViewModel>> GetRecords(string sortOrder, string currentFilter, string searchString, int? page)
        {
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            var vehicleType = _context.TypeOfVehicle.AsQueryable();

            if (!String.IsNullOrEmpty(searchString))
            {
                vehicleType = vehicleType.Where(s => s.TypeName.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "TypeName_desc":
                    vehicleType = vehicleType.OrderByDescending(s => s.TypeName);
                    break;
                default:
                    vehicleType = vehicleType.OrderByDescending(s => s.CreationDate);
                    break;
            }
            var listModel = vehicleType.Select(f => new TypeOfVehicleViewModel
            {
                CreationDate = f.CreationDate,
                ModifiedDate = f.ModifiedDate,
                State = f.State,
                TypeName = f.TypeName,
                TypeOfVehicleID = f.TypeOfVehicleID
            }).AsNoTracking();

            int pageSize = 10;
            return await PaginatedList<TypeOfVehicleViewModel>.CreateAsync(listModel, page ?? 1, pageSize);

        }
        public async Task<List<TypeOfVehicleViewModel>> GetRecords()
        {

            var vehicleType = _context.TypeOfVehicle.AsQueryable();

            var listModel = vehicleType.Select(f => new TypeOfVehicleViewModel
            {
                CreationDate = f.CreationDate,
                ModifiedDate = f.ModifiedDate,
                State = f.State,
                TypeName = f.TypeName,
                TypeOfVehicleID = f.TypeOfVehicleID
            }).AsNoTracking();

            return await listModel.ToListAsync();

        }
        public async Task<List<SelectListItem>> GetRecordsForDropdown(int? SelectedID = null)
        {
            try
            {
                var vehicleType = await _context.TypeOfVehicle.Where(f => f.State == Convert.ToInt16(StateEnum.Active)).ToListAsync();
                var listModel = vehicleType.Select(f => new SelectListItem
                {
                    Text = f.TypeName,
                    Value = f.TypeOfVehicleID.ToString(),
                    Selected = (SelectedID.HasValue && SelectedID.Value == SelectedID) ? true : false
                }).ToList();

                listModel.Insert(0, new SelectListItem
                {
                    Selected = (listModel.Any(f => f.Selected) == false),
                    Value = "",
                    Text = "Select Options"
                });

                return listModel;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public async Task UpdateRecord(TypeOfVehicleEditViewModel UpdateRecord)
        {
            var vehicleType = await _context.TypeOfVehicle.FirstOrDefaultAsync(f => f.TypeOfVehicleID == UpdateRecord.TypeOfVehicleID);

            vehicleType.TypeName = UpdateRecord.TypeName;
            vehicleType.State = UpdateRecord.State.Value;
            _context.TypeOfVehicle.Update(vehicleType);
            await _context.SaveChangesAsync();
        }
    }
}
