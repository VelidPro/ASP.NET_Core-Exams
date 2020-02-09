using Microsoft.EntityFrameworkCore;
using RS1_PrakticniDioIspita_2017_01_24.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RS1_PrakticniDioIspita_2017_01_24.EF
{
    public class MojContext: DbContext
    {
        public MojContext(DbContextOptions<MojContext> options): base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<OdrzaniCas>()
                .HasOne(x => x.Angazovan)
                .WithMany(x => x.OdrzaniCasovi)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Angazovan>()
                .HasOne(x => x.Predmet)
                .WithMany().OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Angazovan>()
                .HasOne(x => x.Nastavnik)
                .WithMany().OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Angazovan>()
                .HasOne(x => x.Odjeljenje)
                .WithMany().OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Odjeljenje>()
                .HasOne(x => x.Nastavnik)
                .WithMany().OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UpisUOdjeljenje>()
                .HasOne(x => x.Odjeljenje)
                .WithMany().OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UpisUOdjeljenje>()
                .HasOne(x => x.Ucenik)
                .WithMany().OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OdrzaniCasDetalj>()
                .HasOne(x => x.OdrzaniCas)
                .WithMany().OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OdrzaniCasDetalj>()
                .HasOne(x => x.UpisUOdjeljenje)
                .WithMany().OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AuthorizationToken>()
                .HasOne(x => x.User)
                .WithMany(x => x.AuthTokens)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Nastavnik>()
                .HasOne(x => x.User)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);


            base.OnModelCreating(modelBuilder);


        }

        public DbSet<User> Users { get; set; }
        public DbSet<AuthorizationToken> AuthorizationTokens { get; set; }

        public DbSet<Nastavnik> Nastavnici { get; set; }
        public DbSet<OdrzaniCasDetalj> OdrzaniCasDetalji { get; set; }
        public DbSet<OdrzaniCas> OdrzaniCasovi { get; set; }
        public DbSet<Predmet> Predmeti { get; set; }
        public DbSet<Angazovan> Angazovani { get; set; }
        public DbSet<Odjeljenje> Odjeljenja { get; set; }
        public DbSet<UpisUOdjeljenje> UpisiUOdjeljenja { get; set; }
        public DbSet<Ucenik> Ucenici { get; set; }

    }
}
