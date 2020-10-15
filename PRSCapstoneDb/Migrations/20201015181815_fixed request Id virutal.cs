using Microsoft.EntityFrameworkCore.Migrations;

namespace PRSCapstoneDb.Migrations
{
    public partial class fixedrequestIdvirutal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_RequestLines_RequestId",
                table: "RequestLines",
                column: "RequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_RequestLines_Requests_RequestId",
                table: "RequestLines",
                column: "RequestId",
                principalTable: "Requests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RequestLines_Requests_RequestId",
                table: "RequestLines");

            migrationBuilder.DropIndex(
                name: "IX_RequestLines_RequestId",
                table: "RequestLines");
        }
    }
}
