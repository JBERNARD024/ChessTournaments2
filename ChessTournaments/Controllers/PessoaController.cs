using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ChessTournaments.Data;
using ChessTournaments.Models;
using Microsoft.AspNetCore.Identity;

namespace ChessTournaments.Controllers
{
    public class PessoaController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<IdentityUser> _userManager;

        public PessoaController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
        }

        // GET: Pessoa
        public async Task<IActionResult> Index()
        {
            // var auxiliar
            string idDaPessoaAutenticada = _userManager.GetUserId(User);

            var listaJogadores = _context.Pessoa
                               .Include(f => f.ListaFotos)
                               .Where(p => p.Username == idDaPessoaAutenticada);

            var applicationDbContext = _context.Pessoa.
                Include(p => p.Equipa).
                Include(f => f.ListaFotos);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Pessoa/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Pessoa == null)
            {
                return NotFound();
            }

            var pessoa = await _context.Pessoa
                .Include(p => p.Equipa)
                .Include(f => f.ListaFotos)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pessoa == null)
            {
                return NotFound();
            }

            return View(pessoa);
        }

        // GET: Pessoa/Create
        public IActionResult Create()
        {
            ViewData["EquipaFK"] = new SelectList(_context.Equipa, "Id", "Nome");
            return View();
        }

        // POST: Pessoa/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Primeiro_Nome,Ultimo_Nome,Username,DataNascimento,Sexo,Nacionalidade,Email,Telemovel,Morada,CodPostal,isFuncionario,EquipaFK,Score")] Pessoa pessoa, IFormFile imagemPessoa)
        {
            //variáveis auxiliares

            string nomeFoto = "";
            bool existeFoto = false;

            if (imagemPessoa == null)
            {
                // O Utilizador não fez upload de uma imagem
                // É adicionada uma imagem pré-definida à pessoa
                pessoa.ListaFotos
                        .Add(new Fotografia
                        {
                            Data = DateTime.Now,
                            Local = "SemImagem",
                            NomeFicheiro = "ImagemDefaultPessoa.jpeg"
                        });
            }
            else
            {
                if (imagemPessoa.ContentType != "image/jpg" &&
                    imagemPessoa.ContentType != "image/png" && imagemPessoa.ContentType != "image/jpeg")
                {
                    pessoa.ListaFotos
                        .Add(new Fotografia
                        {
                            Data = DateTime.Now,
                            Local = "SemImagem",
                            NomeFicheiro = "ImagemDefaultPessoa.jpeg"
                        });
                }
                else
                {
                    Guid g = Guid.NewGuid();
                    nomeFoto = g.ToString();
                    string extensaoNomeFoto = Path.GetExtension(imagemPessoa.FileName).ToLower();
                    nomeFoto += extensaoNomeFoto;

                    pessoa.ListaFotos
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

                    _context.Add(pessoa);
                    await _context.SaveChangesAsync();
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
                        await imagemPessoa.CopyToAsync(stream);
                    }

                    return RedirectToAction(nameof(Index));

                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Ocorreu um erro com a adição dos dados "
                        + pessoa.Primeiro_Nome);
                    //throw;
                }
            }
            ViewData["EquipaFK"] = new SelectList(_context.Equipa, "Id", "Nome", pessoa.EquipaFK);
            return View(pessoa);
        }

        // GET: Pessoa/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Pessoa == null)
            {
                return NotFound();
            }

            var pessoa = await _context.Pessoa.FindAsync(id);
            if (pessoa == null)
            {
                return NotFound();
            }
            ViewData["EquipaFK"] = new SelectList(_context.Equipa, "Id", "Nome", pessoa.EquipaFK);
            return View(pessoa);
        }

        // POST: Pessoa/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Primeiro_Nome,Ultimo_Nome,Username,DataNascimento,Sexo,Nacionalidade,Email,Telemovel,Morada,CodPostal,isFuncionario,EquipaFK,Score")] Pessoa pessoa)
        {
            if (id != pessoa.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pessoa);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PessoaExists(pessoa.Id))
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
            ViewData["EquipaFK"] = new SelectList(_context.Equipa, "Id", "Nome", pessoa.EquipaFK);
            return View(pessoa);
        }

        // GET: Pessoa/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Pessoa == null)
            {
                return NotFound();
            }

            var pessoa = await _context.Pessoa
                .Include(p => p.Equipa)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pessoa == null)
            {
                return NotFound();
            }

            return View(pessoa);
        }

        // POST: Pessoa/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Pessoa == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Pessoa'  is null.");
            }
            var pessoa = await _context.Pessoa.FindAsync(id);
            if (pessoa != null)
            {
                _context.Pessoa.Remove(pessoa);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PessoaExists(int id)
        {
            return (_context.Pessoa?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}