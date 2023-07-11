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
    public class TorneioController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TorneioController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Torneio
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Torneio.Include(t => t.Organizacao);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Torneio/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Torneio == null)
            {
                return NotFound();
            }

            var torneio = await _context.Torneio
                .Include(t => t.Organizacao)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (torneio == null)
            {
                return NotFound();
            }

            return View(torneio);
        }

        // GET: Torneio/Create
        public IActionResult Create()
        {
            ViewData["OrganizacaoFK"] = new SelectList(_context.Organizacao, "Id", "Nome");
            return View();
        }

        // POST: Torneio/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,dataInicio,dataFim,Local,OrganizacaoFK,ValorPremio,Descricao,Website")] Torneio torneio)
        {
            if (ModelState.IsValid)
            {
                _context.Add(torneio);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["OrganizacaoFK"] = new SelectList(_context.Organizacao, "Id", "Nome", torneio.OrganizacaoFK);
            return View(torneio);
        }

        // GET: Torneio/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Torneio == null)
            {
                return NotFound();
            }

            var torneio = await _context.Torneio.FindAsync(id);
            if (torneio == null)
            {
                return NotFound();
            }
            ViewData["OrganizacaoFK"] = new SelectList(_context.Organizacao, "Id", "Nome", torneio.OrganizacaoFK);
            return View(torneio);
        }

        // POST: Torneio/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,dataInicio,dataFim,Local,OrganizacaoFK,ValorPremio,Descricao,Website")] Torneio torneio)
        {
            if (id != torneio.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(torneio);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TorneioExists(torneio.Id))
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
            ViewData["OrganizacaoFK"] = new SelectList(_context.Organizacao, "Id", "Nome", torneio.OrganizacaoFK);
            return View(torneio);
        }

        // GET: Torneio/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Torneio == null)
            {
                return NotFound();
            }

            var torneio = await _context.Torneio
                .Include(t => t.Organizacao)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (torneio == null)
            {
                return NotFound();
            }

            return View(torneio);
        }

        // POST: Torneio/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Torneio == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Torneio'  is null.");
            }
            var torneio = await _context.Torneio.FindAsync(id);
            if (torneio != null)
            {
                _context.Torneio.Remove(torneio);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TorneioExists(int id)
        {
          return (_context.Torneio?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
