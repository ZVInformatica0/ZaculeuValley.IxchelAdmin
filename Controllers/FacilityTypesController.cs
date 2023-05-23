using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ZaculeuValley.IxchelAdmin.Models;

namespace ZaculeuValley.IxchelAdmin.Controllers
{
    public class FacilityTypesController : Controller
    {
        private readonly IxchelWebpruebasContext _context;

        public FacilityTypesController(IxchelWebpruebasContext context)
        {
            _context = context;
        }

        // GET: FacilityTypes
        public async Task<IActionResult> Index()
        {
            var ixchelWebpruebasContext = _context.FacilityTypes.Include(f => f.IdinstitutionNavigation);
            return View(await ixchelWebpruebasContext.Where(i => i.Deleted == false).ToListAsync());
        }

        // GET: FacilityTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.FacilityTypes == null)
            {
                return NotFound();
            }

            var facilityType = await _context.FacilityTypes
                .Include(f => f.IdinstitutionNavigation)
                .FirstOrDefaultAsync(m => m.IdfacilityType == id);
            if (facilityType == null)
            {
                return NotFound();
            }

            return View(facilityType);
        }

        // GET: FacilityTypes/Create
        public IActionResult Create()
        {
            ViewData["Idinstitution"] = new SelectList(_context.Institutions, "Idinstitution", "InstitutionName");
            return View();
        }

        // POST: FacilityTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdfacilityType,Idinstitution,FacilityTypeCode,FacilityTypeName,Enabled,Deleted")] FacilityType facilityType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(facilityType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Idinstitution"] = new SelectList(_context.Institutions, "Idinstitution", "Idinstitution", facilityType.Idinstitution);
            return View(facilityType);
        }

        // GET: FacilityTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.FacilityTypes == null)
            {
                return NotFound();
            }

            var facilityType = await _context.FacilityTypes.FindAsync(id);
            if (facilityType == null)
            {
                return NotFound();
            }
            ViewData["Idinstitution"] = new SelectList(_context.Institutions, "Idinstitution", "Idinstitution", facilityType.Idinstitution);
            return View(facilityType);
        }

        // POST: FacilityTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdfacilityType,Idinstitution,FacilityTypeCode,FacilityTypeName,Enabled,Deleted")] FacilityType facilityType)
        {
            if (id != facilityType.IdfacilityType)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(facilityType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FacilityTypeExists(facilityType.IdfacilityType))
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
            ViewData["Idinstitution"] = new SelectList(_context.Institutions, "Idinstitution", "Idinstitution", facilityType.Idinstitution);
            return View(facilityType);
        }

        // GET: FacilityTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.FacilityTypes == null)
            {
                return NotFound();
            }

            var facilityType = await _context.FacilityTypes
                .Include(f => f.IdinstitutionNavigation)
                .FirstOrDefaultAsync(m => m.IdfacilityType == id);
            if (facilityType == null)
            {
                return NotFound();
            }

            return View(facilityType);
        }

        // POST: FacilityTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.FacilityTypes == null)
            {
                return Problem("Entity set 'IxchelWebpruebasContext.FacilityTypes'  is null.");
            }
            var facilityType = await _context.FacilityTypes.FindAsync(id);
            if (facilityType != null)
            {
                _context.FacilityTypes.Remove(facilityType);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FacilityTypeExists(int id)
        {
          return (_context.FacilityTypes?.Any(e => e.IdfacilityType == id)).GetValueOrDefault();
        }
    }
}
