using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GeekShopping.CartAPI.Migrations
{
    /// <inheritdoc />
    public partial class AlterTableCartDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_cart_detail_cart_header_user_id",
                table: "cart_detail");

            migrationBuilder.DropForeignKey(
                name: "FK_cart_detail_product_ProductId",
                table: "cart_detail");

            migrationBuilder.DropColumn(
                name: "CartId",
                table: "cart_detail");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "cart_detail",
                newName: "cart_header_id");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "cart_detail",
                newName: "product_id");

            migrationBuilder.RenameIndex(
                name: "IX_cart_detail_user_id",
                table: "cart_detail",
                newName: "IX_cart_detail_cart_header_id");

            migrationBuilder.RenameIndex(
                name: "IX_cart_detail_ProductId",
                table: "cart_detail",
                newName: "IX_cart_detail_product_id");


            migrationBuilder.AlterColumn<string>(
                name: "coupon_code",
                table: "cart_header",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddForeignKey(
                name: "FK_cart_detail_cart_header_cart_header_id",
                table: "cart_detail",
                column: "cart_header_id",
                principalTable: "cart_header",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_cart_detail_product_product_id",
                table: "cart_detail",
                column: "product_id",
                principalTable: "product",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_cart_detail_cart_header_cart_header_id",
                table: "cart_detail");

            migrationBuilder.DropForeignKey(
                name: "FK_cart_detail_product_product_id",
                table: "cart_detail");

            migrationBuilder.RenameColumn(
                name: "product_id",
                table: "cart_detail",
                newName: "ProductId");

            migrationBuilder.RenameColumn(
                name: "cart_header_id",
                table: "cart_detail",
                newName: "user_id");

            migrationBuilder.RenameIndex(
                name: "IX_cart_detail_product_id",
                table: "cart_detail",
                newName: "IX_cart_detail_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_cart_detail_cart_header_id",
                table: "cart_detail",
                newName: "IX_cart_detail_user_id");

            migrationBuilder.UpdateData(
                table: "cart_header",
                keyColumn: "coupon_code",
                keyValue: null,
                column: "coupon_code",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "coupon_code",
                table: "cart_header",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<long>(
                name: "CartId",
                table: "cart_detail",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddForeignKey(
                name: "FK_cart_detail_cart_header_user_id",
                table: "cart_detail",
                column: "user_id",
                principalTable: "cart_header",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_cart_detail_product_ProductId",
                table: "cart_detail",
                column: "ProductId",
                principalTable: "product",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
