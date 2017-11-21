using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Vehicle.DataAccess;
using Vehicle.DomainModel;
using Vehicle.Business.ServiceContract;
using Vehicle.ViewModel.VehicleModel;

namespace Vehicle.Web.Controllers
{
    public class VehicleModelsController : Controller
    {
        private readonly IVehicleModelService _modelService;
        private readonly IVehicleBrandService _brandService;

        public VehicleModelsController(IVehicleModelService modelService, IVehicleBrandService brandService)
        {
            _modelService = modelService;
            _brandService = brandService;
        }

        // GET: VehicleModels
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["CurrentFilter"] = searchString ?? currentFilter;
            ViewData["BrandNameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "BrandName_desc" : "";
            ViewData["ModelNameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "ModelName_desc" : "";

            return View(await _modelService.GetRecords(sortOrder, currentFilter, searchString, page));
        }

        // GET: VehicleModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicleBrand = await _modelService.GetRecordInfo(id.Value);
            if (vehicleBrand == null)
            {
                return NotFound();
            }

            return View(vehicleBrand);
        }

        // GET: VehicleModels/Create
        public async Task<IActionResult> Create()
        {
            VehicleModelEditViewModel model = new VehicleModelEditViewModel();
            model.VehicleBrandList = await _brandService.GetRecordsForDropdown();
            return View(model);
        }

        // POST: VehicleModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VehicleModelEditViewModel vehicleModel)
        {
            if (ModelState.IsValid)
            {
                await _modelService.AddRecord(vehicleModel);
                return RedirectToAction(nameof(Index));
            }
            return View(vehicleModel);
        }

        // GET: VehicleModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicleModel = await _modelService.GetEditRecordInfo(id.Value);
            if (vehicleModel == null)
            {
                return NotFound();
            }
            return View(vehicleModel);
        }

        // POST: VehicleModels/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, VehicleModelEditViewModel vehicleModel)
        {
            if (id != vehicleModel.VehicleModelID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _modelService.UpdateRecord(vehicleModel);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VehicleModelExists(vehicleModel.VehicleModelID))
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
            return View(vehicleModel);
        }

        // GET: VehicleModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicleModel = await _modelService.GetRecordInfo(id.Value);
            if (vehicleModel == null)
            {
                return NotFound();
            }

            return View(vehicleModel);
        }

        // POST: VehicleModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _modelService.DeleteRecord(id);
            return RedirectToAction(nameof(Index));
        }

        private bool VehicleModelExists(int id)
        {
            return (_modelService.GetRecordInfo(id) != null) ? true : false;
        }
    }
}
