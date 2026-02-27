using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LogisticIntegration.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TripSettlements",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WeighingOrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PhysicalNetWeight = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Penalty_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Penalty_MissingWeight = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Penalty_AppliedMaxPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Penalty_TotalPenaltyAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TripSettlements", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProviderLoad",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProviderCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DocumentaryWeight = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TripSettlementId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProviderLoad", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProviderLoad_TripSettlements_TripSettlementId",
                        column: x => x.TripSettlementId,
                        principalTable: "TripSettlements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProviderLoad_TripSettlementId",
                table: "ProviderLoad",
                column: "TripSettlementId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProviderLoad");

            migrationBuilder.DropTable(
                name: "TripSettlements");
        }
    }
}
