using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vehicle.Business.ServiceContract;
using System;
using Vehicle.ViewModel.Vehicle;
using Microsoft.AspNetCore.Authorization;

namespace Vehicle.Web.Controllers
{
    [Authorize]
    public class VehicleController : Controller
    {
        private readonly IVehicleService _vehicleService;
        private readonly ITypeOfVehicleService _typeService;
        private readonly IVehicleModelService _modelService;
        private readonly IVehicleBrandService _brandService;

        public VehicleController(IVehicleService vehicleService, ITypeOfVehicleService typeService, IVehicleModelService modelService, IVehicleBrandService brandService)
        {
            _vehicleService = vehicleService;
            _typeService = typeService;
            _modelService = modelService;
            _brandService = brandService;
        }

        // GET: Vehicle
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["CurrentFilter"] = searchString ?? currentFilter;
            ViewData["PlateOfVehicleSortParm"] = String.IsNullOrEmpty(sortOrder) ? "PlateOfVehicle_desc" : "";
            ViewData["NicknameOfVehicleSortParm"] = String.IsNullOrEmpty(sortOrder) ? "NicknameOfVehicle_desc" : "";
            ViewData["ModelNameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "ModelName_desc" : "";

            return View(await _vehicleService.GetRecords(sortOrder, currentFilter, searchString, page));
        }

        // GET: Vehicle/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await _vehicleService.GetRecordInfo(id.Value);
            if (vehicle == null)
            {
                return NotFound();
            }

            return View(vehicle);
        }

        // GET: Vehicle/Create
        public async Task<IActionResult> Create()
        {
            var vehicle = new VehicleEditViewModel
            {
                TypeOfVehicleList = await _typeService.GetRecordsForDropdown(),
                VehicleBrandList = await _brandService.GetRecordsForDropdown()
            };
            return View(vehicle);
        }

        // POST: Vehicle/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VehicleEditViewModel vehicle)
        {
            var paletControl = await _vehicleService.GetVehicleForPaletNumber(vehicle.PlateOfVehicle);
            if (paletControl != null)
            {
                ModelState.AddModelError("PaletNumber", "This Vehicle Plate number already exists");
                vehicle.TypeOfVehicleList = await _typeService.GetRecordsForDropdown();
                vehicle.VehicleBrandList = await _brandService.GetRecordsForDropdown();
            }
            if (ModelState.IsValid)
            {
                await _vehicleService.AddRecord(vehicle);
                return RedirectToAction(nameof(Index));
            }
            return View(vehicle);
        }

        // GET: Vehicle/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await _vehicleService.GetEditRecordInfo(id.Value);
            if (vehicle == null)
            {
                return NotFound();
            }
            return View(vehicle);
        }

        // POST: Vehicle/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, VehicleEditViewModel vehicle)
        {
            if (vehicle.VehicleID.HasValue == false || id != vehicle.VehicleID)
            {
                return NotFound();
            }
            var paletControl = await _vehicleService.GetVehicleForPaletNumber(vehicle.PlateOfVehicle, id);
            if (paletControl != null)
            {
                ModelState.AddModelError("PaletNumber", "This Vehicle Plate number already exists");
                vehicle = await _vehicleService.GetEditRecordInfo(id);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _vehicleService.UpdateRecord(vehicle);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VehicleExists(vehicle.VehicleID.Value))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(vehicle);
        }

        // GET: Vehicle/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await _vehicleService.GetRecordInfo(id.Value);
            if (vehicle == null)
            {
                return NotFound();
            }

            return View(vehicle);
        }

        // POST: Vehicle/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _vehicleService.DeleteRecord(id);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> GetModelOfBrand(int id)
        {
            var modelList = await _modelService.GetRecordsForDropdown(null, id);
            return Ok(modelList);
        }
        private bool VehicleExists(int id)
        {
            return (_vehicleService.GetRecordInfo(id) != null) ? true : false;
        }
    }
}
