using eKlinika.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
namespace eKlinika.DataAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Doktor> Doktori { get; set; }

        public DbSet<Admin> Admin { get; set; }

        public DbSet<Pacijent> Pacijents { get; set; }

        public DbSet<Direktor> Direktor { get; set; }

        public DbSet<DoktorPacijentSpoj> DoktorPacijent { get; set; }

        public DbSet<ApplicationUser> Users { get; set; }

        public DbSet<Grad> Gradovi { get; set; }

        public DbSet<DoktorsIDNum> DoktorsIDNums { get; set; }

        public DbSet<Pakovanje> Pakovanja { get; set; }

        public DbSet<Lek> Lekovi { get; set; }

        public DbSet<DetaljiNarudzbine> DetaljiNarudzbine { get; set; }

        public DbSet<Narudzbina> Narudzbine { get; set; }

        public DbSet<Kupovina> Kupovine { get; set; }

        public DbSet<Pregled> Pregledi { get; set; }

        public DbSet<Odeljenje> Odeljenja { get; set; }

        public DbSet<Vreme> Vremena { get; set; }


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DoktorPacijentSpoj>()
                .HasOne(x => x.Pacijent)
                .WithMany(x => x.ListaDoktora)
                .HasForeignKey(x => x.PacijentID);
            modelBuilder.Entity<DoktorPacijentSpoj>()
                .HasOne(x => x.Doktor)
                .WithMany(x => x.ListaPacijenata)
                .HasForeignKey(x => x.DoktorID);

            base.OnModelCreating(modelBuilder);
        }
    }
}
