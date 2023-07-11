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
    public class PartidaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PartidaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Partida
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Partidas.Include(p => p.Jogador1).Include(p => p.Jogador2).Include(p => p.Torneio);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Partida/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Partidas == null)
            {
                return NotFound();
            }

            var partidas = await _context.Partidas
                .Include(p => p.Jogador1)
                .Include(p => p.Jogador2)
                .Include(p => p.Torneio)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (partidas == null)
            {
                return NotFound();
            }

            return View(partidas);
        }

        // GET: Partida/Create
        public IActionResult Create()
        {
            ViewData["JogadorFK1"] = new SelectList(_context.Pessoa, "Id", "CodPostal");
            ViewData["JogadorFK2"] = new SelectList(_context.Pessoa, "Id", "CodPostal");
            ViewData["TorneioFK"] = new SelectList(_context.Torneio, "Id", "Descricao");
            return View();
        }

        // POST: Partida/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TorneioFK,JogadorFK1,JogadorFK2,Ronda,Resultado,dataJogo,Movimentos")] Partidas partidas)
        {
            if (ModelState.IsValid)
            {
                _context.Add(partidas);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["JogadorFK1"] = new SelectList(_context.Pessoa, "Id", "CodPostal", partidas.JogadorFK1);
            ViewData["JogadorFK2"] = new SelectList(_context.Pessoa, "Id", "CodPostal", partidas.JogadorFK2);
            ViewData["TorneioFK"] = new SelectList(_context.Torneio, "Id", "Descricao", partidas.TorneioFK);
            return View(partidas);
        }

        // GET: Partida/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Partidas == null)
            {
                return NotFound();
            }

            var partidas = await _context.Partidas.FindAsync(id);
            if (partidas == null)
            {
                return NotFound();
            }
            ViewData["JogadorFK1"] = new SelectList(_context.Pessoa, "Id", "CodPostal", partidas.JogadorFK1);
            ViewData["JogadorFK2"] = new SelectList(_context.Pessoa, "Id", "CodPostal", partidas.JogadorFK2);
            ViewData["TorneioFK"] = new SelectList(_context.Torneio, "Id", "Descricao", partidas.TorneioFK);
            return View(partidas);
        }

        // POST: Partida/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TorneioFK,JogadorFK1,JogadorFK2,Ronda,Resultado,dataJogo,Movimentos")] Partidas partidas)
        {
            if (id != partidas.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(partidas);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PartidasExists(partidas.Id))
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
            ViewData["JogadorFK1"] = new SelectList(_context.Pessoa, "Id", "CodPostal", partidas.JogadorFK1);
            ViewData["JogadorFK2"] = new SelectList(_context.Pessoa, "Id", "CodPostal", partidas.JogadorFK2);
            ViewData["TorneioFK"] = new SelectList(_context.Torneio, "Id", "Descricao", partidas.TorneioFK);
            return View(partidas);
        }

        // GET: Partida/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Partidas == null)
            {
                return NotFound();
            }

            var partidas = await _context.Partidas
                .Include(p => p.Jogador1)
                .Include(p => p.Jogador2)
                .Include(p => p.Torneio)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (partidas == null)
            {
                return NotFound();
            }

            return View(partidas);
        }

        // POST: Partida/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Partidas == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Partidas'  is null.");
            }
            var partidas = await _context.Partidas.FindAsync(id);
            if (partidas != null)
            {
                _context.Partidas.Remove(partidas);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PartidasExists(int id)
        {
          return (_context.Partidas?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
