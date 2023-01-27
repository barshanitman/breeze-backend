using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backendengine.Migrations
{
    /// <inheritdoc />
    public partial class AddedStockUploadValuation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StockUploadValuations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Methodologies = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StockUploadId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockUploadValuations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockUploadValuations_StockUploads_StockUploadId",
                        column: x => x.StockUploadId,
                        principalTable: "StockUploads",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StockUploadValuations_StockUploadId",
                table: "StockUploadValuations",
                column: "StockUploadId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StockUploadValuations");
        }
    }
}
