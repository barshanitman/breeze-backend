using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backendengine.Migrations
{
    /// <inheritdoc />
    public partial class AmendedDriversTableFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_DriverTearSheetOutputs_FinancialYearId",
                table: "DriverTearSheetOutputs",
                column: "FinancialYearId");

            migrationBuilder.CreateIndex(
                name: "IX_DriverTearSheetOutputs_StockUploadId",
                table: "DriverTearSheetOutputs",
                column: "StockUploadId");

            migrationBuilder.AddForeignKey(
                name: "FK_DriverTearSheetOutputs_FinancialYears_FinancialYearId",
                table: "DriverTearSheetOutputs",
                column: "FinancialYearId",
                principalTable: "FinancialYears",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DriverTearSheetOutputs_StockUploads_StockUploadId",
                table: "DriverTearSheetOutputs",
                column: "StockUploadId",
                principalTable: "StockUploads",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DriverTearSheetOutputs_FinancialYears_FinancialYearId",
                table: "DriverTearSheetOutputs");

            migrationBuilder.DropForeignKey(
                name: "FK_DriverTearSheetOutputs_StockUploads_StockUploadId",
                table: "DriverTearSheetOutputs");

            migrationBuilder.DropIndex(
                name: "IX_DriverTearSheetOutputs_FinancialYearId",
                table: "DriverTearSheetOutputs");

            migrationBuilder.DropIndex(
                name: "IX_DriverTearSheetOutputs_StockUploadId",
                table: "DriverTearSheetOutputs");
        }
    }
}
