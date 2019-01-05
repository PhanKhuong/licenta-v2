using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Homefind.Infrastructure.Migrations.EstateDb
{
    public partial class estates_migr69 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EstateLocation",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Address = table.Column<string>(maxLength: 250, nullable: true),
                    City = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    Country = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    State = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    ZipCode = table.Column<string>(unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstateLocation", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EstateType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TypeName = table.Column<string>(unicode: false, maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstateType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Comment = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    RatedUserId = table.Column<string>(nullable: true),
                    Rating = table.Column<string>(nullable: true),
                    Reviewer = table.Column<string>(nullable: true),
                    ReviewerEmail = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EstateUnit",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Balconies = table.Column<int>(nullable: false),
                    Bathrooms = table.Column<int>(nullable: false),
                    Bedrooms = table.Column<int>(nullable: false),
                    CarpetArea = table.Column<int>(nullable: false),
                    DateAvailable = table.Column<DateTime>(type: "date", nullable: false),
                    DatePosted = table.Column<DateTime>(type: "date", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    EstateLocationId = table.Column<int>(nullable: false),
                    EstateTypeId = table.Column<int>(nullable: false),
                    FloorNumber = table.Column<int>(nullable: false),
                    PostedBy = table.Column<string>(maxLength: 450, nullable: false),
                    Price = table.Column<int>(nullable: false),
                    Status = table.Column<string>(type: "char(1)", nullable: false),
                    Title = table.Column<string>(unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstateUnit", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EstateUnit_EstateLocation_EstateLocationId",
                        column: x => x.EstateLocationId,
                        principalTable: "EstateLocation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EstateUnit_EstateType_EstateTypeId",
                        column: x => x.EstateTypeId,
                        principalTable: "EstateType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EstateFeature",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    ArePetsAllowed = table.Column<bool>(type: "bit", nullable: false),
                    HasAirConditioning = table.Column<bool>(type: "bit", nullable: false),
                    HasCarParking = table.Column<bool>(type: "bit", nullable: false),
                    HasInternet = table.Column<bool>(type: "bit", nullable: false),
                    HasSwimmingPool = table.Column<bool>(type: "bit", nullable: false),
                    HasTV = table.Column<bool>(type: "bit", nullable: false),
                    IsFurnished = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstateFeature", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EstateFeature_EstateUnit_Id",
                        column: x => x.Id,
                        principalTable: "EstateUnit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EstateImage",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ContentType = table.Column<string>(type: "varchar(50)", nullable: false),
                    Data = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    EstateUnitId = table.Column<int>(nullable: false),
                    Height = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "varchar(50)", nullable: false),
                    Width = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstateImage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EstateImage_EstateUnit_EstateUnitId",
                        column: x => x.EstateUnitId,
                        principalTable: "EstateUnit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Favourites",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateAdded = table.Column<DateTime>(nullable: false),
                    EstateUnitId = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    Views = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Favourites", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Favourites_EstateUnit_EstateUnitId",
                        column: x => x.EstateUnitId,
                        principalTable: "EstateUnit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EstateImage_EstateUnitId",
                table: "EstateImage",
                column: "EstateUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_EstateUnit_EstateLocationId",
                table: "EstateUnit",
                column: "EstateLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_EstateUnit_EstateTypeId",
                table: "EstateUnit",
                column: "EstateTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Favourites_EstateUnitId",
                table: "Favourites",
                column: "EstateUnitId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EstateFeature");

            migrationBuilder.DropTable(
                name: "EstateImage");

            migrationBuilder.DropTable(
                name: "Favourites");

            migrationBuilder.DropTable(
                name: "Reviews");

            migrationBuilder.DropTable(
                name: "EstateUnit");

            migrationBuilder.DropTable(
                name: "EstateLocation");

            migrationBuilder.DropTable(
                name: "EstateType");
        }
    }
}
