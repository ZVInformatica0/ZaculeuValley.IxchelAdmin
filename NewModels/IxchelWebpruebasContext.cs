using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ZaculeuValley.IxchelAdmin.NewModels;

public partial class IxchelWebpruebasContext : DbContext
{
    public IxchelWebpruebasContext()
    {
    }

    public IxchelWebpruebasContext(DbContextOptions<IxchelWebpruebasContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Permission> Permissions { get; set; }

    public virtual DbSet<PermissionRol> PermissionRols { get; set; }

    public virtual DbSet<Rol> Rols { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserRol> UserRols { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=ixchelserver.database.windows.net; Initial Catalog=IxchelWEBPruebas; User=IxchelDBAdmin; Password=dbAdmin!0;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Permission>(entity =>
        {
            entity.HasKey(e => e.Idpermission);

            entity.ToTable("Permission");

            entity.Property(e => e.Idpermission).HasColumnName("IDPermission");
            entity.Property(e => e.PermissionDescription)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PermissionName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<PermissionRol>(entity =>
        {
            entity.HasKey(e => e.IdpermissionRol);

            entity.ToTable("PermissionRol");

            entity.Property(e => e.IdpermissionRol).HasColumnName("IDPermissionRol");
            entity.Property(e => e.Idpermission).HasColumnName("IDPermission");
            entity.Property(e => e.Idrol).HasColumnName("IDRol");

            entity.HasOne(d => d.IdpermissionNavigation).WithMany(p => p.PermissionRols)
                .HasForeignKey(d => d.Idpermission)
                .HasConstraintName("FK_PermissionRol_IdPermission");

            entity.HasOne(d => d.IdrolNavigation).WithMany(p => p.PermissionRols)
                .HasForeignKey(d => d.Idrol)
                .HasConstraintName("FK_PermissionRol_IdRol");
        });

        modelBuilder.Entity<Rol>(entity =>
        {
            entity.HasKey(e => e.Idrol);

            entity.ToTable("Rol");

            entity.Property(e => e.Idrol).HasColumnName("IDRol");
            entity.Property(e => e.RolDescription)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.RolName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Iduser);

            entity.ToTable("User");

            entity.Property(e => e.Iduser).HasColumnName("IDUser");
            entity.Property(e => e.UserName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UserPassword)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<UserRol>(entity =>
        {
            entity.HasKey(e => e.IduserRol);

            entity.ToTable("UserRol");

            entity.Property(e => e.IduserRol).HasColumnName("IDUserRol");
            entity.Property(e => e.Idrol).HasColumnName("IDRol");
            entity.Property(e => e.Iduser).HasColumnName("IDUser");

            entity.HasOne(d => d.IdrolNavigation).WithMany(p => p.UserRols)
                .HasForeignKey(d => d.Idrol)
                .HasConstraintName("FK_UserRol_IDRol");

            entity.HasOne(d => d.IduserNavigation).WithMany(p => p.UserRols)
                .HasForeignKey(d => d.Iduser)
                .HasConstraintName("FK_UserRol_IDUser");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
