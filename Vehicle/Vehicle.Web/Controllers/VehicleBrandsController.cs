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
using Vehicle.ViewModel.VehicleBrand;
using Microsoft.AspNetCore.Authorization;

namespace Vehicle.Web.Controllers
{
    [Authorize]
    public class VehicleBrandsController : Controller
    {
        private readonly IVehicleBrandService _brandService;

        public VehicleBrandsController(IVehicleBrandService brandService)
        {
            _brandService = brandService;
        }

        // GET: VehicleBrands
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["CurrentFilter"] = searchString ?? currentFilter;
            ViewData["BrandNameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "BrandName_desc" : "";

            return View(await _brandService.GetRecords(sortOrder, currentFilter, searchString, page));
        }

        // GET: VehicleBrands/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicleBrand = await _brandService.GetRecordInfo(id.Value);
            if (vehicleBrand == null)
            {
                return NotFound();
            }

            return View(vehicleBrand);
        }

        // GET: VehicleBrands/Create
        public IActionResult Create()
        {
            VehicleBrandEditViewModel model = new VehicleBrandEditViewModel();
            return View(model);
        }

        // POST: VehicleBrands/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VehicleBrandEditViewModel vehicleBrand)
        {
            if (ModelState.IsValid)
            {
                await _brandService.AddRecord(vehicleBrand);
                return RedirectToAction(nameof(Index));
            }
            return View(vehicleBrand);
        }

        // GET: VehicleBrands/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicleBrand = await _brandService.GetEditRecordInfo(id.Value);
            if (vehicleBrand == null)
            {
                return NotFound();
            }
            return View(vehicleBrand);
        }

        // POST: VehicleBrands/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, VehicleBrandEditViewModel vehicleBrand)
        {
            if (id != vehicleBrand.VehicleBrandID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _brandService.UpdateRecord(vehicleBrand);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VehicleBrandExists(vehicleBrand.VehicleBrandID))
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
            return View(vehicleBrand);
        }

        // GET: VehicleBrands/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicleBrand = await _brandService.GetRecordInfo(id.Value);
            if (vehicleBrand == null)
            {
                return NotFound();
            }

            return View(vehicleBrand);
        }

        // POST: VehicleBrands/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _brandService.DeleteRecord(id);
            return RedirectToAction(nameof(Index));
        }

        private bool VehicleBrandExists(int id)
        {
            return (_brandService.GetRecordInfo(id) != null) ? true : false;
        }
    }
}
