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
    public class InstitutionCountriesController : Controller
    {
        private readonly IxchelWebpruebasContext _context;

        public InstitutionCountriesController(IxchelWebpruebasContext context)
        {
            _context = context;
        }

        // GET: InstitutionCountries

        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["CurrentFilter"] = searchString;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["IdSortParm"] = sortOrder == "Id" ? "id_desc" : "Id";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            //var institutions = await _context.Institutions.Where(i => i.Deleted == false).ToListAsync();
            //IQueryable<InstitutionCountry> countries = _context.InstitutionCountries.Where(i => i.Deleted == false);

            int? institutionId = HttpContext.Session.GetInt32("InstitutionId");
            string? institutionName = HttpContext.Session.GetString("InstitutionName");
            ViewBag.InstitutionName = institutionName;
            // Use the Institution ID to filter facilities
            IQueryable<InstitutionCountry> countries = _context.InstitutionCountries
                .Where(f => f.Idinstitution == institutionId && f.Deleted == false);

            if (!String.IsNullOrEmpty(searchString))
            {
                countries = countries.Where(i =>
                    i.CountryName.Contains(searchString) ||
                    i.IdinstitutionCountry.ToString().Contains(searchString) &&
                    i.Deleted == false);
            }

            switch (sortOrder)
            {
                case "name_desc":
                    countries = countries.OrderByDescending(i => i.CountryName);
                    break;
                case "Id":
                    countries = countries.OrderBy(i => i.CountryCode);
                    break;
                case "id_desc":
                    countries = countries.OrderByDescending(i => i.CountryCode);
                    break;
                case "Enabled":
                    countries = countries.OrderBy(i => i.Enabled);
                    break;
                default:
                    countries = countries.OrderBy(i => i.CountryName);
                    break;
            }

            int pageSize = 5;
            var pagedInstitutionCountries = await PaginatedList<InstitutionCountry>.CreateAsync(countries, pageNumber ?? 1, pageSize);

            return View(pagedInstitutionCountries);
        }
            //public async Task<IActionResult> Index()
            //{
            //    var ixchelWebpruebasContext = _context.InstitutionCountries.Include(i => i.IdinstitutionNavigation);
            //    return View(await ixchelWebpruebasContext.Where(i => i.Deleted == false).ToListAsync());
            //}

            // GET: InstitutionCountries/Details/5
            public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.InstitutionCountries == null)
            {
                return NotFound();
            }

            var institutionCountry = await _context.InstitutionCountries
                .Include(i => i.IdinstitutionNavigation)
                .FirstOrDefaultAsync(m => m.IdinstitutionCountry == id);
            if (institutionCountry == null)
            {
                return NotFound();
            }

            return View(institutionCountry);
        }

        // GET: InstitutionCountries/Create
        public IActionResult Create()
        {
            ViewData["Idinstitution"] = new SelectList(_context.Institutions, "Idinstitution", "InstitutionName");
            return View();
        }

        // POST: InstitutionCountries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdinstitutionCountry,Idinstitution,CountryCode,CountryName,CountryPhoneCode,CountryDomainName,Enabled,Deleted")] InstitutionCountry institutionCountry)
        {
            if (ModelState.IsValid)
            {
                _context.Add(institutionCountry);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Idinstitution"] = new SelectList(_context.Institutions, "Idinstitution", "Idinstitution", institutionCountry.Idinstitution);
            return View(institutionCountry);
        }

        // GET: InstitutionCountries/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.InstitutionCountries == null)
            {
                return NotFound();
            }

            var institutionCountry = await _context.InstitutionCountries.FindAsync(id);
            if (institutionCountry == null)
            {
                return NotFound();
            }
            ViewData["Idinstitution"] = new SelectList(_context.Institutions, "Idinstitution", "Idinstitution", institutionCountry.Idinstitution);
            return View(institutionCountry);
        }

        // POST: InstitutionCountries/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdinstitutionCountry,Idinstitution,CountryCode,CountryName,CountryPhoneCode,CountryDomainName,Enabled,Deleted")] InstitutionCountry institutionCountry)
        {
            if (id != institutionCountry.IdinstitutionCountry)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(institutionCountry);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InstitutionCountryExists(institutionCountry.IdinstitutionCountry))
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
            ViewData["Idinstitution"] = new SelectList(_context.Institutions, "Idinstitution", "Idinstitution", institutionCountry.Idinstitution);
            return View(institutionCountry);
        }

        // GET: InstitutionCountries/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.InstitutionCountries == null)
            {
                return NotFound();
            }

            var institutionCountry = await _context.InstitutionCountries
                .Include(i => i.IdinstitutionNavigation)
                .FirstOrDefaultAsync(m => m.IdinstitutionCountry == id);
            if (institutionCountry == null)
            {
                return NotFound();
            }

            return View(institutionCountry);
        }

        // POST: InstitutionCountries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.InstitutionCountries == null)
            {
                return Problem("Entity set 'IxchelWebpruebasContext.InstitutionCountries'  is null.");
            }
            var institutionCountry = await _context.InstitutionCountries.FindAsync(id);
            if (institutionCountry != null)
            {
                //_context.InstitutionCountries.Remove(institutionCountry);
                institutionCountry.Deleted = true;
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InstitutionCountryExists(int id)
        {
          return (_context.InstitutionCountries?.Any(e => e.IdinstitutionCountry == id)).GetValueOrDefault();
        }
    }
}
