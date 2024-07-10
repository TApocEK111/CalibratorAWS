﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Calibrator.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigation : Migration
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
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reports", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sensors",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    SerialNumber = table.Column<string>(type: "text", nullable: false),
                    SoftwareVersion = table.Column<string>(type: "text", nullable: false),
                    ManufactureDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    EffectiveRangeMin = table.Column<double>(type: "double precision", nullable: false),
                    EffectiveRangeMax = table.Column<double>(type: "double precision", nullable: false),
                    ReportId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sensors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sensors_Reports_ReportId",
                        column: x => x.ReportId,
                        principalTable: "Reports",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SensorChannels",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PhisicalQuantity = table.Column<int>(type: "integer", nullable: false),
                    SensorId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SensorChannels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SensorChannels_Sensors_SensorId",
                        column: x => x.SensorId,
                        principalTable: "Sensors",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AverageSamples",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ReferenceValue = table.Column<double>(type: "double precision", nullable: false),
                    Parameter = table.Column<double>(type: "double precision", nullable: false),
                    PhysicalQuantity = table.Column<double>(type: "double precision", nullable: false),
                    SensorChannelId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AverageSamples", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AverageSamples_SensorChannels_SensorChannelId",
                        column: x => x.SensorChannelId,
                        principalTable: "SensorChannels",
                        principalColumn: "Id");
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
                    SensorChannelId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Samples", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Samples_SensorChannels_SensorChannelId",
                        column: x => x.SensorChannelId,
                        principalTable: "SensorChannels",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ExternalImpacts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Value = table.Column<double>(type: "double precision", nullable: false),
                    PhisicalQuantity = table.Column<int>(type: "integer", nullable: false),
                    SampleId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExternalImpacts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExternalImpacts_Samples_SampleId",
                        column: x => x.SampleId,
                        principalTable: "Samples",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AverageSamples_SensorChannelId",
                table: "AverageSamples",
                column: "SensorChannelId");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalImpacts_SampleId",
                table: "ExternalImpacts",
                column: "SampleId");

            migrationBuilder.CreateIndex(
                name: "IX_Samples_SensorChannelId",
                table: "Samples",
                column: "SensorChannelId");

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
                name: "ExternalImpacts");

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