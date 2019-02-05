using Homefind.Core.DomainModels;
using Microsoft.EntityFrameworkCore;

namespace Homefind.Infrastructure.Repository
{
    public partial class EstateDbContext : DbContext
    {
        public EstateDbContext(DbContextOptions<EstateDbContext> options) : base(options)
        {

        }

        public virtual DbSet<EstateFeature> EstateFeature { get; set; }
        public virtual DbSet<EstateImage> EstateImage { get; set; }
        public virtual DbSet<EstateLocation> EstateLocation { get; set; }
        public virtual DbSet<EstateType> EstateType { get; set; }
        public virtual DbSet<EstateUnit> EstateUnit { get; set; }
        public virtual DbSet<Favourites> Favourites { get; set; }
        public virtual DbSet<Review> Reviews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EstateFeature>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.ArePetsAllowed).HasColumnType("bit");

                entity.Property(e => e.HasAirConditioning).HasColumnType("bit");

                entity.Property(e => e.HasCarParking).HasColumnType("bit");

                entity.Property(e => e.HasInternet).HasColumnType("bit");

                entity.Property(e => e.HasSwimmingPool).HasColumnType("bit");

                entity.Property(e => e.HasTv)
                    .HasColumnName("HasTV")
                    .HasColumnType("bit");

                entity.Property(e => e.IsFurnished).HasColumnType("bit");

                entity.HasOne<EstateUnit>()
                    .WithOne()
                    .HasForeignKey<EstateFeature>(d => d.Id);
            });

            modelBuilder.Entity<EstateImage>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.Data)
                    .IsRequired()
                    .HasColumnType("varbinary(max)");

                entity.Property(e => e.Width)
                    .IsRequired()
                    .HasColumnType("int");

                entity.Property(e => e.Height)
                    .IsRequired()
                    .HasColumnType("int");

                entity.Property(e => e.ContentType)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.HasOne<EstateUnit>()
                    .WithMany(d => d.EstateImages)
                    .HasForeignKey(d => d.EstateUnitId);
            });

            modelBuilder.Entity<EstateLocation>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Address).HasMaxLength(250);

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Country)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.State)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ZipCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne<EstateUnit>()
                    .WithOne()
                    .HasForeignKey<EstateLocation>(d => d.Id);
            });

            modelBuilder.Entity<EstateType>(entity =>
            {
                entity.Property(e => e.TypeName)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<EstateUnit>(entity =>
            {
                entity.Property(e => e.DateAvailable).HasColumnType("date");

                entity.Property(e => e.DatePosted).HasColumnType("date");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnType("text");

                entity.Property(e => e.PostedBy)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasColumnType("char(1)");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.EstateType)
                    .WithMany()
                    .HasForeignKey(d => d.EstateTypeId);

                entity.HasMany(d => d.EstateImages)
                    .WithOne()
                    .HasForeignKey(d => d.EstateUnitId);

                entity.HasOne(d => d.EstateFeature)
                    .WithOne()
                    .HasForeignKey<EstateFeature>(d => d.Id);

                entity.HasOne(d => d.EstateLocation)
                    .WithOne()
                    .HasForeignKey<EstateLocation>(d => d.Id);
            });

            modelBuilder.Entity<Favourites>(entity =>
            {
                entity.HasOne(d => d.EstateUnit)
                    .WithMany()
                    .HasForeignKey(d => d.EstateUnitId);
            });

            modelBuilder.Entity<Review>();
        }
    }
}
