using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MySqlConnect.Migrations
{
    public partial class AddLocationFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservation_Location_LocationId",
                table: "Reservation");

            migrationBuilder.AlterColumn<int>(
                name: "LocationId",
                table: "Reservation",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Capacity",
                table: "Location",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ShopType",
                table: "Location",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SlotDuration",
                table: "Location",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SlotSize",
                table: "Location",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "LocationOpening",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    OpeningTime = table.Column<DateTime>(nullable: false),
                    ClosingTime = table.Column<DateTime>(nullable: false),
                    OpeningDays = table.Column<string>(nullable: true),
                    LocationId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocationOpening", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LocationOpening_Location_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Location",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LocationOpening_LocationId",
                table: "LocationOpening",
                column: "LocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservation_Location_LocationId",
                table: "Reservation",
                column: "LocationId",
                principalTable: "Location",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservation_Location_LocationId",
                table: "Reservation");

            migrationBuilder.DropTable(
                name: "LocationOpening");

            migrationBuilder.DropColumn(
                name: "Capacity",
                table: "Location");

            migrationBuilder.DropColumn(
                name: "ShopType",
                table: "Location");

            migrationBuilder.DropColumn(
                name: "SlotDuration",
                table: "Location");

            migrationBuilder.DropColumn(
                name: "SlotSize",
                table: "Location");

            migrationBuilder.AlterColumn<int>(
                name: "LocationId",
                table: "Reservation",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Reservation_Location_LocationId",
                table: "Reservation",
                column: "LocationId",
                principalTable: "Location",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
