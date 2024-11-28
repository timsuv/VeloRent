using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VeloRent.Migrations
{
    /// <inheritdoc />
    public partial class AddBookedCarsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CarRentalLocation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__CarRenta__3214EC07049D1820", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CarType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Make = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Model = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Type = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    DailyRate = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    NumberOfSeats = table.Column<int>(type: "int", nullable: false),
                    FuelType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    AutomaticOrManual = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__CarType__3214EC07B3F31F0A", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DriverLicenseNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ContactNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Customer__3214EC07725B05E8", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Car",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CarTypeId = table.Column<int>(type: "int", nullable: false),
                    LocationId = table.Column<int>(type: "int", nullable: false),
                    LicensePlate = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Mileage = table.Column<int>(type: "int", nullable: false),
                    MaintenanceStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Car__3214EC07C769F414", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Car__CarTypeId__4316F928",
                        column: x => x.CarTypeId,
                        principalTable: "CarType",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__Car__LocationId__440B1D61",
                        column: x => x.LocationId,
                        principalTable: "CarRentalLocation",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Rental",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CarId = table.Column<int>(type: "int", nullable: false),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: false),
                    TotalCost = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Rental__3214EC07788CFA0A", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Rental__CarId__44FF419A",
                        column: x => x.CarId,
                        principalTable: "Car",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__Rental__Customer__45F365D3",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Car_CarTypeId",
                table: "Car",
                column: "CarTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Car_LocationId",
                table: "Car",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "UQ__Car__026BC15CAF79E10B",
                table: "Car",
                column: "LicensePlate",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__Customer__536C85E4480C0BCE",
                table: "Customer",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__Customer__A9D1053438DFEEA4",
                table: "Customer",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__Customer__C32FF260E1D1CF49",
                table: "Customer",
                column: "DriverLicenseNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rental_CarId",
                table: "Rental",
                column: "CarId");

            migrationBuilder.CreateIndex(
                name: "IX_Rental_CustomerId",
                table: "Rental",
                column: "CustomerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Rental");

            migrationBuilder.DropTable(
                name: "Car");

            migrationBuilder.DropTable(
                name: "Customer");

            migrationBuilder.DropTable(
                name: "CarType");

            migrationBuilder.DropTable(
                name: "CarRentalLocation");
        }
    }
}
