using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using ZaculeuValley.IxchelAdmin.NewModels;

namespace ZaculeuValley.IxchelAdmin.Models;

public partial class IxchelWebpruebasContext : DbContext
{
    public IxchelWebpruebasContext()
    {
    }

    public IxchelWebpruebasContext(DbContextOptions<IxchelWebpruebasContext> options)
        : base(options)
    {
    }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserRol> UserRols { get; set; }

    public virtual DbSet<Facility> Facilities { get; set; }

    public virtual DbSet<FacilityType> FacilityTypes { get; set; }

    public virtual DbSet<Institution> Institutions { get; set; }

    public virtual DbSet<InstitutionArea> InstitutionAreas { get; set; }

    public virtual DbSet<InstitutionCountry> InstitutionCountries { get; set; }

    public virtual DbSet<InstitutionDistrict> InstitutionDistricts { get; set; }

    public virtual DbSet<Log> Logs { get; set; }

    public virtual DbSet<LogError> LogErrors { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//quitar luego de aqui la connectionstring
        => optionsBuilder.UseSqlServer("Data Source=ixchelserver.database.windows.net; Initial catalog=IxchelWEBPruebas; user=IxchelDBAdmin; password=dbAdmin!0;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Facility>(entity =>
        {
            entity.HasKey(e => e.Idfacility);

            entity.ToTable("Facility");

            entity.Property(e => e.Idfacility)
                .HasComment("Id for table Facility")
                .HasColumnName("IDFacility");
            entity.Property(e => e.AreaCode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("Internal code id for area");
            entity.Property(e => e.Deleted).HasComment("State of delete of a Facility for Facility Table");
            entity.Property(e => e.DistrictCode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("Internal code id for district");
            entity.Property(e => e.Enabled).HasComment("State of facility for Facility table");
            entity.Property(e => e.FacilityCode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("Internal Code for table Facility");
            entity.Property(e => e.FacilityName)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasComment("Name for Facility table");
            entity.Property(e => e.Iddistrict)
                .HasComment("Id for District (Foreign key) for table Facility")
                .HasColumnName("IDDistrict");
            entity.Property(e => e.IdfacilityType)
                .HasComment("Id for type of facility for Facility table")
                .HasColumnName("IDFacilityType");
            entity.Property(e => e.Idinstitution)
                .HasComment("Id Foreign for Institution for Facility Table")
                .HasColumnName("IDInstitution");

            entity.HasOne(d => d.IddistrictNavigation).WithMany(p => p.Facilities)
                .HasForeignKey(d => d.Iddistrict)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Facility_InstitutionDistrict");

            entity.HasOne(d => d.IdfacilityTypeNavigation).WithMany(p => p.Facilities)
                .HasForeignKey(d => d.IdfacilityType)
                .HasConstraintName("FK_Facility_FacilityType");

            entity.HasOne(d => d.IdinstitutionNavigation).WithMany(p => p.Facilities)
                .HasForeignKey(d => d.Idinstitution)
                .HasConstraintName("FK_Facility_Institution");
        });

        modelBuilder.Entity<FacilityType>(entity =>
        {
            entity.HasKey(e => e.IdfacilityType);

            entity.ToTable("FacilityType");

            entity.Property(e => e.IdfacilityType)
                .HasComment("ID of facility type for Table FacilityType")
                .HasColumnName("IDFacilityType");
            entity.Property(e => e.Deleted).HasComment("State of delete of a Facility for FacilityType Table");
            entity.Property(e => e.Enabled).HasComment("State of facility for FacilityType table");
            entity.Property(e => e.FacilityTypeCode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("Internal code for Facility Type (diff. each institution)");
            entity.Property(e => e.FacilityTypeName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("Facility type name for table FacilityTypeName");
            entity.Property(e => e.Idinstitution)
                .HasComment("Foreign key for Institution on FacilityType table")
                .HasColumnName("IDInstitution");

            entity.HasOne(d => d.IdinstitutionNavigation).WithMany(p => p.FacilityTypes)
                .HasForeignKey(d => d.Idinstitution)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FacilityType_Institution");
        });

        modelBuilder.Entity<Institution>(entity =>
        {
            entity.HasKey(e => e.Idinstitution);

            entity.ToTable("Institution");

            entity.Property(e => e.Idinstitution)
                .HasComment("Id fot table Institution")
                .HasColumnName("IDInstitution");
            entity.Property(e => e.InstitutionCode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("Internal code for table Institution");
            entity.Property(e => e.InstitutionName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("Name for table Institution");
        });

        modelBuilder.Entity<InstitutionArea>(entity =>
        {
            entity.HasKey(e => e.IdinstitutionArea);

            entity.ToTable("InstitutionArea");

            entity.Property(e => e.IdinstitutionArea)
                .HasComment("Id for table Institution Area")
                .HasColumnName("IDInstitutionArea");
            entity.Property(e => e.AreaCode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("Internal code for Table Institution Area");
            entity.Property(e => e.AreaName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasComment("Name of the area for table Institution Area");
            entity.Property(e => e.Idinstitution)
                .HasComment("Id Foreign for Institution for Area Table")
                .HasColumnName("IDInstitution");
            entity.Property(e => e.IdinstitutionCountry)
                .HasComment("Id for table Institution Country (Foreign Key)")
                .HasColumnName("IDInstitutionCountry");

            entity.HasOne(d => d.IdinstitutionNavigation).WithMany(p => p.InstitutionAreas)
                .HasForeignKey(d => d.Idinstitution)
                .HasConstraintName("FK_InstitutionArea_Institution1");

            entity.HasOne(d => d.IdinstitutionCountryNavigation).WithMany(p => p.InstitutionAreas)
                .HasForeignKey(d => d.IdinstitutionCountry)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InstitutionArea_Institution");
        });

        modelBuilder.Entity<InstitutionCountry>(entity =>
        {
            entity.HasKey(e => e.IdinstitutionCountry);

            entity.ToTable("InstitutionCountry");

            entity.Property(e => e.IdinstitutionCountry)
                .HasComment("Id for the table Institution Country")
                .HasColumnName("IDInstitutionCountry");
            entity.Property(e => e.CountryDomainName)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasComment("Domain ectention for the table Institution Country");
            entity.Property(e => e.CountryName).HasComment("Name for Table Institution Country");
            entity.Property(e => e.CountryPhoneCode)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasComment("Phone code for Table Institution Country");
            entity.Property(e => e.Idinstitution)
                .HasComment("Id for table Institution Country (Foreign Key)")
                .HasColumnName("IDInstitution");

            entity.HasOne(d => d.IdinstitutionNavigation).WithMany(p => p.InstitutionCountries)
                .HasForeignKey(d => d.Idinstitution)
                .HasConstraintName("FK_InstitutionCountry_Institution");
        });

        modelBuilder.Entity<InstitutionDistrict>(entity =>
        {
            entity.HasKey(e => e.IdinstitutionDistrict);

            entity.ToTable("InstitutionDistrict");

            entity.Property(e => e.IdinstitutionDistrict)
                .HasComment("Id for table Institution District")
                .HasColumnName("IDInstitutionDistrict");
            entity.Property(e => e.AreaCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.DistrictCode).HasComment("Internal code for table Institution District");
            entity.Property(e => e.DistrictName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasComment("Name for table Institution District");
            entity.Property(e => e.Idinstitution)
                .HasComment("Id Foreign for Institution for DistrictTable")
                .HasColumnName("IDInstitution");
            entity.Property(e => e.IdinstitutionArea)
                .HasComment("Id for table Institution District (Foreign Key)")
                .HasColumnName("IDInstitutionArea");

            entity.HasOne(d => d.IdinstitutionNavigation).WithMany(p => p.InstitutionDistricts)
                .HasForeignKey(d => d.Idinstitution)
                .HasConstraintName("FK_InstitutionDistrict_Institution");

            entity.HasOne(d => d.IdinstitutionAreaNavigation).WithMany(p => p.InstitutionDistricts)
                .HasForeignKey(d => d.IdinstitutionArea)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InstitutionDistrict_InstitutionArea");
        });

        modelBuilder.Entity<Log>(entity =>
        {
            entity.HasKey(e => e.Idlog);

            entity.ToTable("Log");

            entity.Property(e => e.Idlog).HasColumnName("idlog");
            entity.Property(e => e.Action)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AlterationDate).HasColumnType("datetime");
            entity.Property(e => e.AlteredTable)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Message).IsUnicode(false);
        });

        modelBuilder.Entity<LogError>(entity =>
        {
            entity.HasKey(e => e.Idlogerror);

            entity.ToTable("log_error");

            entity.Property(e => e.Idlogerror).HasColumnName("idlogerror");
            entity.Property(e => e.Class)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("class");
            entity.Property(e => e.Datetime)
                .HasColumnType("datetime")
                .HasColumnName("datetime");
            entity.Property(e => e.ExMessage)
                .IsUnicode(false)
                .HasColumnName("ex_message");
            entity.Property(e => e.ExStacktrace)
                .IsUnicode(false)
                .HasColumnName("ex_stacktrace");
            entity.Property(e => e.Function)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("function");
            entity.Property(e => e.Message)
                .IsUnicode(false)
                .HasColumnName("message");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Iduser);

            entity.ToTable("User");

            entity.Property(e => e.Iduser).HasColumnName("IDUser");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<UserRol>(entity =>
        {
            entity.HasKey(e => e.IduserRol);

            entity.ToTable("UserRol");

            entity.Property(e => e.IduserRol).HasColumnName("IDUserRol");
            entity.Property(e => e.Iduser).HasColumnName("IDUser");
            entity.Property(e => e.UserRolDescription)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UserRolName)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.IduserNavigation).WithMany(p => p.UserRols)
                .HasForeignKey(d => d.Iduser)
                .HasConstraintName("FK_UserRol_User");
        });


        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
