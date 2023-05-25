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
    public class InstitutionAreasController : Controller
    {
        private readonly IxchelWebpruebasContext _context;

        public InstitutionAreasController(IxchelWebpruebasContext context)
        {
            _context = context;
        }

        // GET: InstitutionAreas
        //public async Task<IActionResult> Index()
        //{
        //    var ixchelWebpruebasContext = _context.InstitutionAreas.Include(i => i.IdinstitutionCountryNavigation).Include(i => i.IdinstitutionNavigation);
        //    return View(await ixchelWebpruebasContext.Where(i => i.Deleted == false).ToListAsync());
        //}

        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["CurrentFilter"] = searchString;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["IdSortParm"] = sortOrder == "Id" ? "id_desc" : "Id";
            ViewData["EnabledSortParm"] = sortOrder == "Enabled" ? "enabled_desc" : "Enabled";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            IQueryable<InstitutionArea> areas = _context.InstitutionAreas.Where(i => i.Deleted == false);

            //if (!String.IsNullOrEmpty(searchString))
            //{
            //    //institutions = institutions.Where(i => i.InstitutionName.Contains(searchString)).ToList();
            //    areas = areas.Where(i => i.AreaName.Contains(searchString)
            //                           && i.Deleted == false);
            //}

            if (!String.IsNullOrEmpty(searchString))
            {
                areas = areas.Where(i =>
                    i.AreaName.Contains(searchString) ||
                    i.IdinstitutionArea.ToString().Contains(searchString) ||
                    i.AreaCode.Contains(searchString) &&
                    i.Deleted == false);
            }

            switch (sortOrder)
            {
                case "name_desc":
                    areas = areas.OrderByDescending(i => i.AreaName);
                    break;
                case "Id":
                    areas = areas.OrderBy(i => i.IdinstitutionArea);
                    break;
                case "id_desc":
                    areas = areas.OrderByDescending(i => i.IdinstitutionArea);
                    break;
                case "Enabled":
                    areas = areas.OrderBy(i => i.Enabled);
                    break;
                default:
                    areas = areas.OrderBy(i => i.AreaName);
                    break;
            }

            int pageSize = 5;
            var pagedInstitutionArea = await PaginatedList<InstitutionArea>.CreateAsync(areas, pageNumber ?? 1, pageSize);
            return View(pagedInstitutionArea);
        }

        // GET: InstitutionAreas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.InstitutionAreas == null)
            {
                return NotFound();
            }

            var institutionArea = await _context.InstitutionAreas
                .Include(i => i.IdinstitutionCountryNavigation)
                .Include(i => i.IdinstitutionNavigation)
                .FirstOrDefaultAsync(m => m.IdinstitutionArea == id);
            if (institutionArea == null)
            {
                return NotFound();
            }

            return View(institutionArea);
        }

        // GET: InstitutionAreas/Create
        public IActionResult Create()
        {
            ViewData["IdinstitutionCountry"] = new SelectList(_context.InstitutionCountries, "IdinstitutionCountry", "CountryName");
            ViewData["Idinstitution"] = new SelectList(_context.Institutions, "Idinstitution", "InstitutionName");
            return View();
        }

        // POST: InstitutionAreas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdinstitutionArea,Idinstitution,IdinstitutionCountry,AreaCode,AreaName,Enabled,Deleted")] InstitutionArea institutionArea)
        {
            if (ModelState.IsValid)
            {
                _context.Add(institutionArea);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdinstitutionCountry"] = new SelectList(_context.InstitutionCountries, "IdinstitutionCountry", "IdinstitutionCountry", institutionArea.IdinstitutionCountry);
            ViewData["Idinstitution"] = new SelectList(_context.Institutions, "Idinstitution", "Idinstitution", institutionArea.Idinstitution);
            return View(institutionArea);
        }

        // GET: InstitutionAreas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.InstitutionAreas == null)
            {
                return NotFound();
            }

            var institutionArea = await _context.InstitutionAreas.FindAsync(id);
            if (institutionArea == null)
            {
                return NotFound();
            }
            ViewData["IdinstitutionCountry"] = new SelectList(_context.InstitutionCountries, "IdinstitutionCountry", "IdinstitutionCountry", institutionArea.IdinstitutionCountry);
            ViewData["Idinstitution"] = new SelectList(_context.Institutions, "Idinstitution", "Idinstitution", institutionArea.Idinstitution);
            return View(institutionArea);
        }

        // POST: InstitutionAreas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdinstitutionArea,Idinstitution,IdinstitutionCountry,AreaCode,AreaName,Enabled,Deleted")] InstitutionArea institutionArea)
        {
            if (id != institutionArea.IdinstitutionArea)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(institutionArea);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InstitutionAreaExists(institutionArea.IdinstitutionArea))
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
            ViewData["IdinstitutionCountry"] = new SelectList(_context.InstitutionCountries, "IdinstitutionCountry", "IdinstitutionCountry", institutionArea.IdinstitutionCountry);
            ViewData["Idinstitution"] = new SelectList(_context.Institutions, "Idinstitution", "Idinstitution", institutionArea.Idinstitution);
            return View(institutionArea);
        }

        // GET: InstitutionAreas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.InstitutionAreas == null)
            {
                return NotFound();
            }

            var institutionArea = await _context.InstitutionAreas
                .Include(i => i.IdinstitutionCountryNavigation)
                .Include(i => i.IdinstitutionNavigation)
                .FirstOrDefaultAsync(m => m.IdinstitutionArea == id);
            if (institutionArea == null)
            {
                return NotFound();
            }

            return View(institutionArea);
        }

        // POST: InstitutionAreas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.InstitutionAreas == null)
            {
                return Problem("Entity set 'IxchelWebpruebasContext.InstitutionAreas'  is null.");
            }
            var institutionArea = await _context.InstitutionAreas.FindAsync(id);
            if (institutionArea != null)
            {
                _context.InstitutionAreas.Remove(institutionArea);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InstitutionAreaExists(int id)
        {
          return (_context.InstitutionAreas?.Any(e => e.IdinstitutionArea == id)).GetValueOrDefault();
        }
    }
}
