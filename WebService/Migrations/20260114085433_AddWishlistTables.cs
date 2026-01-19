using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebService.Migrations
{
    /// <inheritdoc />
    public partial class AddWishlistTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Wishlists",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ma_nguoi_dung = table.Column<string>(type: "varchar(8)", nullable: false),
                    ma_san_pham = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    ngay_them = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wishlists", x => x.id);
                    table.ForeignKey(
                        name: "FK_Wishlists_Products_ma_san_pham",
                        column: x => x.ma_san_pham,
                        principalTable: "Products",
                        principalColumn: "ma_san_pham",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Wishlists_Users_ma_nguoi_dung",
                        column: x => x.ma_nguoi_dung,
                        principalTable: "Users",
                        principalColumn: "ma_nguoi_dung",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Wishlists_ma_nguoi_dung_ma_san_pham",
                table: "Wishlists",
                columns: new[] { "ma_nguoi_dung", "ma_san_pham" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Wishlists_ma_san_pham",
                table: "Wishlists",
                column: "ma_san_pham");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Wishlists");
        }
    }
}
