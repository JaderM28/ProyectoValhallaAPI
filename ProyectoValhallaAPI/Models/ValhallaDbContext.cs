using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ProyectoValhallaAPI.Models;

public partial class ValhallaDbContext : DbContext
{
    public ValhallaDbContext()
    {
    }

    public ValhallaDbContext(DbContextOptions<ValhallaDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<HistorialRefreshToken> HistorialRefreshTokens { get; set; }

    public virtual DbSet<Permiso> Permisos { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<RolesPermiso> RolesPermisos { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<HistorialRefreshToken>(entity =>
        {
            entity.HasKey(e => e.IdHistorial).HasName("PK__Historia__9CC7DBB4C0889607");

            entity.ToTable("HistorialRefreshToken");

            entity.Property(e => e.EsActivo).HasComputedColumnSql("(case when [FechaExpiracion]<getdate() then CONVERT([bit],(0)) else CONVERT([bit],(1)) end)", false);
            entity.Property(e => e.FechaCreacion).HasColumnType("datetime");
            entity.Property(e => e.FechaExpiracion).HasColumnType("datetime");
            entity.Property(e => e.RefreshToken).IsUnicode(false);
            entity.Property(e => e.Token).IsUnicode(false);

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.HistorialRefreshTokens)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK__Historial__IdUsu__571DF1D5");
        });

        modelBuilder.Entity<Permiso>(entity =>
        {
            entity.HasKey(e => e.IdPermiso).HasName("PK__Permisos__0D626EC8EB833373");

            entity.Property(e => e.NombreModulo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.NombrePermiso)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.IdRol).HasName("PK__Roles__2A49584CFDAF6144");

            entity.Property(e => e.Descripcion).HasColumnType("text");
            entity.Property(e => e.NombreRol)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<RolesPermiso>(entity =>
        {
            entity.HasKey(e => e.IdRolPermiso).HasName("PK__Roles_Pe__1F805E913D765B5C");

            entity.ToTable("Roles_Permisos");

            entity.Property(e => e.IdRolPermiso).HasColumnName("IdRol_Permiso");

            entity.HasOne(d => d.IdPermisoNavigation).WithMany(p => p.RolesPermisos)
                .HasForeignKey(d => d.IdPermiso)
                .HasConstraintName("FK__Roles_Per__IdPer__4D94879B");

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.RolesPermisos)
                .HasForeignKey(d => d.IdRol)
                .HasConstraintName("FK__Roles_Per__IdRol__4E88ABD4");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PK__Usuarios__5B65BF97B15E779A");

            entity.Property(e => e.Apellidos)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CorreoElectronico)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Nombres)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Telefono)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdRol)
                .HasConstraintName("FK__Usuarios__IdRol__5441852A");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
