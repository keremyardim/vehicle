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
using Vehicle.ViewModel.TypeOfVehicle;

namespace Vehicle.Web.Controllers
{
    public class TypeOfVehiclesController : Controller
    {
        private readonly ITypeOfVehicleService _typeService;

        public TypeOfVehiclesController(ITypeOfVehicleService typeService)
        {
            _typeService = typeService;
        }

        // GET: TypeOfVehicles
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["CurrentFilter"] = searchString ?? currentFilter;
            ViewData["TypeNameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "TypeName_desc" : "";

            return View(await _typeService.GetRecords(sortOrder, currentFilter, searchString, page));
        }

        // GET: TypeOfVehicles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicletype = await _typeService.GetRecordInfo(id.Value);
            if (vehicletype == null)
            {
                return NotFound();
            }

            return View(vehicletype);
        }

        // GET: TypeOfVehicles/Create
        public IActionResult Create()
        {
            TypeOfVehicleEditViewModel model = new TypeOfVehicleEditViewModel();
            return View(model);
        }

        // POST: TypeOfVehicles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TypeOfVehicleEditViewModel vehicletype)
        {
            if (ModelState.IsValid)
            {
                await _typeService.AddRecord(vehicletype);
                return RedirectToAction(nameof(Index));
            }
            return View(vehicletype);
        }

        // GET: TypeOfVehicles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicletype = await _typeService.GetEditRecordInfo(id.Value);
            if (vehicletype == null)
            {
                return NotFound();
            }
            return View(vehicletype);
        }

        // POST: TypeOfVehicles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TypeOfVehicleEditViewModel VehicleType)
        {
            if (id != VehicleType.TypeOfVehicleID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _typeService.UpdateRecord(VehicleType);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VehicletypeExists(VehicleType.TypeOfVehicleID))
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
            return View(VehicleType);
        }

        // GET: TypeOfVehicles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicletype = await _typeService.GetRecordInfo(id.Value);
            if (vehicletype == null)
            {
                return NotFound();
            }

            return View(vehicletype);
        }

        // POST: TypeOfVehicles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _typeService.DeleteRecord(id);
            return RedirectToAction(nameof(Index));
        }

        private bool VehicletypeExists(int id)
        {
            return (_typeService.GetRecordInfo(id) != null) ? true : false;
        }
    }
}
