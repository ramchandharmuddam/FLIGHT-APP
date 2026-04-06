using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Group3Flight.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Airline",
                columns: table => new
                {
                    AirlineId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    ImageName = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Airline", x => x.AirlineId);
                });

            migrationBuilder.CreateTable(
                name: "Flight",
                columns: table => new
                {
                    FlightId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FlightCode = table.Column<string>(type: "TEXT", nullable: false),
                    From = table.Column<string>(type: "TEXT", nullable: false),
                    To = table.Column<string>(type: "TEXT", nullable: false),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DepartureTime = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    ArrivalTime = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    CabinType = table.Column<string>(type: "TEXT", nullable: false),
                    Emission = table.Column<double>(type: "REAL", nullable: false),
                    AircraftType = table.Column<string>(type: "TEXT", nullable: false),
                    Price = table.Column<decimal>(type: "TEXT", nullable: false),
                    AirlineId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flight", x => x.FlightId);
                    table.ForeignKey(
                        name: "FK_Flight_Airline_AirlineId",
                        column: x => x.AirlineId,
                        principalTable: "Airline",
                        principalColumn: "AirlineId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FlightReservations",
                columns: table => new
                {
                    FlightReservationsId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FlightId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlightReservations", x => x.FlightReservationsId);
                    table.ForeignKey(
                        name: "FK_FlightReservations_Flight_FlightId",
                        column: x => x.FlightId,
                        principalTable: "Flight",
                        principalColumn: "FlightId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Airline",
                columns: new[] { "AirlineId", "ImageName", "Name" },
                values: new object[,]
                {
                    { 11, "SpiritAirlines.png", "Spirit Airlines" },
                    { 12, "FrontierAirlines.png", "Frontier Airlines" },
                    { 13, "HawaiianAirlines.png", "Hawaiian Airlines" },
                    { 14, "AllegiantAir.png", "Allegiant Air" },
                    { 15, "SunCountry.png", "Sun Country Airlines" },
                    { 16, "SilverAirways.png", "Silver Airways" }
                });

            migrationBuilder.InsertData(
                table: "Flight",
                columns: new[] { "FlightId", "AircraftType", "AirlineId", "ArrivalTime", "CabinType", "Date", "DepartureTime", "Emission", "FlightCode", "From", "Price", "To" },
                values: new object[,]
                {
                    { 1, "Airbus A321", 11, new TimeSpan(0, 9, 30, 0, 0), "Economy", new DateTime(2026, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 15, 0, 0), 200.30000000000001, "NK601", "Miami", 150.00m, "Houston" },
                    { 2, "Airbus A320neo", 12, new TimeSpan(0, 20, 10, 0, 0), "Basic Economy", new DateTime(2026, 5, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 12, 45, 0, 0), 300.5, "F9123", "Las Vegas", 275.00m, "Orlando" },
                    { 3, "Airbus A330", 13, new TimeSpan(0, 23, 30, 0, 0), "Business", new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 16, 0, 0, 0), 410.80000000000001, "HA789", "Honolulu", 680.00m, "San Diego" },
                    { 4, "Boeing 737 MAX", 14, new TimeSpan(0, 12, 55, 0, 0), "Economy", new DateTime(2026, 5, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 10, 20, 0, 0), 220.40000000000001, "G4550", "Nashville", 195.00m, "Phoenix" },
                    { 5, "Boeing 737-800", 15, new TimeSpan(0, 10, 45, 0, 0), "Economy Plus", new DateTime(2026, 5, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 6, 0, 0, 0), 350.89999999999998, "SY880", "Minneapolis", 520.00m, "Cancun" },
                    { 6, "ATR 72", 16, new TimeSpan(0, 10, 25, 0, 0), "Economy", new DateTime(2026, 5, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 9, 10, 0, 0), 90.599999999999994, "3M222", "Tampa", 130.00m, "Key West" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Flight_AirlineId",
                table: "Flight",
                column: "AirlineId");

            migrationBuilder.CreateIndex(
                name: "IX_FlightReservations_FlightId",
                table: "FlightReservations",
                column: "FlightId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FlightReservations");

            migrationBuilder.DropTable(
                name: "Flight");

            migrationBuilder.DropTable(
                name: "Airline");
        }
    }
}
