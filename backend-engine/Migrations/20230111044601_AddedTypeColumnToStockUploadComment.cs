using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backendengine.Migrations
{
    /// <inheritdoc />
    public partial class AddedTypeColumnToStockUploadComment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "type",
                table: "StockUploadComments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "type",
                table: "StockUploadComments");
        }
    }
}
