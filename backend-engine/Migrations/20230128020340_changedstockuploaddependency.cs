using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backendengine.Migrations
{
    /// <inheritdoc />
    public partial class changedstockuploaddependency : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_StockUploadValuations_StockUploadId",
                table: "StockUploadValuations");

            migrationBuilder.CreateIndex(
                name: "IX_StockUploadValuations_StockUploadId",
                table: "StockUploadValuations",
                column: "StockUploadId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_StockUploadValuations_StockUploadId",
                table: "StockUploadValuations");

            migrationBuilder.CreateIndex(
                name: "IX_StockUploadValuations_StockUploadId",
                table: "StockUploadValuations",
                column: "StockUploadId",
                unique: true);
        }
    }
}
