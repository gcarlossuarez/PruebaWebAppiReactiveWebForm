using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace PruebaWebAppiReactiveWebForm.Models
{
    public partial class Prueba1Context : DbContext
    {
        public Prueba1Context()
        {
        }

        public Prueba1Context(DbContextOptions<Prueba1Context> options)
            : base(options)
        {
        }

        public virtual DbSet<Direccion> Direccion { get; set; }
        public virtual DbSet<Persona> Persona { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Data Source=DESKTOP-TQ5EDHD;Initial Catalog=Prueba1;User ID=sa;Password=armagedon0;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Direccion>(entity =>
            {
                entity.HasIndex(e => e.PersonaId);

                entity.HasOne(d => d.Persona)
                    .WithMany(p => p.Direcciones)
                    .HasForeignKey(d => d.PersonaId);
            });

            modelBuilder.Entity<Persona>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.FechaNacimiento).HasColumnType("datetime");

                entity.Property(e => e.Nombre).IsRequired();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
