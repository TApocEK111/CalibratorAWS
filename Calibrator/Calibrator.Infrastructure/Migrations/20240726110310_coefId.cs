using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Calibrator.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class coefId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exposures_Setpoints_SetpointId",
                table: "Exposures");

            migrationBuilder.RenameColumn(
                name: "CoefficientsId",
                table: "Coefficients",
                newName: "Id");

            migrationBuilder.AlterColumn<Guid>(
                name: "SetpointId",
                table: "Exposures",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_Exposures_Setpoints_SetpointId",
                table: "Exposures",
                column: "SetpointId",
                principalTable: "Setpoints",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exposures_Setpoints_SetpointId",
                table: "Exposures");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Coefficients",
                newName: "CoefficientsId");

            migrationBuilder.AlterColumn<Guid>(
                name: "SetpointId",
                table: "Exposures",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Exposures_Setpoints_SetpointId",
                table: "Exposures",
                column: "SetpointId",
                principalTable: "Setpoints",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
