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
    public class FacilitiesController : Controller
    {
        private readonly IxchelWebpruebasContext _context;

        public FacilitiesController(IxchelWebpruebasContext context)
        {
            _context = context;
        }

        // GET: Facilities
        //public async Task<IActionResult> Index()
        //{
        //    var ixchelWebpruebasContext = _context.Facilities.Include(f => f.IddistrictNavigation).Include(f => f.IdfacilityTypeNavigation).Include(f => f.IdinstitutionNavigation);
        //    return View(await ixchelWebpruebasContext.Where(i => i.Deleted == false).ToListAsync());
        //}

        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            //Sortorder?
            ViewData["CurrentSort"] = sortOrder;
            ViewData["CurrentFilter"] = searchString;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["IdSortParm"] = sortOrder == "Idfacility" ? "id_desc" : "Idfacility";
            ViewData["IdInstitutionSortParm"] = sortOrder == "Idinstitution" ? "idinstitution_desc" : "Idinstitution";
            ViewData["EnabledSortParm"] = sortOrder == "Enabled" ? "enabled_desc" : "Enabled";
            ViewData["FacilityCodeSortParm"] = sortOrder == "FacilityCode" ? "facilitycode_desc" : "FacilityCode";
            ViewData["FacilityTypeSortParm"] = sortOrder == "IdfacilityType" ? "facilitytype_desc" : "IdfacilityType";
            ViewData["AreaCodeSortParm"] = sortOrder == "AreaCode" ? "areacode_desc" : "AreaCode";
            ViewData["DistrictTypeSortParm"] = sortOrder == "DistrictCode" ? "districtcode_desc" : "DistrictCode";


            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            //var institutions = await _context.Institutions.Where(i => i.Deleted == false).ToListAsync();
            IQueryable<Facility> facilities = _context.Facilities
                .Where(i => i.Deleted == false)
                .Include(f => f.IddistrictNavigation)
                    .ThenInclude(d => d.DistrictName)
                .Include(f => f.IdfacilityTypeNavigation)
                    .ThenInclude(t => t.FacilityTypeName);

            //if (!String.IsNullOrEmpty(searchString))
            //{
            //    //institutions = institutions.Where(i => i.InstitutionName.Contains(searchString)).ToList();
            //    facilities = facilities.Where(i => i.FacilityName.Contains(searchString)
            //                           && i.Deleted == false);
            //}

            if (!String.IsNullOrEmpty(searchString))
            {
                facilities = facilities.Where(i =>
                    i.FacilityName.Contains(searchString) ||
                    i.Idfacility.ToString().Contains(searchString) ||
                    i.FacilityCode.Contains(searchString) &&
                    i.Deleted == false);
            }
            //switch for the filtering ASC and DESC for each column
            switch (sortOrder)
            {
                case "name_desc":
                    facilities = facilities.OrderByDescending(i => i.FacilityName);
                    break;
                case "Idfacility":
                    facilities = facilities.OrderBy(i => i.Idfacility);
                    break;
                case "id_desc":
                    facilities = facilities.OrderByDescending(i => i.Idfacility);
                    break;
                case "Idinstitution":
                    facilities = facilities.OrderBy(i => i.Idinstitution);
                    break;
                case "idinstitution_desc":
                    facilities = facilities.OrderByDescending(i => i.Idinstitution);
                    break;
                case "Enabled":
                    facilities = facilities.OrderBy(i => i.Enabled);
                    break;;
                case "FacilityCode":
                    facilities = facilities.OrderBy(i => i.FacilityCode);
                    break;
                case "FacilityType":
                    facilities = facilities.OrderBy(i => i.IdfacilityType);
                    break;
                case "enabled_desc":
                    facilities = facilities.OrderByDescending(i => i.Enabled);
                    break; ;
                case "facilitycode_desc":
                    facilities = facilities.OrderByDescending(i => i.FacilityCode);
                    break;
                case "facilitytype_desc":
                    facilities = facilities.OrderByDescending(i => i.IdfacilityType);
                    break;
                case "AreaCode":
                    facilities = facilities.OrderBy(i => i.AreaCode);
                    break;
                case "areacode_desc":
                    facilities = facilities.OrderByDescending(i => i.AreaCode);
                    break;
                case "DistrictCode":
                    facilities = facilities.OrderBy(i => i.DistrictCode);
                    break;
                case "districtcode_desc":
                    facilities = facilities.OrderByDescending(i => i.DistrictCode);
                    break;
                default:
                    facilities = facilities.OrderBy(i => i.FacilityName);
                    break;
            }


            int pageSize = 5;
            var pagedFacilities = await PaginatedList<Facility>.CreateAsync(facilities, pageNumber ?? 1, pageSize);

            return View(pagedFacilities);
            //int pageSize = 3;
            //return View(await PaginatedList<Institution>.CreateAsync(institutions, pageNumber ?? 1, pageSize));
        }

        // GET: Facilities/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Facilities == null)
            {
                return NotFound();
            }

            var facility = await _context.Facilities
                .Include(f => f.IddistrictNavigation)
                .Include(f => f.IdfacilityTypeNavigation)
                .Include(f => f.IdinstitutionNavigation)
                .FirstOrDefaultAsync(m => m.Idfacility == id);
            if (facility == null)
            {
                return NotFound();
            }

            return View(facility);
        }

        // GET: Facilities/Create
        public IActionResult Create()
        {
            ViewData["Iddistrict"] = new SelectList(_context.InstitutionDistricts, "IdinstitutionDistrict", "DistrictName");
            ViewData["IdfacilityType"] = new SelectList(_context.FacilityTypes, "IdfacilityType", "FacilityTypeName");
            ViewData["Idinstitution"] = new SelectList(_context.Institutions, "Idinstitution", "InstitutionName");
            return View();
        }

        // POST: Facilities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Idfacility,Idinstitution,Iddistrict,FacilityCode,IdfacilityType,FacilityName,Enabled,Deleted,AreaCode,DistrictCode")] Facility facility)
        {
            if (ModelState.IsValid)
            {
                _context.Add(facility);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Iddistrict"] = new SelectList(_context.InstitutionDistricts, "IdinstitutionDistrict", "IdinstitutionDistrict", facility.Iddistrict);
            ViewData["IdfacilityType"] = new SelectList(_context.FacilityTypes, "IdfacilityType", "IdfacilityType", facility.IdfacilityType);
            ViewData["Idinstitution"] = new SelectList(_context.Institutions, "Idinstitution", "Idinstitution", facility.Idinstitution);
            return View(facility);
        }

        // GET: Facilities/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Facilities == null)
            {
                return NotFound();
            }

            var facility = await _context.Facilities.FindAsync(id);
            if (facility == null)
            {
                return NotFound();
            }
            ViewData["Iddistrict"] = new SelectList(_context.InstitutionDistricts, "IdinstitutionDistrict", "IdinstitutionDistrict", facility.Iddistrict);
            ViewData["IdfacilityType"] = new SelectList(_context.FacilityTypes, "IdfacilityType", "IdfacilityType", facility.IdfacilityType);
            ViewData["Idinstitution"] = new SelectList(_context.Institutions, "Idinstitution", "Idinstitution", facility.Idinstitution);
            return View(facility);
        }

        // POST: Facilities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Idfacility,Idinstitution,Iddistrict,FacilityCode,IdfacilityType,FacilityName,Enabled,Deleted,AreaCode,DistrictCode")] Facility facility)
        {
            if (id != facility.Idfacility)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(facility);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FacilityExists(facility.Idfacility))
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
            ViewData["Iddistrict"] = new SelectList(_context.InstitutionDistricts, "IdinstitutionDistrict", "IdinstitutionDistrict", facility.Iddistrict);
            ViewData["IdfacilityType"] = new SelectList(_context.FacilityTypes, "IdfacilityType", "IdfacilityType", facility.IdfacilityType);
            ViewData["Idinstitution"] = new SelectList(_context.Institutions, "Idinstitution", "Idinstitution", facility.Idinstitution);
            return View(facility);
        }

        // GET: Facilities/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Facilities == null)
            {
                return NotFound();
            }

            var facility = await _context.Facilities
                .Include(f => f.IddistrictNavigation)
                .Include(f => f.IdfacilityTypeNavigation)
                .Include(f => f.IdinstitutionNavigation)
                .FirstOrDefaultAsync(m => m.Idfacility == id);
            if (facility == null)
            {
                return NotFound();
            }

            return View(facility);
        }

        // POST: Facilities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Facilities == null)
            {
                return Problem("Entity set 'IxchelWebpruebasContext.Facilities'  is null.");
            }
            var facility = await _context.Facilities.FindAsync(id);
            if (facility != null)
            {
                _context.Facilities.Remove(facility);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FacilityExists(int id)
        {
          return (_context.Facilities?.Any(e => e.Idfacility == id)).GetValueOrDefault();
        }
    }
}
