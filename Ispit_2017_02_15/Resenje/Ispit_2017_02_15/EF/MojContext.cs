using Ispit_2017_02_15.Models;
using Microsoft.EntityFrameworkCore;

namespace Ispit_2017_02_15.EF
{
    public class MojContext : DbContext
    {
        public MojContext(DbContextOptions<MojContext> options) : base(options)
        {
        }

        public DbSet<AkademskaGodina> AkademskaGodina { get; set; }
        public DbSet<Nastavnik> Nastavnik { get; set; }
        public DbSet<Angazovan> Angazovan { get; set; }
        public DbSet<Predmet> Predmet { get; set; }
        public DbSet<SlusaPredmet> SlusaPredmet { get; set; }
        public DbSet<UpisGodine> UpisGodine { get; set; }
        public DbSet<Student> Student { get; set; }
        public DbSet<OdrzaniCas> OdrzaniCasovi { get; set; }
        public DbSet<OdrzaniCasDetalji> OdrzaniCasDetalji { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<AuthorizationToken> AuthorizationTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<SlusaPredmet>()
                .HasOne(x => x.UpisGodine)
                .WithMany()
                .HasForeignKey(x => x.UpisGodineId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OdrzaniCasDetalji>()
                .HasOne(x => x.SlusaPredmet)
                .WithMany(x=>x.Casovi)
                .HasForeignKey(x => x.SlusaPredmetId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SlusaPredmet>()
                .HasOne(x => x.UpisGodine)
                .WithMany(x => x.SlusaPredmete)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OdrzaniCasDetalji>()
                .HasOne(x => x.OdrzaniCas)
                .WithMany(x => x.OdrzaniCasDetaljii)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}