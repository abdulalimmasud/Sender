using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace AstManagement.AssetDB
{
    public partial class EyeAssetDbContext : DbContext
    {
        private readonly string _connectionString;
        public EyeAssetDbContext()
        {
        }

        public EyeAssetDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public EyeAssetDbContext(DbContextOptions<EyeAssetDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<App> Apps { get; set; }
        public virtual DbSet<Attachment> Attachments { get; set; }
        public virtual DbSet<AttachmentLog> AttachmentLogs { get; set; }
        public virtual DbSet<MobileOperator> MobileOperators { get; set; }
        public virtual DbSet<MobileOperatorPackage> MobileOperatorPackages { get; set; }
        public virtual DbSet<SimCard> SimCards { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<App>(entity =>
            {
                entity.ToTable("App", "mng");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Attachment>(entity =>
            {
                entity.HasKey(e => e.SimCardId)
                    .HasName("PK_Sim_Attachment");

                entity.ToTable("Attachment", "sim");

                entity.Property(e => e.SimCardId).ValueGeneratedNever();

                entity.Property(e => e.AttachmentTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.App)
                    .WithMany(p => p.Attachments)
                    .HasForeignKey(d => d.AppId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Sim_Attachment_App");

                entity.HasOne(d => d.SimCard)
                    .WithOne(p => p.Attachment)
                    .HasForeignKey<Attachment>(d => d.SimCardId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Sim_SimCard");
            });

            modelBuilder.Entity<AttachmentLog>(entity =>
            {
                entity.ToTable("AttachmentLog", "sim");

                entity.Property(e => e.LogTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.App)
                    .WithMany(p => p.AttachmentLogs)
                    .HasForeignKey(d => d.AppId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Sim_AttachmentLog_App");
            });

            modelBuilder.Entity<MobileOperator>(entity =>
            {
                entity.ToTable("MobileOperator", "sim");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreationTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<MobileOperatorPackage>(entity =>
            {
                entity.ToTable("MobileOperatorPackages", "sim");

                entity.Property(e => e.CreationTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Description)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.Operator)
                    .WithMany(p => p.MobileOperatorPackages)
                    .HasForeignKey(d => d.OperatorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Sim_MobileOperatorPackages_Operator");
            });

            modelBuilder.Entity<SimCard>(entity =>
            {
                entity.ToTable("SimCard", "sim");

                entity.HasIndex(e => e.MobileNumber, "UC_SimCard_MobileNumber")
                    .IsUnique();

                entity.HasIndex(e => e.SimNumber, "UC_SimCard_SimNumber")
                    .IsUnique();

                entity.Property(e => e.CreationTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.MobileNumber)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.SimNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UpdateByUser)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UpdateTime).HasColumnType("datetime");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.Operator)
                    .WithMany(p => p.SimCards)
                    .HasForeignKey(d => d.OperatorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Sim_SimCard_Operator");

                entity.HasOne(d => d.OperatorPackage)
                    .WithMany(p => p.SimCards)
                    .HasForeignKey(d => d.OperatorPackageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Sim_SimCard_OperatorPackage");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User", "mng");

                entity.Property(e => e.CreationTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.PasswordHash).IsRequired();

                entity.Property(e => e.PasswordSalt).IsRequired();

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
