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
    public class OrganizacaoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrganizacaoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Organizacao
        public async Task<IActionResult> Index()
        {
              return _context.Organizacao != null ? 
                          View(await _context.Organizacao.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Organizacao'  is null.");
        }

        // GET: Organizacao/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Organizacao == null)
            {
                return NotFound();
            }

            var organizacao = await _context.Organizacao
                .FirstOrDefaultAsync(m => m.Id == id);
            if (organizacao == null)
            {
                return NotFound();
            }

            return View(organizacao);
        }

        // GET: Organizacao/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Organizacao/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,Email,Telemovel,Morada,CodigoPostal")] Organizacao organizacao)
        {
            if (ModelState.IsValid)
            {
                _context.Add(organizacao);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(organizacao);
        }

        // GET: Organizacao/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Organizacao == null)
            {
                return NotFound();
            }

            var organizacao = await _context.Organizacao.FindAsync(id);
            if (organizacao == null)
            {
                return NotFound();
            }
            return View(organizacao);
        }

        // POST: Organizacao/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Email,Telemovel,Morada,CodigoPostal")] Organizacao organizacao)
        {
            if (id != organizacao.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(organizacao);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrganizacaoExists(organizacao.Id))
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
            return View(organizacao);
        }

        // GET: Organizacao/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Organizacao == null)
            {
                return NotFound();
            }

            var organizacao = await _context.Organizacao
                .FirstOrDefaultAsync(m => m.Id == id);
            if (organizacao == null)
            {
                return NotFound();
            }

            return View(organizacao);
        }

        // POST: Organizacao/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Organizacao == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Organizacao'  is null.");
            }
            var organizacao = await _context.Organizacao.FindAsync(id);
            if (organizacao != null)
            {
                _context.Organizacao.Remove(organizacao);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrganizacaoExists(int id)
        {
          return (_context.Organizacao?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
