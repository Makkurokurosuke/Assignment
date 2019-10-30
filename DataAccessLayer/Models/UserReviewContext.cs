using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Configuration;
using Common.Utility;

namespace DataAccessLayer.Models
{
    public partial class UserReviewContext : DbContext
    {
        public UserReviewContext()
        {
        }

        public UserReviewContext(DbContextOptions<UserReviewContext> options)
            : base(options)
        {
        }

        public virtual DbSet<RatingTypes> RatingTypes { get; set; }
        public virtual DbSet<Reviews> Reviews { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(AppSettingsConfigurationManager.AppSetting["ConnectionStrings:DefaultConnection"]);

            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.4-servicing-10062");

            modelBuilder.Entity<RatingTypes>(entity =>
            {
                entity.ToTable("RatingTypes", "UserReview");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.EnglishDesc)
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.FrenchDesc)
                    .HasMaxLength(40)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Reviews>(entity =>
            {
                entity.ToTable("Reviews", "UserReview");

                entity.Property(e => e.Comment)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.RatingType)
                    .WithMany(p => p.Reviews)
                    .HasForeignKey(d => d.RatingTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Reviews__RatingT__58D1301D");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Reviews)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Reviews__UserId__59C55456");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.ToTable("Users", "UserReview");

                entity.HasIndex(e => e.Username)
                    .HasName("UQ__Users__536C85E4C033849E")
                    .IsUnique();

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.StoppedDate).HasColumnType("datetime");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });
        }
    }
}
