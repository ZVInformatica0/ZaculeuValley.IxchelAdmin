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
    public class UserRolsController : Controller
    {
        private readonly IxchelWebpruebasContext _context;

        public UserRolsController(IxchelWebpruebasContext context)
        {
            _context = context;
        }

        // GET: UserRols
        public async Task<IActionResult> Index()
        {
            var ixchelWebpruebasContext = _context.UserRols.Include(u => u.IdrolNavigation).Include(u => u.IduserNavigation);
            return View(await ixchelWebpruebasContext.ToListAsync());
        }

        // GET: UserRols/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.UserRols == null)
            {
                return NotFound();
            }

            var userRol = await _context.UserRols
                .Include(u => u.IdrolNavigation)
                .Include(u => u.IduserNavigation)
                .FirstOrDefaultAsync(m => m.IduserRol == id);
            if (userRol == null)
            {
                return NotFound();
            }

            return View(userRol);
        }

        // GET: UserRols/Create
        public IActionResult Create()
        {
            ViewData["Idrol"] = new SelectList(_context.Rols, "Idrol", "Idrol");
            ViewData["Iduser"] = new SelectList(_context.Users, "Iduser", "Iduser");
            return View();
        }

        // POST: UserRols/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IduserRol,Iduser,Idrol")] UserRol userRol)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userRol);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Idrol"] = new SelectList(_context.Rols, "Idrol", "Idrol", userRol.Idrol);
            ViewData["Iduser"] = new SelectList(_context.Users, "Iduser", "Iduser", userRol.Iduser);
            return View(userRol);
        }

        // GET: UserRols/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.UserRols == null)
            {
                return NotFound();
            }

            var userRol = await _context.UserRols.FindAsync(id);
            if (userRol == null)
            {
                return NotFound();
            }
            ViewData["Idrol"] = new SelectList(_context.Rols, "Idrol", "Idrol", userRol.Idrol);
            ViewData["Iduser"] = new SelectList(_context.Users, "Iduser", "Iduser", userRol.Iduser);
            return View(userRol);
        }

        // POST: UserRols/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IduserRol,Iduser,Idrol")] UserRol userRol)
        {
            if (id != userRol.IduserRol)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userRol);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserRolExists(userRol.IduserRol))
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
            ViewData["Idrol"] = new SelectList(_context.Rols, "Idrol", "Idrol", userRol.Idrol);
            ViewData["Iduser"] = new SelectList(_context.Users, "Iduser", "Iduser", userRol.Iduser);
            return View(userRol);
        }

        // GET: UserRols/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.UserRols == null)
            {
                return NotFound();
            }

            var userRol = await _context.UserRols
                .Include(u => u.IdrolNavigation)
                .Include(u => u.IduserNavigation)
                .FirstOrDefaultAsync(m => m.IduserRol == id);
            if (userRol == null)
            {
                return NotFound();
            }

            return View(userRol);
        }

        // POST: UserRols/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.UserRols == null)
            {
                return Problem("Entity set 'IxchelWebpruebasContext.UserRols'  is null.");
            }
            var userRol = await _context.UserRols.FindAsync(id);
            if (userRol != null)
            {
                _context.UserRols.Remove(userRol);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserRolExists(int id)
        {
          return (_context.UserRols?.Any(e => e.IduserRol == id)).GetValueOrDefault();
        }
    }
}
