using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace SolutionFrontEnd.Models
{
    public partial class GranjaCoralesContext : IdentityDbContext
    {
        public GranjaCoralesContext()
        {
        }

        public GranjaCoralesContext(DbContextOptions<GranjaCoralesContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Articulos> Articulos { get; set; }
        public virtual DbSet<ArticulosSolicitud> ArticulosSolicitud { get; set; }
        public virtual DbSet<Categorias> Categorias { get; set; }
        public virtual DbSet<Solicitudes> Solicitudes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=localhost;Database=GranjaCorales;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Articulos>(entity =>
            {
                entity.HasKey(e => e.IdArticulo);

                entity.Property(e => e.IdArticulo)
                    .HasColumnName("id_articulo");

                entity.Property(e => e.Color)
                    .HasColumnName("color")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Dieta)
                    .HasColumnName("dieta")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Dificultad)
                    .HasColumnName("dificultad")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Familia)
                    .HasColumnName("familia")
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.FecModificacion)
                    .HasColumnName("fec_modificacion")
                    .HasColumnType("datetime");

                entity.Property(e => e.IdCategoria).HasColumnName("id_categoria");

                entity.Property(e => e.ModificadoPor)
                    .HasColumnName("modificado_por")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NombreCientifico)
                    .HasColumnName("nombre_cientifico")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.NombreComun)
                    .HasColumnName("nombre_comun")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Origen)
                    .HasColumnName("origen")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TamanoMax)
                    .HasColumnName("tamano_max")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TamanoMinPecera)
                    .HasColumnName("tamano_min_pecera")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Temperamento)
                    .HasColumnName("temperamento")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Tipo)
                    .HasColumnName("tipo")
                    .HasMaxLength(10)
                    .IsFixedLength();

                 entity.Property(e => e.ImagePath)
                    .HasColumnName("imagePath")
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdCategoriaNavigation)
                    .WithMany(p => p.Articulos)
                    .HasForeignKey(d => d.IdCategoria)
                    .HasConstraintName("FK_Articulos_Categorias");
            });

            modelBuilder.Entity<ArticulosSolicitud>(entity =>
            {
                entity.HasKey(e => e.IdArticuloSolicitud);

                entity.Property(e => e.IdArticuloSolicitud)
                    .HasColumnName("id_articulo_solicitud");

                entity.Property(e => e.IdArticulo).HasColumnName("id_articulo");

                entity.Property(e => e.IdSolicitud).HasColumnName("id_solicitud");

                entity.HasOne(d => d.IdArticuloNavigation)
                    .WithMany(p => p.ArticulosSolicitud)
                    .HasForeignKey(d => d.IdArticulo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ArticulosSolicitud_Articulos");

                entity.HasOne(d => d.IdSolicitudNavigation)
                    .WithMany(p => p.ArticulosSolicitud)
                    .HasForeignKey(d => d.IdSolicitud)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ArticulosSolicitud_Solicitudes");

                entity.Property(e => e.Cantidad)
                    .HasColumnName("cantidad")
                    .HasColumnType("int");
            });

            modelBuilder.Entity<Categorias>(entity =>
            {
                entity.HasKey(e => e.IdCategoria);

                entity.Property(e => e.IdCategoria)
                    .HasColumnName("id_categoria")
                    .ValueGeneratedNever();

                entity.Property(e => e.Tipo)
                    .IsRequired()
                    .HasColumnName("tipo")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Solicitudes>(entity =>
            {
                entity.HasKey(e => e.IdSolicitud);

                entity.Property(e => e.IdSolicitud)
                    .HasColumnName("id_solicitud");

                entity.Property(e => e.EstadoSolicitud)
                    .IsRequired()
                    .HasColumnName("estado_solicitud")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FecCreacion)
                    .HasColumnName("fec_creacion")
                    .HasColumnType("datetime");

                entity.Property(e => e.IdUsuario)
                    .IsRequired()
                    .HasColumnName("id_usuario")
                    .HasMaxLength(450);

                entity.Property(e => e.EstadoAprobacion)
                    .HasColumnName("estado_aprobacion")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

        }

    }
}
