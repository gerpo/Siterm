﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Siterm.EntityFramework;

namespace Siterm.EntityFramework.Migrations
{
    [DbContext(typeof(SitermDbContext))]
    [Migration("20200828090039_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Siterm.Domain.Models.Device", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int?>("ChiefId")
                        .HasColumnType("int");

                    b.Property<int>("DeviceNumber")
                        .HasColumnType("int");

                    b.Property<int>("FacilityId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Path")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.HasIndex("ChiefId");

                    b.HasIndex("FacilityId");

                    b.ToTable("Devices");
                });

            modelBuilder.Entity("Siterm.Domain.Models.Facility", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("OrderNr")
                        .HasColumnType("int");

                    b.Property<string>("Path")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.ToTable("Facilities");
                });

            modelBuilder.Entity("Siterm.Domain.Models.Instruction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("AllowedActivities")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("DeviceId")
                        .HasColumnType("int");

                    b.Property<string>("ForbiddenActivities")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int?>("InstructedId")
                        .HasColumnType("int");

                    b.Property<int?>("InstructorId")
                        .HasColumnType("int");

                    b.Property<bool>("IsArchived")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("NotificationSent")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("OldInstructedString")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Path")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.HasIndex("DeviceId");

                    b.HasIndex("InstructedId");

                    b.HasIndex("InstructorId");

                    b.ToTable("Instructions");
                });

            modelBuilder.Entity("Siterm.Domain.Models.ServiceReport", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("DeviceId")
                        .HasColumnType("int");

                    b.Property<bool>("NotificationSent")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Path")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int?>("PerformingUserId")
                        .HasColumnType("int");

                    b.Property<int>("Validity")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("DeviceId");

                    b.HasIndex("PerformingUserId");

                    b.ToTable("ServiceReports");
                });

            modelBuilder.Entity("Siterm.Domain.Models.ServiceTask", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Area")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Comment")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Description")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<bool>("IsDone")
                        .HasColumnType("tinyint(1)");

                    b.Property<int?>("ServiceReportId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ServiceReportId");

                    b.ToTable("ServiceTask");
                });

            modelBuilder.Entity("Siterm.Domain.Models.Substance", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Path")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.ToTable("Substances");
                });

            modelBuilder.Entity("Siterm.Domain.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("FirstName")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("LastName")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Siterm.Domain.Models.Device", b =>
                {
                    b.HasOne("Siterm.Domain.Models.User", "Chief")
                        .WithMany()
                        .HasForeignKey("ChiefId");

                    b.HasOne("Siterm.Domain.Models.Facility", "Facility")
                        .WithMany("Devices")
                        .HasForeignKey("FacilityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Siterm.Domain.Models.Instruction", b =>
                {
                    b.HasOne("Siterm.Domain.Models.Device", "Device")
                        .WithMany("Instructions")
                        .HasForeignKey("DeviceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Siterm.Domain.Models.User", "Instructed")
                        .WithMany("Instructions")
                        .HasForeignKey("InstructedId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Siterm.Domain.Models.User", "Instructor")
                        .WithMany("PerformedInstructions")
                        .HasForeignKey("InstructorId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Siterm.Domain.Models.ServiceReport", b =>
                {
                    b.HasOne("Siterm.Domain.Models.Device", "Device")
                        .WithMany("ServiceReports")
                        .HasForeignKey("DeviceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Siterm.Domain.Models.User", "PerformingUser")
                        .WithMany("ServiceReports")
                        .HasForeignKey("PerformingUserId");
                });

            modelBuilder.Entity("Siterm.Domain.Models.ServiceTask", b =>
                {
                    b.HasOne("Siterm.Domain.Models.ServiceReport", null)
                        .WithMany("ServiceTasks")
                        .HasForeignKey("ServiceReportId");
                });
#pragma warning restore 612, 618
        }
    }
}
