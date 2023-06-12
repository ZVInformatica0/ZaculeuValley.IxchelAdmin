using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ZaculeuValley.IxchelAdmin.Data;
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
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["CurrentFilter"] = searchString;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["IdSortParm"] = sortOrder == "Id" ? "id_desc" : "Id";
            ViewData["EnabledSortParm"] = sortOrder == "Enabled" ? "enabled_desc" : "Enabled";
            ViewData["DistrictCodeSortParm"] = sortOrder == "DistrictCode" ? "districtcode_desc" : "DistrictCode";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            //IQueryable<InstitutionDistrict> districts = _context.InstitutionDistricts.Where(i => i.Deleted == false);

            int? institutionId = HttpContext.Session.GetInt32("InstitutionId");
            string? institutionName = HttpContext.Session.GetString("InstitutionName");
            ViewBag.InstitutionName = institutionName;
            // Use the Institution ID to filter facilities
            //IQueryable<InstitutionDistrict> districts = _context.InstitutionDistricts
            //    .Where(f => f.Idinstitution == institutionId && f.Deleted == false);

            IQueryable<InstitutionDistrict> districts = _context.InstitutionDistricts
            .Include(d => d.IdinstitutionAreaNavigation)
            .Where(d => d.Idinstitution == institutionId && d.Deleted == false);


            //if (!String.IsNullOrEmpty(searchString))
            //{
            //    //institutions = institutions.Where(i => i.InstitutionName.Contains(searchString)).ToList();
            //    districts = districts.Where(i => i.DistrictName.Contains(searchString)
            //                           && i.Deleted == false);
            //}

            if (!String.IsNullOrEmpty(searchString))
            {
                districts = districts.Where(i =>
                    i.DistrictName.Contains(searchString) ||
                    i.IdinstitutionDistrict.ToString().Contains(searchString) &&
                    i.Deleted == false);
            }

            switch (sortOrder)
            {
                case "name_desc":
                    districts = districts.OrderByDescending(i => i.DistrictName);
                    break;
                case "Id":
                    districts = districts.OrderBy(i => i.IdinstitutionAreaNavigation.AreaCode);
                    break;
                case "id_desc":
                    districts = districts.OrderByDescending(i => i.IdinstitutionAreaNavigation.AreaCode);
                    break;
                case "DistrictCode":
                    districts = districts.OrderBy(i => i.DistrictCode);
                    break;
                case "districtcode_desc":
                    districts = districts.OrderByDescending(i => i.DistrictCode);
                    break;
                case "Enabled":
                    districts = districts.OrderBy(i => i.Enabled);
                    break;
                default:
                    districts = districts.OrderBy(i => i.DistrictName);
                    break;
            }
            

            int pageSize = 5;
            var pagedInstitutionDistricts = await PaginatedList<InstitutionDistrict>.CreateAsync(districts, pageNumber ?? 1, pageSize);
            return View(pagedInstitutionDistricts);
        }

        //public async Task<IActionResult> Index()
        //{
        //    var ixchelWebpruebasContext = _context.InstitutionDistricts.Include(i => i.IdinstitutionAreaNavigation).Include(i => i.IdinstitutionNavigation);
        //    return View(await ixchelWebpruebasContext.Where(i => i.Deleted == false).ToListAsync());
        //}

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
            ViewData["Idinstitution"] = new SelectList(_context.Institutions, "Idinstitution", "InstitutionName", institutionDistrict.Idinstitution);
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
            ViewData["IdinstitutionArea"] = new SelectList(_context.InstitutionAreas, "IdinstitutionArea", "AreaName", institutionDistrict.IdinstitutionArea);
            ViewData["Idinstitution"] = new SelectList(_context.Institutions, "Idinstitution", "InstitutionName", institutionDistrict.Idinstitution);
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
            ViewData["IdinstitutionArea"] = new SelectList(_context.InstitutionAreas, "IdinstitutionArea", "AreaName", institutionDistrict.IdinstitutionArea);
            ViewData["Idinstitution"] = new SelectList(_context.Institutions, "Idinstitution", "InstitutionName", institutionDistrict.Idinstitution);
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
                //_context.InstitutionDistricts.Remove(institutionDistrict);
                institutionDistrict.Deleted = true;
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
