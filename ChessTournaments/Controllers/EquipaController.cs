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
    public class EquipaController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public EquipaController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Equipa
        public async Task<IActionResult> Index()
        {
              return _context.Equipa != null ? 
                          View(await _context.Equipa
                          .Include(f => f.ListaFotos) 
                          .ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Equipa'  is null.");
        }

        // GET: Equipa/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Equipa == null)
            {
                return NotFound();
            }

            var equipa = await _context.Equipa
                .Include(f => f.ListaFotos)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (equipa == null)
            {
                return NotFound();
            }
            ViewBag.ListaJogadores = _context.Pessoa.Where(p => p.EquipaFK == id).ToList();
            return View(equipa);
        }

        // GET: Equipa/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Equipa/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,Morada,CodigoPostal,dataFundacao")] Equipa equipa, IFormFile imagemEquipa)
        {
            //variáveis auxiliares

            string nomeFoto = "";
            bool existeFoto = false;

            if (imagemEquipa == null)
            {
                // O Utilizador não fez upload de uma imagem
                // É adicionada uma imagem pré-definida à pessoa
                equipa.ListaFotos
                        .Add(new Fotografia
                        {
                            Data = DateTime.Now,
                            Local = "SemImagem",
                            NomeFicheiro = "ImagemDefault.jpeg"
                        });
            }
            else
            {
                if (imagemEquipa.ContentType != "image/jpg" &&
                    imagemEquipa.ContentType != "image/png" && imagemEquipa.ContentType != "image/jpeg")
                {
                    equipa.ListaFotos
                        .Add(new Fotografia
                        {
                            Data = DateTime.Now,
                            Local = "SemImagem",
                            NomeFicheiro = "ImagemDefault.jpeg"
                        });
                }
                else
                {
                    Guid g = Guid.NewGuid();
                    nomeFoto = g.ToString();
                    string extensaoNomeFoto = Path.GetExtension(imagemEquipa.FileName).ToLower();
                    nomeFoto += extensaoNomeFoto;

                    equipa.ListaFotos
                            .Add(new Fotografia
                            {
                                Data = DateTime.Now,
                                Local = "",
                                NomeFicheiro = nomeFoto
                            });
                    existeFoto = true;
                }
            }


            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(equipa);
                    await _context
                        .SaveChangesAsync();

                    if (existeFoto)
                    {
                        string nomeLocalizaoImagem = _webHostEnvironment.WebRootPath;
                        nomeLocalizaoImagem = Path.Combine(nomeLocalizaoImagem, "imagens");

                        if (!Directory.Exists(nomeLocalizaoImagem))
                        {
                            Directory.CreateDirectory(nomeLocalizaoImagem);
                        }

                        string nomeDoFicheiro = Path.Combine(nomeLocalizaoImagem, nomeFoto);
                        //guardar o ficheiro
                        using var stream = new FileStream(nomeDoFicheiro, FileMode.Create);
                        await imagemEquipa.CopyToAsync(stream);
                    }

                    return RedirectToAction(nameof(Index));

                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Ocorreu um erro com a adição dos dados "
                        + equipa.Nome);
                    //throw;
                }

            }
            return View(equipa);
        }

        // GET: Equipa/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Equipa == null)
            {
                return NotFound();
            }

            var equipa = await _context.Equipa.FindAsync(id);
            if (equipa == null)
            {
                return NotFound();
            }
            return View(equipa);
        }

        // POST: Equipa/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Morada,CodigoPostal,dataFundacao")] Equipa equipa)
        {
            if (id != equipa.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(equipa);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EquipaExists(equipa.Id))
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
            return View(equipa);
        }

        // GET: Equipa/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Equipa == null)
            {
                return NotFound();
            }

            var equipa = await _context.Equipa
                .FirstOrDefaultAsync(m => m.Id == id);
            if (equipa == null)
            {
                return NotFound();
            }

            return View(equipa);
        }

        // POST: Equipa/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Equipa == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Equipa'  is null.");
            }
            var equipa = await _context.Equipa.FindAsync(id);
            if (equipa != null)
            {
                _context.Equipa.Remove(equipa);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EquipaExists(int id)
        {
          return (_context.Equipa?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
