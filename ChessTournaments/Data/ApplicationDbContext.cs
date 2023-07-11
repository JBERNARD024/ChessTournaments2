using ChessTournaments.Models;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace ChessTournaments.Data
{

    /// <summary>
    /// esta classe representa a Base de Dados do nosso projeto
    /// </summary>
    public class ApplicationDbContext : IdentityDbContext
    {

        public ApplicationDbContext(
           DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        /* *********************************************
         * Criação das Tabelas
         * ********************************************* */
        public DbSet<Equipa> Equipa { get; set; }
        public DbSet<Fotografia> Fotografia { get; set; }
        public DbSet<Organizacao> Organizacao { get; set; }
        public DbSet<Partidas> Partidas { get; set; }
        public DbSet<Pessoa> Pessoa { get; set; }
        public DbSet<Torneio> Torneio { get; set; }

    }
}