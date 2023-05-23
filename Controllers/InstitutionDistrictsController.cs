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
    public class InstitutionDistrictsController : Controller
    {
        private readonly IxchelWebpruebasContext _context;

        public InstitutionDistrictsController(IxchelWebpruebasContext context)
        {
            _context = context;
        }

        // GET: InstitutionDistricts
        public async Task<IActionResult> Index()
        {
            var ixchelWebpruebasContext = _context.InstitutionDistricts.Include(i => i.IdinstitutionAreaNavigation).Include(i => i.IdinstitutionNavigation);
            return View(await ixchelWebpruebasContext.Where(i => i.Deleted == false).ToListAsync());
        }

        // GET: InstitutionDistricts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.InstitutionDistricts == null)
            {
                return NotFound();
            }

            var institutionDistrict = await _context.InstitutionDistricts
                .Include(i => i.IdinstitutionAreaNavigation)
                .Include(i => i.IdinstitutionNavigation)
                .FirstOrDefaultAsync(m => m.IdinstitutionDistrict == id);
            if (institutionDistrict == null)
            {
                return NotFound();
            }

            return View(institutionDistrict);
        }

        // GET: InstitutionDistricts/Create
        public IActionResult Create()
        {
            ViewData["IdinstitutionArea"] = new SelectList(_context.InstitutionAreas, "IdinstitutionArea", "AreaName");
            ViewData["Idinstitution"] = new SelectList(_context.Institutions, "Idinstitution", "InstitutionName");
            return View();
        }

        // POST: InstitutionDistricts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdinstitutionDistrict,IdinstitutionArea,Idinstitution,AreaCode,DistrictCode,DistrictName,Enabled,Deleted")] InstitutionDistrict institutionDistrict)
        {
            if (ModelState.IsValid)
            {
                _context.Add(institutionDistrict);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdinstitutionArea"] = new SelectList(_context.InstitutionAreas, "IdinstitutionArea", "IdinstitutionArea", institutionDistrict.IdinstitutionArea);
            ViewData["Idinstitution"] = new SelectList(_context.Institutions, "Idinstitution", "Idinstitution", institutionDistrict.Idinstitution);
            return View(institutionDistrict);
        }

        // GET: InstitutionDistricts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.InstitutionDistricts == null)
            {
                return NotFound();
            }

            var institutionDistrict = await _context.InstitutionDistricts.FindAsync(id);
            if (institutionDistrict == null)
            {
                return NotFound();
            }
            ViewData["IdinstitutionArea"] = new SelectList(_context.InstitutionAreas, "IdinstitutionArea", "IdinstitutionArea", institutionDistrict.IdinstitutionArea);
            ViewData["Idinstitution"] = new SelectList(_context.Institutions, "Idinstitution", "Idinstitution", institutionDistrict.Idinstitution);
            return View(institutionDistrict);
        }

        // POST: InstitutionDistricts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdinstitutionDistrict,IdinstitutionArea,Idinstitution,AreaCode,DistrictCode,DistrictName,Enabled,Deleted")] InstitutionDistrict institutionDistrict)
        {
            if (id != institutionDistrict.IdinstitutionDistrict)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(institutionDistrict);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InstitutionDistrictExists(institutionDistrict.IdinstitutionDistrict))
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
            ViewData["IdinstitutionArea"] = new SelectList(_context.InstitutionAreas, "IdinstitutionArea", "IdinstitutionArea", institutionDistrict.IdinstitutionArea);
            ViewData["Idinstitution"] = new SelectList(_context.Institutions, "Idinstitution", "Idinstitution", institutionDistrict.Idinstitution);
            return View(institutionDistrict);
        }

        // GET: InstitutionDistricts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.InstitutionDistricts == null)
            {
                return NotFound();
            }

            var institutionDistrict = await _context.InstitutionDistricts
                .Include(i => i.IdinstitutionAreaNavigation)
                .Include(i => i.IdinstitutionNavigation)
                .FirstOrDefaultAsync(m => m.IdinstitutionDistrict == id);
            if (institutionDistrict == null)
            {
                return NotFound();
            }

            return View(institutionDistrict);
        }

        // POST: InstitutionDistricts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.InstitutionDistricts == null)
            {
                return Problem("Entity set 'IxchelWebpruebasContext.InstitutionDistricts'  is null.");
            }
            var institutionDistrict = await _context.InstitutionDistricts.FindAsync(id);
            if (institutionDistrict != null)
            {
                _context.InstitutionDistricts.Remove(institutionDistrict);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InstitutionDistrictExists(int id)
        {
          return (_context.InstitutionDistricts?.Any(e => e.IdinstitutionDistrict == id)).GetValueOrDefault();
        }
    }
}
