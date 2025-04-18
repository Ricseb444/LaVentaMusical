using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Models.Data
{
    public partial class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSets de tus entidades personalizadas
        public virtual DbSet<Albumes> Albumes { get; set; }
        public virtual DbSet<Artistas> Artistas { get; set; }
        public virtual DbSet<Canciones> Canciones { get; set; }
        public virtual DbSet<CarritoCompras> CarritoCompras { get; set; }
        public virtual DbSet<DetalleCarrito> DetalleCarrito { get; set; }
        public virtual DbSet<DetalleVentas> DetalleVentas { get; set; }
        public virtual DbSet<GenerosMusicales> GenerosMusicales { get; set; }
        public virtual DbSet<Perfiles> Perfiles { get; set; }
        public virtual DbSet<Usuario> Usuario { get; set; }
        public virtual DbSet<Ventas> Ventas { get; set; }
        public DbSet<ApplicationUser> applicationUsers { get; set; }
        //public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
#warning Revisa la cadena de conexión para no exponer información sensible.
            optionsBuilder.UseSqlServer("Server=DESKTOP-22VIRD6;Database=LaVentaMusical;User ID=sa;Password=Jebermo270700@;Trusted_Connection=true;Encrypt=false;MultipleActiveResultSets=true;Trust Server Certificate=True");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Primero se invoca la configuración por defecto de Identity
            base.OnModelCreating(modelBuilder);

            // Configuración de la entidad Albumes
            modelBuilder.Entity<Albumes>(entity =>
            {
                entity.HasKey(e => e.CodigoAlbum)
                      .HasName("PK__Albumes__A192F53D9753F21D");

                entity.Property(e => e.ImagenAlbum)
                      .HasMaxLength(255)
                      .IsUnicode(false);

                entity.Property(e => e.NombreAlbum)
                      .HasMaxLength(100)
                      .IsUnicode(false);

                entity.HasOne(d => d.CodigoArtistaNavigation)
                      .WithMany(p => p.Albumes)
                      .HasForeignKey(d => d.CodigoArtista)
                      .OnDelete(DeleteBehavior.ClientSetNull)
                      .HasConstraintName("FK__Albumes__CodigoA__31EC6D26");
            });

            // Configuración de la entidad Artistas
            modelBuilder.Entity<Artistas>(entity =>
            {
                entity.HasKey(e => e.CodigoArtista)
                      .HasName("PK__Artistas__1E9FC153A06508F2");

                entity.Property(e => e.FotoArtista)
                      .HasMaxLength(255)
                      .IsUnicode(false);

                entity.Property(e => e.LinkBiografia)
                      .HasMaxLength(255)
                      .IsUnicode(false);

                entity.Property(e => e.Nacionalidad)
                      .HasMaxLength(50)
                      .IsUnicode(false);

                entity.Property(e => e.NombreArtistico)
                      .HasMaxLength(100);

                entity.Property(e => e.NombreReal)
                      .HasMaxLength(100)
                      .IsUnicode(false);
            });

            // Si deseas agregar configuración adicional a las tablas de Identity puedes hacerlo.
            // En este ejemplo solo se configuran algunos índices y tamaños para IdentityUser.
            modelBuilder.Entity<ApplicationUser>(entity =>
            {
                entity.HasIndex(e => e.NormalizedEmail)
                      .HasDatabaseName("EmailIndex");

                entity.HasIndex(e => e.NormalizedUserName)
                      .HasDatabaseName("UserNameIndex")
                      .IsUnique()
                      .HasFilter("([NormalizedUserName] IS NOT NULL)");

                entity.Property(e => e.CorreoElectronico)
                      .HasMaxLength(256);

                entity.Property(e => e.NormalizedEmail)
                      .HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName)
                      .HasMaxLength(256);

                entity.Property(e => e.UserName)
                      .HasMaxLength(256);
            });

            // Configuración de la entidad Canciones
            modelBuilder.Entity<Canciones>(entity =>
            {
                entity.HasKey(e => e.CodigoCancion)
                      .HasName("PK__Cancione__E309E08A38BE660E");

                entity.Property(e => e.LinkVideo)
                      .HasMaxLength(255)
                      .IsUnicode(false);

                entity.Property(e => e.NombreCancion)
                      .HasMaxLength(100)
                      .IsUnicode(false);

                entity.Property(e => e.Precio)
                      .HasColumnType("decimal(10, 2)");

                entity.HasOne(d => d.CodigoAlbumNavigation)
                      .WithMany(p => p.Canciones)
                      .HasForeignKey(d => d.CodigoAlbum)
                      .OnDelete(DeleteBehavior.ClientSetNull)
                      .HasConstraintName("FK__Canciones__Codig__35BCFE0A");

                entity.HasOne(d => d.CodigoGeneroNavigation)
                      .WithMany(p => p.Canciones)
                      .HasForeignKey(d => d.CodigoGenero)
                      .OnDelete(DeleteBehavior.ClientSetNull)
                      .HasConstraintName("FK__Canciones__Codig__34C8D9D1");
            });

            // Configuración de la entidad CarritoCompras
            modelBuilder.Entity<CarritoCompras>(entity =>
            {
                entity.HasKey(e => e.IdCarrito)
                      .HasName("PK__CarritoC__8B4A618C6113C3DE");

                entity.Property(e => e.FechaCompra)
                      .HasDefaultValueSql("(getdate())")
                      .HasColumnType("datetime");

                entity.Property(e => e.Subtotal)
                      .HasColumnType("decimal(10, 2)");

                entity.Property(e => e.TipoPago)
                      .HasMaxLength(20)
                      .IsUnicode(false);

                entity.Property(e => e.Total)
                      .HasColumnType("decimal(10, 2)");

            });

            // Configuración de la entidad DetalleCarrito
            modelBuilder.Entity<DetalleCarrito>(entity =>
            {
                entity.HasKey(e => e.IdDetalleCarrito)
                      .HasName("PK__DetalleC__27A5F83BFEAFA3F6");

                entity.ToTable("DetalleCarrito");

                entity.Property(e => e.PrecioUnitario)
                      .HasColumnType("decimal(10, 2)");

                entity.Property(e => e.Total)
                      .HasColumnType("decimal(10, 2)");

                entity.HasOne(d => d.CodigoCancionNavigation)
                      .WithMany(p => p.DetalleCarritos)
                      .HasForeignKey(d => d.CodigoCancion)
                      .OnDelete(DeleteBehavior.ClientSetNull)
                      .HasConstraintName("FK__DetalleCa__Codig__4316F928");

                entity.HasOne(d => d.IdCarritoNavigation)
                      .WithMany(p => p.DetalleCarritos)
                      .HasForeignKey(d => d.IdCarrito)
                      .OnDelete(DeleteBehavior.ClientSetNull)
                      .HasConstraintName("FK__DetalleCa__IdCar__4222D4EF");
            });

            // Configuración de la entidad DetalleVentas
            modelBuilder.Entity<DetalleVentas>(entity =>
            {
                entity.HasKey(e => e.IdDetalleVenta)
                      .HasName("PK__DetalleV__AAA5CEC21F369556");

                entity.Property(e => e.PrecioUnitario)
                      .HasColumnType("decimal(10, 2)");

                entity.Property(e => e.Total)
                      .HasColumnType("decimal(10, 2)");

                entity.HasOne(d => d.CodigoCancionNavigation)
                      .WithMany(p => p.DetalleVenta)
                      .HasForeignKey(d => d.CodigoCancion)
                      .OnDelete(DeleteBehavior.ClientSetNull)
                      .HasConstraintName("FK__DetalleVe__Codig__46E78A0C");

                entity.HasOne(d => d.IdVentaNavigation)
                      .WithMany(p => p.DetalleVenta)
                      .HasForeignKey(d => d.IdVenta)
                      .OnDelete(DeleteBehavior.ClientSetNull)
                      .HasConstraintName("FK__DetalleVe__IdVen__45F365D3");
            });

            // Configuración de la entidad GenerosMusicales
            modelBuilder.Entity<GenerosMusicales>(entity =>
            {
                entity.HasKey(e => e.CodigoGenero)
                      .HasName("PK__GenerosM__4806D4DE20F67BD8");

                entity.Property(e => e.Descripcion)
                      .HasMaxLength(50)
                      .IsUnicode(false);

                entity.Property(e => e.FotoGenero)
                      .HasMaxLength(100);
            });

            // Configuración de la entidad Perfiles
            modelBuilder.Entity<Perfiles>(entity =>
            {
                entity.HasKey(e => e.IdPerfil)
                      .HasName("PK__Perfiles__C7BD5CC1CAECE20C");

                entity.Property(e => e.NombrePerfil)
                      .HasMaxLength(50)
                      .IsUnicode(false);
            });

            // Configuración de la entidad Usuarios
            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.IdUsuario)
                      .HasName("PK__Usuarios__5B65BF971D6D212C");

                entity.HasIndex(e => e.CorreoElectronico)
                      .IsUnique()
                      .HasDatabaseName("UQ__Usuarios__531402F345CE6B0B");

                entity.HasIndex(e => e.NumeroIdentificacion)
                      .IsUnique()
                      .HasDatabaseName("UQ__Usuarios__FCA68D91346B8688");

                entity.Property(e => e.Contraseña)
                      .HasMaxLength(255)
                      .IsUnicode(false);

                entity.Property(e => e.CorreoElectronico)
                      .HasMaxLength(100)
                      .IsUnicode(false);

                entity.Property(e => e.DineroDisponible)
                      .HasColumnType("decimal(10, 2)");

                entity.Property(e => e.Genero)
                      .HasMaxLength(10)
                      .IsUnicode(false);

                entity.Property(e => e.NombreCompleto)
                      .HasMaxLength(100)
                      .IsUnicode(false);

                entity.Property(e => e.NumeroIdentificacion)
                      .HasMaxLength(20)
                      .IsUnicode(false);

                entity.Property(e => e.NumeroTarjeta)
                      .HasMaxLength(19)
                      .IsUnicode(false);

                entity.Property(e => e.TipoTarjeta)
                      .HasMaxLength(20)
                      .IsUnicode(false);

                entity.HasOne(d => d.IdPerfilNavigation)
                      .WithMany(p => p.Usuarios)
                      .HasForeignKey(d => d.IdPerfil)
                      .OnDelete(DeleteBehavior.ClientSetNull)
                      .HasConstraintName("FK__Usuarios__IdPerf__2B3F6F97");
            });

            // Configuración de la entidad Ventas
            modelBuilder.Entity<Ventas>(entity =>
            {
                entity.HasKey(e => e.IdVenta)
                      .HasName("PK__Ventas__BC1240BD0B8C9999");

                entity.Property(e => e.FechaCompra)
                      .HasDefaultValueSql("(getdate())")
                      .HasColumnType("datetime");

                entity.Property(e => e.Subtotal)
                      .HasColumnType("decimal(10, 2)");

                entity.Property(e => e.TipoPago)
                      .HasMaxLength(20)
                      .IsUnicode(false);

                entity.Property(e => e.Total)
                      .HasColumnType("decimal(10, 2)");

                //entity.HasOne(d => d.IdUsuarioNavigation)
                //      .WithMany(p => p.Venta)
                //      .HasForeignKey(d => d.IdUsuario)
                //      .OnDelete(DeleteBehavior.ClientSetNull)
                //      .HasConstraintName("FK__Ventas__IdUsuari__3D5E1FD2");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
