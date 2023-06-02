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
        //public async Task<IActionResult> Index()
        //{
        //    var ixchelWebpruebasContext = _context.FacilityTypes.Include(f => f.IdinstitutionNavigation);
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

            //var institutions = await _context.Institutions.Where(i => i.Deleted == false).ToListAsync();
            //IQueryable<FacilityType> facilitytypes = _context.FacilityTypes.Where(i => i.Deleted == false);

            int? institutionId = HttpContext.Session.GetInt32("InstitutionId");

            // Use the Institution ID to filter facilities
            IQueryable<FacilityType> facilitytypes = _context.FacilityTypes
                .Where(f => f.Idinstitution == institutionId && f.Deleted == false);
            //if (!String.IsNullOrEmpty(searchString))
            //{
            //    //institutions = institutions.Where(i => i.InstitutionName.Contains(searchString)).ToList();
            //    facilitytypes = facilitytypes.Where(i => i.FacilityTypeName.Contains(searchString)
            //                           && i.Deleted == false);
            //}

            if (!String.IsNullOrEmpty(searchString))
            {
                facilitytypes = facilitytypes.Where(i =>
                    i.FacilityTypeName.Contains(searchString) ||
                    i.IdfacilityType.ToString().Contains(searchString) ||
                    i.FacilityTypeCode.Contains(searchString) &&
                    i.Deleted == false);
            }

            switch (sortOrder)
            {
                case "name_desc":
                    facilitytypes = facilitytypes.OrderByDescending(i => i.FacilityTypeName);
                    break;
                case "Id":
                    facilitytypes = facilitytypes.OrderBy(i => i.IdfacilityType);
                    break;
                case "id_desc":
                    facilitytypes = facilitytypes.OrderByDescending(i => i.Idinstitution);
                    break;
                case "Enabled":
                    facilitytypes = facilitytypes.OrderBy(i => i.Enabled);
                    break;
                default:
                    facilitytypes = facilitytypes.OrderBy(i => i.IdfacilityType);
                    break;
            }

            int pageSize = 5;
            var pagedFacilityTypes = await PaginatedList<FacilityType>.CreateAsync(facilitytypes, pageNumber ?? 1, pageSize);

            return View(pagedFacilityTypes);
            //int pageSize = 3;
            //return View(await PaginatedList<Institution>.CreateAsync(institutions, pageNumber ?? 1, pageSize));
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
