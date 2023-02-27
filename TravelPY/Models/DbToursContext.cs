using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TravelPY.Models;

public partial class DbToursContext : DbContext
{
    public DbToursContext()
    {
    }

    public DbToursContext(DbContextOptions<DbToursContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BaiViet> BaiViets { get; set; }

    public virtual DbSet<ChiTietDatTour> ChiTietDatTours { get; set; }

    public virtual DbSet<DanhMuc> DanhMucs { get; set; }

    public virtual DbSet<DatTour> DatTours { get; set; }

    public virtual DbSet<HuongDanVien> HuongDanViens { get; set; }

    public virtual DbSet<KhachHang> KhachHangs { get; set; }

    public virtual DbSet<KhachSan> KhachSans { get; set; }

    public virtual DbSet<Location> Locations { get; set; }

    public virtual DbSet<Page> Pages { get; set; }

    public virtual DbSet<TaiKhoan> TaiKhoans { get; set; }

    public virtual DbSet<Tour> Tours { get; set; }

    public virtual DbSet<TrangThai> TrangThais { get; set; }

    public virtual DbSet<VaiTro> VaiTros { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=NGOCDUNG;Initial Catalog=dbTours;Integrated Security=True; Encrypt=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BaiViet>(entity =>
        {
            entity.HasKey(e => e.MaBaiViet).HasName("PK_BaiViets");

            entity.ToTable("BaiViet");

            entity.Property(e => e.Alias).HasMaxLength(255);
            entity.Property(e => e.HinhAnh).HasMaxLength(255);
            entity.Property(e => e.NgayTao).HasColumnType("datetime");
            entity.Property(e => e.TieuDe).HasMaxLength(255);

            entity.HasOne(d => d.MaPageNavigation).WithMany(p => p.BaiViets)
                .HasForeignKey(d => d.MaPage)
                .HasConstraintName("FK_BaiViet_Page");

            entity.HasOne(d => d.MaTaiKhoanNavigation).WithMany(p => p.BaiViets)
                .HasForeignKey(d => d.MaTaiKhoan)
                .HasConstraintName("FK_BaiViet_TaiKhoan");
        });

        modelBuilder.Entity<ChiTietDatTour>(entity =>
        {
            entity.HasKey(e => e.MaChiTiet);

            entity.ToTable("ChiTietDatTour");

            entity.Property(e => e.NgayTao).HasColumnType("datetime");

            entity.HasOne(d => d.MaDatTourNavigation).WithMany(p => p.ChiTietDatTours)
                .HasForeignKey(d => d.MaDatTour)
                .HasConstraintName("FK_ChiTietDatTour_DatTour");

            entity.HasOne(d => d.MaTourNavigation).WithMany(p => p.ChiTietDatTours)
                .HasForeignKey(d => d.MaTour)
                .HasConstraintName("FK_ChiTietDatTour_Tour");
        });

        modelBuilder.Entity<DanhMuc>(entity =>
        {
            entity.HasKey(e => e.MaDanhMuc).HasName("PK_DanhMucs");

            entity.ToTable("DanhMuc");

            entity.Property(e => e.Alias).HasMaxLength(255);
            entity.Property(e => e.HinhAnh).HasMaxLength(255);
            entity.Property(e => e.TenDanhMuc).HasMaxLength(50);
        });

        modelBuilder.Entity<DatTour>(entity =>
        {
            entity.HasKey(e => e.MaDatTour);

            entity.ToTable("DatTour");

            entity.Property(e => e.LocationId).HasColumnName("LocationID");
            entity.Property(e => e.NgayDatTour).HasColumnType("datetime");
            entity.Property(e => e.NgayDi).HasColumnType("datetime");
            entity.Property(e => e.NgayThanhToan).HasColumnType("datetime");

            entity.HasOne(d => d.MaKhachHangNavigation).WithMany(p => p.DatTours)
                .HasForeignKey(d => d.MaKhachHang)
                .HasConstraintName("FK_DatTour_KhachHang");

            entity.HasOne(d => d.MaTrangThaiNavigation).WithMany(p => p.DatTours)
                .HasForeignKey(d => d.MaTrangThai)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DatTour_TrangThai");
        });

        modelBuilder.Entity<HuongDanVien>(entity =>
        {
            entity.HasKey(e => e.MaHdv).HasName("PK_HuongDanViens");

            entity.ToTable("HuongDanVien");

            entity.Property(e => e.MaHdv).HasColumnName("MaHDV");
            entity.Property(e => e.DiaChiHdv)
                .HasMaxLength(255)
                .HasColumnName("DiaChiHDV");
            entity.Property(e => e.HinhAnh).HasMaxLength(255);
            entity.Property(e => e.NgaySinhHdv)
                .HasColumnType("date")
                .HasColumnName("NgaySinhHDV");
            entity.Property(e => e.Sdt)
                .HasMaxLength(12)
                .IsFixedLength()
                .HasColumnName("SDT");
            entity.Property(e => e.TenHdv)
                .HasMaxLength(50)
                .HasColumnName("TenHDV");
        });

        modelBuilder.Entity<KhachHang>(entity =>
        {
            entity.HasKey(e => e.MaKhachHang);

            entity.ToTable("KhachHang");

            entity.Property(e => e.Avatar).HasMaxLength(255);
            entity.Property(e => e.DiaChi).HasMaxLength(50);
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.LastLogin).HasColumnType("datetime");
            entity.Property(e => e.LocationId).HasColumnName("LocationID");
            entity.Property(e => e.MatKhau).HasMaxLength(50);
            entity.Property(e => e.NgaySinh).HasColumnType("datetime");
            entity.Property(e => e.NgayTao).HasColumnType("datetime");
            entity.Property(e => e.Salt)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.Sdt)
                .HasMaxLength(12)
                .IsFixedLength()
                .HasColumnName("SDT");
            entity.Property(e => e.TenKhachHang).HasMaxLength(255);

            entity.HasOne(d => d.Location).WithMany(p => p.KhachHangs)
                .HasForeignKey(d => d.LocationId)
                .HasConstraintName("FK_KhachHang_Locations");
        });

        modelBuilder.Entity<KhachSan>(entity =>
        {
            entity.HasKey(e => e.MaKhachSan);

            entity.ToTable("KhachSan");

            entity.Property(e => e.DiaChi).HasMaxLength(255);
            entity.Property(e => e.TenKhachSan).HasMaxLength(255);
        });

        modelBuilder.Entity<Location>(entity =>
        {
            entity.Property(e => e.LocationId).HasColumnName("LocationID");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.NameWithType).HasMaxLength(100);
            entity.Property(e => e.Slug).HasMaxLength(100);
            entity.Property(e => e.Type).HasMaxLength(10);
        });

        modelBuilder.Entity<Page>(entity =>
        {
            entity.HasKey(e => e.MaPage).HasName("PK_Pages");

            entity.ToTable("Page");

            entity.Property(e => e.Alias).HasMaxLength(255);
            entity.Property(e => e.HinhAnh).HasMaxLength(255);
            entity.Property(e => e.TenPage).HasMaxLength(255);
        });

        modelBuilder.Entity<TaiKhoan>(entity =>
        {
            entity.HasKey(e => e.MaTaiKhoan).HasName("PK_TaiKhoans");

            entity.ToTable("TaiKhoan");

            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.LastLogin).HasColumnType("datetime");
            entity.Property(e => e.MatKhau).HasMaxLength(50);
            entity.Property(e => e.NgayTao).HasColumnType("datetime");
            entity.Property(e => e.Salt)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.Sdt)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("SDT");
            entity.Property(e => e.TenTaiKhoan).HasMaxLength(255);

            entity.HasOne(d => d.MaVaiTroNavigation).WithMany(p => p.TaiKhoans)
                .HasForeignKey(d => d.MaVaiTro)
                .HasConstraintName("FK_TaiKhoan_VaiTro");
        });

        modelBuilder.Entity<Tour>(entity =>
        {
            entity.HasKey(e => e.MaTour).HasName("PK_Tours");

            entity.ToTable("Tour");

            entity.Property(e => e.Alias).HasMaxLength(255);
            entity.Property(e => e.HinhAnh).HasMaxLength(255);
            entity.Property(e => e.MaHdv).HasColumnName("MaHDV");
            entity.Property(e => e.NgayKhoiHanh).HasMaxLength(255);
            entity.Property(e => e.NoiKhoiHanh).HasMaxLength(255);
            entity.Property(e => e.PhuongTien).HasMaxLength(50);
            entity.Property(e => e.SoNgay).HasMaxLength(50);
            entity.Property(e => e.TenTour).HasMaxLength(255);

            entity.HasOne(d => d.MaDanhMucNavigation).WithMany(p => p.Tours)
                .HasForeignKey(d => d.MaDanhMuc)
                .HasConstraintName("FK_Tour_DanhMuc");

            entity.HasOne(d => d.MaHdvNavigation).WithMany(p => p.Tours)
                .HasForeignKey(d => d.MaHdv)
                .HasConstraintName("FK_Tour_HuongDanVien");
        });

        modelBuilder.Entity<TrangThai>(entity =>
        {
            entity.HasKey(e => e.MaTrangThai);

            entity.ToTable("TrangThai");

            entity.Property(e => e.TenTrangThai).HasMaxLength(50);
        });

        modelBuilder.Entity<VaiTro>(entity =>
        {
            entity.HasKey(e => e.MaVaiTro).HasName("PK_VaiTros");

            entity.ToTable("VaiTro");

            entity.Property(e => e.TenVaiTro).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
