using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartSpaceControl.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAreaIdToRoom : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AreaId",
                table: "Rooms",
                type: "int",
                nullable: true,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_AreaId",
                table: "Rooms",
                column: "AreaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_Areas_AreaId",
                table: "Rooms",
                column: "AreaId",
                principalTable: "Areas",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_Areas_AreaId",
                table: "Rooms");

            migrationBuilder.DropIndex(
                name: "IX_Rooms_AreaId",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "AreaId",
                table: "Rooms");
        }
    }
}
