using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebService.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddUniqueConstraint(
                name: "AK_Users_ma_nguoi_dung",
                table: "Users",
                column: "ma_nguoi_dung");

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ma_don_hang = table.Column<string>(type: "varchar(20)", nullable: false),
                    ma_nguoi_dung = table.Column<string>(type: "varchar(8)", nullable: false),
                    ten_nguoi_nhan = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    so_dien_thoai = table.Column<string>(type: "varchar(20)", nullable: false),
                    dia_chi = table.Column<string>(type: "nvarchar(255)", nullable: false),
                    ghi_chu = table.Column<string>(type: "nvarchar(500)", nullable: false),
                    tam_tinh = table.Column<decimal>(type: "decimal(15,2)", nullable: false),
                    ma_giam_gia = table.Column<string>(type: "varchar(50)", nullable: true),
                    so_tien_giam = table.Column<decimal>(type: "decimal(15,2)", nullable: false),
                    phi_van_chuyen = table.Column<decimal>(type: "decimal(15,2)", nullable: false),
                    tong_thanh_toan = table.Column<decimal>(type: "decimal(15,2)", nullable: false),
                    phuong_thuc_thanh_toan = table.Column<string>(type: "varchar(50)", nullable: false),
                    ma_giao_dich = table.Column<string>(type: "varchar(100)", nullable: true),
                    cong_thanh_toan = table.Column<string>(type: "varchar(50)", nullable: true),
                    da_thanh_toan = table.Column<bool>(type: "bit", nullable: false),
                    trang_thai = table.Column<int>(type: "int", nullable: false),
                    ngay_tao = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    ngay_cap_nhat = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.id);
                    table.UniqueConstraint("AK_Orders_ma_don_hang", x => x.ma_don_hang);
                    table.ForeignKey(
                        name: "FK_Orders_Users_ma_nguoi_dung",
                        column: x => x.ma_nguoi_dung,
                        principalTable: "Users",
                        principalColumn: "ma_nguoi_dung",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderDetails",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ma_don_hang = table.Column<string>(type: "varchar(20)", nullable: false),
                    ma_san_pham = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    ten_san_pham = table.Column<string>(type: "nvarchar(150)", nullable: false),
                    hinh_anh = table.Column<string>(type: "varchar(255)", nullable: true),
                    so_luong = table.Column<int>(type: "int", nullable: false),
                    mau_sac = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    kich_thuoc = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    don_gia = table.Column<decimal>(type: "decimal(15,2)", nullable: false),
                    thanh_tien = table.Column<decimal>(type: "decimal(15,2)", nullable: false),
                    ngay_tao = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDetails", x => x.id);
                    table.ForeignKey(
                        name: "FK_OrderDetails_Orders_ma_don_hang",
                        column: x => x.ma_don_hang,
                        principalTable: "Orders",
                        principalColumn: "ma_don_hang",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderDetails_Products_ma_san_pham",
                        column: x => x.ma_san_pham,
                        principalTable: "Products",
                        principalColumn: "ma_san_pham",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderStatusHistories",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ma_don_hang = table.Column<string>(type: "varchar(20)", nullable: false),
                    trang_thai = table.Column<int>(type: "int", nullable: false),
                    ghi_chu = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    thoi_gian = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderStatusHistories", x => x.id);
                    table.ForeignKey(
                        name: "FK_OrderStatusHistories_Orders_ma_don_hang",
                        column: x => x.ma_don_hang,
                        principalTable: "Orders",
                        principalColumn: "ma_don_hang",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_ma_don_hang",
                table: "OrderDetails",
                column: "ma_don_hang");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_ma_san_pham",
                table: "OrderDetails",
                column: "ma_san_pham");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ma_nguoi_dung",
                table: "Orders",
                column: "ma_nguoi_dung");

            migrationBuilder.CreateIndex(
                name: "IX_OrderStatusHistories_ma_don_hang",
                table: "OrderStatusHistories",
                column: "ma_don_hang");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderDetails");

            migrationBuilder.DropTable(
                name: "OrderStatusHistories");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Users_ma_nguoi_dung",
                table: "Users");
        }
    }
}
