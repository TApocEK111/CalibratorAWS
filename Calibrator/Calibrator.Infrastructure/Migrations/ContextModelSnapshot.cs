﻿// <auto-generated />
using System;
using Calibrator.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Calibrator.Infrastructure.Migrations
{
    [DbContext(typeof(Context))]
    partial class ContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Calibrator.Domain.Model.Report.AverageSample", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("ChannelId")
                        .HasColumnType("uuid");

                    b.Property<double>("Parameter")
                        .HasColumnType("double precision");

                    b.Property<double>("PhysicalQuantity")
                        .HasColumnType("double precision");

                    b.Property<double>("ReferenceValue")
                        .HasColumnType("double precision");

                    b.HasKey("Id");

                    b.HasIndex("ChannelId");

                    b.ToTable("AverageSamples");
                });

            modelBuilder.Entity("Calibrator.Domain.Model.Report.Coefficients", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<double>("A")
                        .HasColumnType("double precision");

                    b.Property<double>("B")
                        .HasColumnType("double precision");

                    b.Property<double>("C")
                        .HasColumnType("double precision");

                    b.Property<Guid>("SensorChannelId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("SensorChannelId")
                        .IsUnique();

                    b.ToTable("Coefficients");
                });

            modelBuilder.Entity("Calibrator.Domain.Model.Report.ExternalImpact", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("PhisicalQuantity")
                        .HasColumnType("integer");

                    b.Property<Guid>("SampleId")
                        .HasColumnType("uuid");

                    b.Property<double>("Value")
                        .HasColumnType("double precision");

                    b.HasKey("Id");

                    b.HasIndex("SampleId");

                    b.ToTable("ExternalImpacts");
                });

            modelBuilder.Entity("Calibrator.Domain.Model.Report.Report", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Operator")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("SetpointId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("Reports");
                });

            modelBuilder.Entity("Calibrator.Domain.Model.Report.Sample", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("ChannelId")
                        .HasColumnType("uuid");

                    b.Property<int>("Direction")
                        .HasColumnType("integer");

                    b.Property<DateTime>("MeasurementTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<double>("Parameter")
                        .HasColumnType("double precision");

                    b.Property<double>("PhysicalQuantity")
                        .HasColumnType("double precision");

                    b.Property<double>("ReferenceValue")
                        .HasColumnType("double precision");

                    b.HasKey("Id");

                    b.HasIndex("ChannelId");

                    b.ToTable("Samples");
                });

            modelBuilder.Entity("Calibrator.Domain.Model.Report.Sensor", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<double>("EffectiveRangeMax")
                        .HasColumnType("double precision");

                    b.Property<double>("EffectiveRangeMin")
                        .HasColumnType("double precision");

                    b.Property<DateTime>("ManufactureDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("ReportId")
                        .HasColumnType("uuid");

                    b.Property<string>("SerialNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("SoftwareVersion")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ReportId");

                    b.ToTable("Sensors");
                });

            modelBuilder.Entity("Calibrator.Domain.Model.Report.SensorChannel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<double>("MaxError")
                        .HasColumnType("double precision");

                    b.Property<int>("Number")
                        .HasColumnType("integer");

                    b.Property<int>("PhisicalQuantity")
                        .HasColumnType("integer");

                    b.Property<Guid>("SensorId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("SensorId");

                    b.ToTable("SensorChannels");
                });

            modelBuilder.Entity("Calibrator.Domain.Model.Setpoint.Exposure", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<double>("Duration")
                        .HasColumnType("double precision");

                    b.Property<int>("Number")
                        .HasColumnType("integer");

                    b.Property<Guid?>("SetpointId")
                        .HasColumnType("uuid");

                    b.Property<double>("Speed")
                        .HasColumnType("double precision");

                    b.Property<double>("Value")
                        .HasColumnType("double precision");

                    b.HasKey("Id");

                    b.HasIndex("SetpointId");

                    b.ToTable("Exposures");
                });

            modelBuilder.Entity("Calibrator.Domain.Model.Setpoint.Setpoint", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Setpoints");
                });

            modelBuilder.Entity("Calibrator.Domain.Model.Report.AverageSample", b =>
                {
                    b.HasOne("Calibrator.Domain.Model.Report.SensorChannel", "Channel")
                        .WithMany("AverageSamples")
                        .HasForeignKey("ChannelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Channel");
                });

            modelBuilder.Entity("Calibrator.Domain.Model.Report.Coefficients", b =>
                {
                    b.HasOne("Calibrator.Domain.Model.Report.SensorChannel", null)
                        .WithOne("Coefficients")
                        .HasForeignKey("Calibrator.Domain.Model.Report.Coefficients", "SensorChannelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Calibrator.Domain.Model.Report.ExternalImpact", b =>
                {
                    b.HasOne("Calibrator.Domain.Model.Report.Sample", "Sample")
                        .WithMany("ExternalImpacts")
                        .HasForeignKey("SampleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Sample");
                });

            modelBuilder.Entity("Calibrator.Domain.Model.Report.Sample", b =>
                {
                    b.HasOne("Calibrator.Domain.Model.Report.SensorChannel", "Channel")
                        .WithMany("Samples")
                        .HasForeignKey("ChannelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Channel");
                });

            modelBuilder.Entity("Calibrator.Domain.Model.Report.Sensor", b =>
                {
                    b.HasOne("Calibrator.Domain.Model.Report.Report", "Report")
                        .WithMany("Sensors")
                        .HasForeignKey("ReportId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Report");
                });

            modelBuilder.Entity("Calibrator.Domain.Model.Report.SensorChannel", b =>
                {
                    b.HasOne("Calibrator.Domain.Model.Report.Sensor", "Sensor")
                        .WithMany("Channels")
                        .HasForeignKey("SensorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Sensor");
                });

            modelBuilder.Entity("Calibrator.Domain.Model.Setpoint.Exposure", b =>
                {
                    b.HasOne("Calibrator.Domain.Model.Setpoint.Setpoint", null)
                        .WithMany("Exposures")
                        .HasForeignKey("SetpointId");
                });

            modelBuilder.Entity("Calibrator.Domain.Model.Report.Report", b =>
                {
                    b.Navigation("Sensors");
                });

            modelBuilder.Entity("Calibrator.Domain.Model.Report.Sample", b =>
                {
                    b.Navigation("ExternalImpacts");
                });

            modelBuilder.Entity("Calibrator.Domain.Model.Report.Sensor", b =>
                {
                    b.Navigation("Channels");
                });

            modelBuilder.Entity("Calibrator.Domain.Model.Report.SensorChannel", b =>
                {
                    b.Navigation("AverageSamples");

                    b.Navigation("Coefficients")
                        .IsRequired();

                    b.Navigation("Samples");
                });

            modelBuilder.Entity("Calibrator.Domain.Model.Setpoint.Setpoint", b =>
                {
                    b.Navigation("Exposures");
                });
#pragma warning restore 612, 618
        }
    }
}
