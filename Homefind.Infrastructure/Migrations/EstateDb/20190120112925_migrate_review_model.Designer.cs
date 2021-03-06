﻿// <auto-generated />
using System;
using Homefind.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Homefind.Infrastructure.Migrations.EstateDb
{
    [DbContext(typeof(EstateDbContext))]
    [Migration("20190120112925_migrate_review_model")]
    partial class migrate_review_model
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Homefind.Core.DomainModels.EstateFeature", b =>
                {
                    b.Property<int>("Id");

                    b.Property<bool>("ArePetsAllowed")
                        .HasColumnType("bit");

                    b.Property<bool>("HasAirConditioning")
                        .HasColumnType("bit");

                    b.Property<bool>("HasCarParking")
                        .HasColumnType("bit");

                    b.Property<bool>("HasInternet")
                        .HasColumnType("bit");

                    b.Property<bool>("HasSwimmingPool")
                        .HasColumnType("bit");

                    b.Property<bool>("HasTv")
                        .HasColumnName("HasTV")
                        .HasColumnType("bit");

                    b.Property<bool>("IsFurnished")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("EstateFeature");
                });

            modelBuilder.Entity("Homefind.Core.DomainModels.EstateImage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ContentType")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<byte[]>("Data")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<int>("EstateUnitId");

                    b.Property<int>("Height")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<int>("Width")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("EstateUnitId");

                    b.ToTable("EstateImage");
                });

            modelBuilder.Entity("Homefind.Core.DomainModels.EstateLocation", b =>
                {
                    b.Property<int>("Id");

                    b.Property<string>("Address")
                        .HasMaxLength(250);

                    b.Property<string>("City")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<string>("Country")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<string>("State")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<string>("ZipCode")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.HasKey("Id");

                    b.ToTable("EstateLocation");
                });

            modelBuilder.Entity("Homefind.Core.DomainModels.EstateType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("TypeName")
                        .IsRequired()
                        .HasMaxLength(20)
                        .IsUnicode(false);

                    b.HasKey("Id");

                    b.ToTable("EstateType");
                });

            modelBuilder.Entity("Homefind.Core.DomainModels.EstateUnit", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Balconies");

                    b.Property<int>("Bathrooms");

                    b.Property<int>("Bedrooms");

                    b.Property<int>("CarpetArea");

                    b.Property<DateTime>("DateAvailable")
                        .HasColumnType("date");

                    b.Property<DateTime>("DatePosted")
                        .HasColumnType("date");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("EstateLocationId");

                    b.Property<int>("EstateTypeId");

                    b.Property<int>("FloorNumber");

                    b.Property<string>("PostedBy")
                        .IsRequired()
                        .HasMaxLength(450);

                    b.Property<int>("Price");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("char(1)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.HasKey("Id");

                    b.HasIndex("EstateLocationId");

                    b.HasIndex("EstateTypeId");

                    b.ToTable("EstateUnit");
                });

            modelBuilder.Entity("Homefind.Core.DomainModels.Favourites", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DateAdded");

                    b.Property<int>("EstateUnitId");

                    b.Property<string>("UserId");

                    b.Property<int?>("Views");

                    b.HasKey("Id");

                    b.HasIndex("EstateUnitId");

                    b.ToTable("Favourites");
                });

            modelBuilder.Entity("Homefind.Core.DomainModels.Review", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Comment");

                    b.Property<DateTime>("Date");

                    b.Property<string>("Description");

                    b.Property<string>("RatedUserId");

                    b.Property<string>("Rating");

                    b.Property<string>("Reviewer");

                    b.Property<string>("ReviewerEmail");

                    b.Property<string>("ReviewerName");

                    b.HasKey("Id");

                    b.ToTable("Reviews");
                });

            modelBuilder.Entity("Homefind.Core.DomainModels.EstateFeature", b =>
                {
                    b.HasOne("Homefind.Core.DomainModels.EstateUnit")
                        .WithOne("EstateFeature")
                        .HasForeignKey("Homefind.Core.DomainModels.EstateFeature", "Id")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Homefind.Core.DomainModels.EstateImage", b =>
                {
                    b.HasOne("Homefind.Core.DomainModels.EstateUnit")
                        .WithMany("EstateImages")
                        .HasForeignKey("EstateUnitId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Homefind.Core.DomainModels.EstateUnit", b =>
                {
                    b.HasOne("Homefind.Core.DomainModels.EstateLocation", "EstateLocation")
                        .WithMany()
                        .HasForeignKey("EstateLocationId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Homefind.Core.DomainModels.EstateType", "EstateType")
                        .WithMany()
                        .HasForeignKey("EstateTypeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Homefind.Core.DomainModels.Favourites", b =>
                {
                    b.HasOne("Homefind.Core.DomainModels.EstateUnit", "EstateUnit")
                        .WithMany()
                        .HasForeignKey("EstateUnitId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
