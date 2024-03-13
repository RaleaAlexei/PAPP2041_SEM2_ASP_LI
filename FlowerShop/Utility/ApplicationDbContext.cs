using Boutique.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Boutique.Utility
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<Categorie> Categorie { get; set; }
        public DbSet<Produs> Produs { get; set; }
        public DbSet<Utilizator> Utilizator { get; set; }
    }
}
