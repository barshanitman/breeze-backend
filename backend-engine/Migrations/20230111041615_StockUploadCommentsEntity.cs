using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backendengine.Migrations
{
    /// <inheritdoc />
    public partial class StockUploadCommentsEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StockUploadComments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StockUploadId = table.Column<int>(type: "int", nullable: false),
                    comment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    imageURL = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UploadedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockUploadComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockUploadComments_StockUploads_StockUploadId",
                        column: x => x.StockUploadId,
                        principalTable: "StockUploads",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StockUploadComments_StockUploadId",
                table: "StockUploadComments",
                column: "StockUploadId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StockUploadComments");
        }
    }
}
