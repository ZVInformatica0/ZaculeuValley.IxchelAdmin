using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ZaculeuValley.IxchelAdmin.Models;
using Microsoft.AspNetCore.Http;
using ZaculeuValley.IxchelAdmin.Data;

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
            ViewData["CurrentSort"] = sortOrder;
            ViewData["CurrentFilter"] = searchString;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["IdSortParm"] = sortOrder == "Id" ? "id_desc" : "Id";
            ViewData["EnabledSortParm"] = sortOrder == "Enabled" ? "enabled_desc" : "Enabled";
            ViewData["IdInstitutionParm"] = sortOrder == "Idinstitution" ? "idinstitution_desc" : "Idinstitution";
            //ViewData["FacilityCodeParm"] = sortOrder == "FacilityCode" ? "facilitycode_desc" : "FacilityCode" ?? "FacilityCode";
            ViewData["FacilityCodeParm"] = String.IsNullOrEmpty(sortOrder) ? "facilitycode_desc" : "";
            ViewData["FacilityCountryParm"] = sortOrder == "Facilitycountry" ? "facility_desc" : "Facilitycountry";


            ViewData["IdfacilityTypeParm"] =  sortOrder == "IdfacilityType" ? "idfacilitytype_desc" : "IdfacilityType";
            ViewData["AreaCodeParm"] = sortOrder == "AreaCode" ? "areacode_desc" : "AreaCode";
            ViewData["DistrictCodeParm"] = sortOrder == "DistrictCode" ? "districtcode_desc" : "DistrictCode";

            try
            {
                ViewData["FacilityCodeParm"] = sortOrder == "FacilityCode" ? "facilitycode_desc" : "FacilityCode";
                if (ViewData["FacilityCodeParm"] == null)
                {
                    // Handle si es null
                    ViewData["FacilityCodeParm"] = 0;
                }
            }
            catch (SqlNullValueException ex)
            {
                // Handle de la exception
                Console.WriteLine(ex.Message);

            }


            

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            //IQueryable<Facility> facilities = _context.Facilities.Where(i => i.Deleted == false);
            // Retrieve the stored Institution ID from session
            int? institutionId = HttpContext.Session.GetInt32("InstitutionId");
            string? institutionName = HttpContext.Session.GetString("InstitutionName");
            ViewBag.InstitutionName = institutionName;


            // Use the Institution ID to filter facilities
            //IQueryable<Facility> facilities = _context.Facilities
            //    .Where(f => f.Idinstitution == institutionId && f.Deleted == false);

            IQueryable<Facility> facilities = _context.Facilities
                .Where(f => f.Idinstitution == institutionId && f.Deleted == false)
                .Include(f => f.IdinstitutionNavigation)
                .Include(f => f.IddistrictNavigation)
                    .ThenInclude(d => d.IdinstitutionAreaNavigation)
                        .ThenInclude(a => a.IdinstitutionCountryNavigation)
                .Include(f => f.IdfacilityTypeNavigation);


            //IQueryable<Facility> facilities = _context.Facilities
            //.Where(i => i.Deleted == false)
            //.Join(_context.InstitutionDistricts, f => f.IDDistrict, id => id.IDInstitutionDistrict, (f, id) => new { Facility = f, InstitutionDistrict = id })
            //.Join(_context.InstitutionAreas, fi => fi.InstitutionDistrict.IDInstitutionArea, ia => ia.IDInstitutionArea, (fi, ia) => new { fi.Facility, fi.InstitutionDistrict, InstitutionArea = ia })
            //.Join(_context.FacilityTypes, fia => fia.Facility.IDFacilityType, ft => ft.IDFacilityType, (fia, ft) => new { fia.Facility, fia.InstitutionDistrict, fia.InstitutionArea, FacilityType = ft })
            //.Join(_context.Institutions, fiaft => fiaft.Facility.IDInstitution, i => i.IDInstitution, (fiaft, i) => new { fiaft.Facility, fiaft.InstitutionDistrict, fiaft.InstitutionArea, fiaft.FacilityType, Institution = i })
            //.Select(fiafti => new Facility
            //{
            //    IDFacility = fiafti.Facility.IDFacility,
            //    FacilityCode = fiafti.Facility.FacilityCode,
            //    IDInstitution = fiafti.Facility.IDInstitution,
            //    InstitutionName = fiafti.Institution.InstitutionName,
            //    FacilityTypeName = fiafti.FacilityType.FacilityTypeName,
            //    AreaName = fiafti.InstitutionArea.AreaName,
            //    DistrictName = fiafti.InstitutionDistrict.DistrictName
            //});




            if (!String.IsNullOrEmpty(searchString))
            {
                facilities = facilities.Where(i =>
                    i.FacilityName.Contains(searchString) ||
                    i.Idfacility.ToString().Contains(searchString) ||
                    i.FacilityCode.Contains(searchString) &&
                    i.Deleted == false);
            }

            switch (sortOrder)
            {
                case "name_desc":
                    facilities = facilities.OrderByDescending(i => i.FacilityName);
                    break;
                case "Id":
                    facilities = facilities.OrderBy(i => i.Idfacility);
                    break;
                case "id_desc":
                    facilities = facilities.OrderByDescending(i => i.Idfacility);
                    break;
                case "Enabled":
                    facilities = facilities.OrderBy(i => i.Enabled);
                    break;
                case "enabled_desc":
                    facilities = facilities.OrderByDescending(i => i.Enabled);
                    break;
                case "Idinstitution":
                    facilities = facilities.OrderBy(i => i.Idinstitution);
                    break;
                case "idinstitution_desc":
                    facilities = facilities.OrderByDescending(i => i.Idinstitution);
                    break;
                case "FacilityCode":
                    facilities = facilities.OrderBy(i => i.FacilityCode);
                    ViewData["FacilityCodeParm"] = "facilitycode_desc";
                    break;
                case "facilitycode_desc":
                    facilities = facilities.OrderByDescending(i => i.FacilityCode);
                    ViewData["FacilityCodeParm"] = "FacilityCode";
                    break;
                case "IdfacilityType":
                    facilities = facilities.OrderBy(i => i.IdfacilityType);
                    break;
                case "idfacilitytype_desc":
                    facilities = facilities.OrderByDescending(i => i.IdfacilityType);
                    break;
                case "AreaCode":
                    facilities = facilities.OrderBy(i => i.AreaCode);
                    break;
                case "areacode_desc":
                    facilities = facilities.OrderByDescending(i => i.AreaCode);
                    break;
                case "Facilitycountry":
                    facilities = facilities.OrderBy(i => i.IddistrictNavigation.IdinstitutionAreaNavigation.IdinstitutionCountryNavigation.CountryName);
                    break;
                case "facilitycountry_desc":
                    facilities = facilities.OrderByDescending(i => i.IddistrictNavigation.IdinstitutionAreaNavigation.IdinstitutionCountryNavigation.CountryName);
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
            // Comprueba si el código de instalación ya existe en la base de datos
            bool isFacilityCodeUnique = await _context.Facilities.AllAsync(f => f.FacilityCode != facility.FacilityCode);

            if (!isFacilityCodeUnique)
            {
                ModelState.AddModelError("FacilityCode", "El código de instalación ya existe. Por favor, ingrese un código único.");
            }
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

            //Query para que el edit jale todas las variables del modelo de Facility
            var facility = await _context.Facilities
                .Include(f => f.IdinstitutionNavigation)
                .Include(f => f.IddistrictNavigation)
                .Include(f => f.IdfacilityTypeNavigation)
                .FirstOrDefaultAsync(m => m.Idfacility == id);

            if (facility == null)
            {
                return NotFound();
            }


            ViewData["Iddistrict"] = new SelectList(_context.InstitutionDistricts, "IdinstitutionDistrict", "DistrictName", facility.Iddistrict);
            ViewData["IdfacilityType"] = new SelectList(_context.FacilityTypes, "IdfacilityType", "FacilityTypeName", facility.IdfacilityType);
            ViewData["Idinstitution"] = new SelectList(_context.Institutions, "Idinstitution", "InstitutionName", facility.Idinstitution);
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
                //_context.Facilities.Remove(facility);
                facility.Deleted = true;
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
