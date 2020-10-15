using Microsoft.EntityFrameworkCore.Migrations;

namespace PRSCapstoneDb.Migrations
{
    public partial class addedrequestline : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RequestId",
                table: "Requests",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "RequestLines",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestId = table.Column<int>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    Quantity = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RequestLines_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Requests_RequestId",
                table: "Requests",
                column: "RequestId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestLines_ProductId",
                table: "RequestLines",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_Requests_RequestId",
                table: "Requests",
                column: "RequestId",
                principalTable: "Requests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Requests_Requests_RequestId",
                table: "Requests");

            migrationBuilder.DropTable(
                name: "RequestLines");

            migrationBuilder.DropIndex(
                name: "IX_Requests_RequestId",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "RequestId",
                table: "Requests");
        }
    }
}
