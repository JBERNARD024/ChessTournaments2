using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ChessTournaments.Data;
using ChessTournaments.Models;

namespace ChessTournaments.Controllers
{
    public class FotografiaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FotografiaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Fotografia
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Fotografia.Include(f => f.Equipa).Include(f => f.Pessoa);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Fotografia/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Fotografia == null)
            {
                return NotFound();
            }

            var fotografia = await _context.Fotografia
                .Include(f => f.Equipa)
                .Include(f => f.Pessoa)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (fotografia == null)
            {
                return NotFound();
            }

            return View(fotografia);
        }

        // GET: Fotografia/Create
        public IActionResult Create()
        {
            ViewData["EquipaFK"] = new SelectList(_context.Equipa, "Id", "Morada");
            ViewData["PessoaFK"] = new SelectList(_context.Pessoa, "Id", "CodPostal");
            return View();
        }

        // POST: Fotografia/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NomeFicheiro,Data,Local,PessoaFK,EquipaFK")] Fotografia fotografia)
        {
            if (ModelState.IsValid)
            {
                _context.Add(fotografia);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EquipaFK"] = new SelectList(_context.Equipa, "Id", "Morada", fotografia.EquipaFK);
            ViewData["PessoaFK"] = new SelectList(_context.Pessoa, "Id", "CodPostal", fotografia.PessoaFK);
            return View(fotografia);
        }

        // GET: Fotografia/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Fotografia == null)
            {
                return NotFound();
            }

            var fotografia = await _context.Fotografia.FindAsync(id);
            if (fotografia == null)
            {
                return NotFound();
            }
            ViewData["EquipaFK"] = new SelectList(_context.Equipa, "Id", "Morada", fotografia.EquipaFK);
            ViewData["PessoaFK"] = new SelectList(_context.Pessoa, "Id", "CodPostal", fotografia.PessoaFK);
            return View(fotografia);
        }

        // POST: Fotografia/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NomeFicheiro,Data,Local,PessoaFK,EquipaFK")] Fotografia fotografia)
        {
            if (id != fotografia.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(fotografia);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FotografiaExists(fotografia.Id))
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
            ViewData["EquipaFK"] = new SelectList(_context.Equipa, "Id", "Morada", fotografia.EquipaFK);
            ViewData["PessoaFK"] = new SelectList(_context.Pessoa, "Id", "CodPostal", fotografia.PessoaFK);
            return View(fotografia);
        }

        // GET: Fotografia/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Fotografia == null)
            {
                return NotFound();
            }

            var fotografia = await _context.Fotografia
                .Include(f => f.Equipa)
                .Include(f => f.Pessoa)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (fotografia == null)
            {
                return NotFound();
            }

            return View(fotografia);
        }

        // POST: Fotografia/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Fotografia == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Fotografia'  is null.");
            }
            var fotografia = await _context.Fotografia.FindAsync(id);
            if (fotografia != null)
            {
                _context.Fotografia.Remove(fotografia);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FotografiaExists(int id)
        {
          return (_context.Fotografia?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
