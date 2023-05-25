using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ZaculeuValley.IxchelAdmin.Models;
using PagedList;

namespace ZaculeuValley.IxchelAdmin.Controllers
{
    public class InstitutionsController : Controller
    {
        private readonly IxchelWebpruebasContext _context;

        public InstitutionsController(IxchelWebpruebasContext context)
        {
            _context = context;
        }

        // GET: Institutions

        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["CurrentFilter"] = searchString;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["IdSortParm"] = sortOrder == "Id" ? "id_desc" : "Id"; 
            ViewData["EnabledSortParm"] = sortOrder == "Enabled" ? "enabled_desc" : "Enabled";
            

            if (searchString != null )
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            //var institutions = await _context.Institutions.Where(i => i.Deleted == false).ToListAsync();
            IQueryable<Institution> institutions = _context.Institutions.Where(i => i.Deleted == false);

            if (!String.IsNullOrEmpty(searchString))
            {
                institutions = institutions.Where(i =>
                    i.InstitutionName.Contains(searchString) ||
                    i.Idinstitution.ToString().Contains(searchString) ||
                    i.InstitutionCode.Contains(searchString) &&
                    i.Deleted == false);
            }

            switch (sortOrder)
            {
                case "name_desc":
                    institutions = institutions.OrderByDescending(i => i.InstitutionName);
                    break;
                case "Id":
                    institutions = institutions.OrderBy(i => i.Idinstitution);
                    break;
                case "id_desc":
                    institutions = institutions.OrderByDescending(i => i.Idinstitution);
                    break;
                case "Enabled":
                    institutions = institutions.OrderBy(i => i.Enabled);
                    break;
                default:
                    institutions = institutions.OrderBy(i => i.InstitutionName);
                    break;
            }

            int pageSize = 5;
            var pagedInstitutions = await PaginatedList<Institution>.CreateAsync(institutions, pageNumber ?? 1, pageSize);

            return View(pagedInstitutions);
            //int pageSize = 3;
            //return View(await PaginatedList<Institution>.CreateAsync(institutions, pageNumber ?? 1, pageSize));
        }

        //public async Task<IActionResult> Index()
        //{
        //      return _context.Institutions != null ?
        //                 View(await _context.Institutions.Where(i => i.Deleted == false).ToListAsync()) :
        //                  Problem("Entity set 'IxchelWebpruebasContext.Institutions' is null.");
        //}

        // GET: Institutions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Institutions == null)
            {
                return NotFound();
            }

            var institution = await _context.Institutions
                .FirstOrDefaultAsync(m => m.Idinstitution == id);
            if (institution == null)
            {
                return NotFound();
            }

            return View(institution);
        }

        // GET: Institutions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Institutions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Idinstitution,InstitutionName,InstitutionCode,Enabled,Deleted")] Institution institution)
        {
            if (ModelState.IsValid)
            {
                _context.Add(institution);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(institution);
        }

        // GET: Institutions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Institutions == null)
            {
                return NotFound();
            }

            var institution = await _context.Institutions.FindAsync(id);
            if (institution == null)
            {
                return NotFound();
            }
            return View(institution);
        }

        // POST: Institutions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Idinstitution,InstitutionName,InstitutionCode,Enabled,Deleted")] Institution institution)
        {
            if (id != institution.Idinstitution)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(institution);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InstitutionExists(institution.Idinstitution))
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
            return View(institution);
        }

        // GET: Institutions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Institutions == null)
            {
                return NotFound();
            }

            var institution = await _context.Institutions
                .FirstOrDefaultAsync(m => m.Idinstitution == id);
            if (institution == null)
            {
                return NotFound();
            }

            return View(institution);
        }

        // POST: Institutions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Institutions == null)
            {
                return Problem("Entity set 'IxchelWebpruebasContext.Institutions'  is null.");
            }
            var institution = await _context.Institutions.FindAsync(id);
            if (institution != null)
            {
                _context.Institutions.Remove(institution);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InstitutionExists(int id)
        {
          return (_context.Institutions?.Any(e => e.Idinstitution == id)).GetValueOrDefault();
        }
    }
}
