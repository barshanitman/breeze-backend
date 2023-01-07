using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backendengine.Migrations
{
    /// <inheritdoc />
    public partial class AddedSummaryDataTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CellReference",
                table: "StockTearSheetOutputs");

            migrationBuilder.DropColumn(
                name: "FinancialYearId",
                table: "StockTearSheetOutputs");

            migrationBuilder.DropColumn(
                name: "SheetReference",
                table: "StockTearSheetOutputs");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CellReference",
                table: "StockTearSheetOutputs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "FinancialYearId",
                table: "StockTearSheetOutputs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "SheetReference",
                table: "StockTearSheetOutputs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
