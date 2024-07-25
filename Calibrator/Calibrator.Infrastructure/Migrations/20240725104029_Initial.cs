using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Calibrator.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Reports",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Operator = table.Column<string>(type: "text", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    SetpointId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reports", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Setpoints",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Setpoints", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sensors",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    SerialNumber = table.Column<string>(type: "text", nullable: false),
                    SoftwareVersion = table.Column<string>(type: "text", nullable: false),
                    ManufactureDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EffectiveRangeMin = table.Column<double>(type: "double precision", nullable: false),
                    EffectiveRangeMax = table.Column<double>(type: "double precision", nullable: false),
                    ReportId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sensors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sensors_Reports_ReportId",
                        column: x => x.ReportId,
                        principalTable: "Reports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Exposures",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Number = table.Column<int>(type: "integer", nullable: false),
                    Value = table.Column<double>(type: "double precision", nullable: false),
                    Speed = table.Column<double>(type: "double precision", nullable: false),
                    Duration = table.Column<double>(type: "double precision", nullable: false),
                    SetpointId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exposures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Exposures_Setpoints_SetpointId",
                        column: x => x.SetpointId,
                        principalTable: "Setpoints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SensorChannels",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Number = table.Column<int>(type: "integer", nullable: false),
                    MaxError = table.Column<double>(type: "double precision", nullable: false),
                    PhisicalQuantity = table.Column<int>(type: "integer", nullable: false),
                    SensorId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SensorChannels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SensorChannels_Sensors_SensorId",
                        column: x => x.SensorId,
                        principalTable: "Sensors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AverageSamples",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ReferenceValue = table.Column<double>(type: "double precision", nullable: false),
                    Parameter = table.Column<double>(type: "double precision", nullable: false),
                    PhysicalQuantity = table.Column<double>(type: "double precision", nullable: false),
                    ChannelId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AverageSamples", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AverageSamples_SensorChannels_ChannelId",
                        column: x => x.ChannelId,
                        principalTable: "SensorChannels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Coefficients",
                columns: table => new
                {
                    CoefficientsId = table.Column<Guid>(type: "uuid", nullable: false),
                    A = table.Column<double>(type: "double precision", nullable: false),
                    B = table.Column<double>(type: "double precision", nullable: false),
                    C = table.Column<double>(type: "double precision", nullable: false),
                    SensorChannelId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coefficients", x => x.CoefficientsId);
                    table.ForeignKey(
                        name: "FK_Coefficients_SensorChannels_SensorChannelId",
                        column: x => x.SensorChannelId,
                        principalTable: "SensorChannels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Samples",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ReferenceValue = table.Column<double>(type: "double precision", nullable: false),
                    MeasurementTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PhysicalQuantity = table.Column<double>(type: "double precision", nullable: false),
                    Parameter = table.Column<double>(type: "double precision", nullable: false),
                    Direction = table.Column<int>(type: "integer", nullable: false),
                    ChannelId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Samples", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Samples_SensorChannels_ChannelId",
                        column: x => x.ChannelId,
                        principalTable: "SensorChannels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExternalImpacts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Value = table.Column<double>(type: "double precision", nullable: false),
                    PhisicalQuantity = table.Column<int>(type: "integer", nullable: false),
                    SampleId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExternalImpacts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExternalImpacts_Samples_SampleId",
                        column: x => x.SampleId,
                        principalTable: "Samples",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AverageSamples_ChannelId",
                table: "AverageSamples",
                column: "ChannelId");

            migrationBuilder.CreateIndex(
                name: "IX_Coefficients_SensorChannelId",
                table: "Coefficients",
                column: "SensorChannelId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Exposures_SetpointId",
                table: "Exposures",
                column: "SetpointId");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalImpacts_SampleId",
                table: "ExternalImpacts",
                column: "SampleId");

            migrationBuilder.CreateIndex(
                name: "IX_Samples_ChannelId",
                table: "Samples",
                column: "ChannelId");

            migrationBuilder.CreateIndex(
                name: "IX_SensorChannels_SensorId",
                table: "SensorChannels",
                column: "SensorId");

            migrationBuilder.CreateIndex(
                name: "IX_Sensors_ReportId",
                table: "Sensors",
                column: "ReportId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AverageSamples");

            migrationBuilder.DropTable(
                name: "Coefficients");

            migrationBuilder.DropTable(
                name: "Exposures");

            migrationBuilder.DropTable(
                name: "ExternalImpacts");

            migrationBuilder.DropTable(
                name: "Setpoints");

            migrationBuilder.DropTable(
                name: "Samples");

            migrationBuilder.DropTable(
                name: "SensorChannels");

            migrationBuilder.DropTable(
                name: "Sensors");

            migrationBuilder.DropTable(
                name: "Reports");
        }
    }
}
