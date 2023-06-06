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
    public class PermissionRolsController : Controller
    {
        private readonly IxchelWebpruebasContext _context;

        public PermissionRolsController(IxchelWebpruebasContext context)
        {
            _context = context;
        }

        // GET: PermissionRols
        public async Task<IActionResult> Index()
        {
            var ixchelWebpruebasContext = _context.PermissionRols.Include(p => p.IdpermissionNavigation).Include(p => p.IdrolNavigation);
            return View(await ixchelWebpruebasContext.ToListAsync());
        }

        // GET: PermissionRols/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.PermissionRols == null)
            {
                return NotFound();
            }

            var permissionRol = await _context.PermissionRols
                .Include(p => p.IdpermissionNavigation)
                .Include(p => p.IdrolNavigation)
                .FirstOrDefaultAsync(m => m.IdpermissionRol == id);
            if (permissionRol == null)
            {
                return NotFound();
            }

            return View(permissionRol);
        }

        // GET: PermissionRols/Create
        public IActionResult Create()
        {
            ViewData["Idpermission"] = new SelectList(_context.Permissions, "Idpermission", "Idpermission");
            ViewData["Idrol"] = new SelectList(_context.Rols, "Idrol", "Idrol");
            return View();
        }

        // POST: PermissionRols/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdpermissionRol,Idrol,Idpermission,Read,Add,Write,Delete,Print")] PermissionRol permissionRol)
        {
            if (ModelState.IsValid)
            {
                _context.Add(permissionRol);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Idpermission"] = new SelectList(_context.Permissions, "Idpermission", "Idpermission", permissionRol.Idpermission);
            ViewData["Idrol"] = new SelectList(_context.Rols, "Idrol", "Idrol", permissionRol.Idrol);
            return View(permissionRol);
        }

        // GET: PermissionRols/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.PermissionRols == null)
            {
                return NotFound();
            }

            var permissionRol = await _context.PermissionRols.FindAsync(id);
            if (permissionRol == null)
            {
                return NotFound();
            }
            ViewData["Idpermission"] = new SelectList(_context.Permissions, "Idpermission", "Idpermission", permissionRol.Idpermission);
            ViewData["Idrol"] = new SelectList(_context.Rols, "Idrol", "Idrol", permissionRol.Idrol);
            return View(permissionRol);
        }

        // POST: PermissionRols/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdpermissionRol,Idrol,Idpermission,Read,Add,Write,Delete,Print")] PermissionRol permissionRol)
        {
            if (id != permissionRol.IdpermissionRol)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(permissionRol);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PermissionRolExists(permissionRol.IdpermissionRol))
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
            ViewData["Idpermission"] = new SelectList(_context.Permissions, "Idpermission", "Idpermission", permissionRol.Idpermission);
            ViewData["Idrol"] = new SelectList(_context.Rols, "Idrol", "Idrol", permissionRol.Idrol);
            return View(permissionRol);
        }

        // GET: PermissionRols/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.PermissionRols == null)
            {
                return NotFound();
            }

            var permissionRol = await _context.PermissionRols
                .Include(p => p.IdpermissionNavigation)
                .Include(p => p.IdrolNavigation)
                .FirstOrDefaultAsync(m => m.IdpermissionRol == id);
            if (permissionRol == null)
            {
                return NotFound();
            }

            return View(permissionRol);
        }

        // POST: PermissionRols/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.PermissionRols == null)
            {
                return Problem("Entity set 'IxchelWebpruebasContext.PermissionRols'  is null.");
            }
            var permissionRol = await _context.PermissionRols.FindAsync(id);
            if (permissionRol != null)
            {
                _context.PermissionRols.Remove(permissionRol);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PermissionRolExists(int id)
        {
          return (_context.PermissionRols?.Any(e => e.IdpermissionRol == id)).GetValueOrDefault();
        }
    }
}
