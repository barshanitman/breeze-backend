using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backendengine.Migrations
{
    /// <inheritdoc />
    public partial class FirstMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FinancialYears",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinancialYears", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Stocks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stocks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TearSheetOutputs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FinancialYearId = table.Column<int>(type: "int", nullable: false),
                    SheetReference = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CellReference = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TearSheetOutputs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TearSheetOutputs_FinancialYears_FinancialYearId",
                        column: x => x.FinancialYearId,
                        principalTable: "FinancialYears",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StockUploads",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UploadedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StockId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockUploads", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockUploads_Stocks_StockId",
                        column: x => x.StockId,
                        principalTable: "Stocks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProfitLossDrivers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StockUploadId = table.Column<int>(type: "int", nullable: false),
                    InputName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InputSheetReference = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InputCellReference = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OutputName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OutputSheetReference = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OutputCellReference = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfitLossDrivers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProfitLossDrivers_StockUploads_StockUploadId",
                        column: x => x.StockUploadId,
                        principalTable: "StockUploads",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StockTearSheetOutputs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TearSheetOutputId = table.Column<int>(type: "int", nullable: false),
                    StockUploadId = table.Column<int>(type: "int", nullable: false),
                    FinancialYearId = table.Column<int>(type: "int", nullable: false),
                    SheetReference = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CellReference = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockTearSheetOutputs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockTearSheetOutputs_StockUploads_StockUploadId",
                        column: x => x.StockUploadId,
                        principalTable: "StockUploads",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StockTearSheetOutputs_TearSheetOutputs_TearSheetOutputId",
                        column: x => x.TearSheetOutputId,
                        principalTable: "TearSheetOutputs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProfitLossDrivers_StockUploadId",
                table: "ProfitLossDrivers",
                column: "StockUploadId");

            migrationBuilder.CreateIndex(
                name: "IX_StockTearSheetOutputs_StockUploadId",
                table: "StockTearSheetOutputs",
                column: "StockUploadId");

            migrationBuilder.CreateIndex(
                name: "IX_StockTearSheetOutputs_TearSheetOutputId",
                table: "StockTearSheetOutputs",
                column: "TearSheetOutputId");

            migrationBuilder.CreateIndex(
                name: "IX_StockUploads_StockId",
                table: "StockUploads",
                column: "StockId");

            migrationBuilder.CreateIndex(
                name: "IX_TearSheetOutputs_FinancialYearId",
                table: "TearSheetOutputs",
                column: "FinancialYearId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProfitLossDrivers");

            migrationBuilder.DropTable(
                name: "StockTearSheetOutputs");

            migrationBuilder.DropTable(
                name: "StockUploads");

            migrationBuilder.DropTable(
                name: "TearSheetOutputs");

            migrationBuilder.DropTable(
                name: "Stocks");

            migrationBuilder.DropTable(
                name: "FinancialYears");
        }
    }
}
