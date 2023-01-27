﻿// <auto-generated />
using System;
using MVP.Date;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MVP.Migrations
{
    [DbContext(typeof(AppDB))]
    [Migration("20230127183338_add_Creator_in_Tasks")]
    partial class add_Creator_in_Tasks
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.20")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("MVP.Date.Models.CompanyStructure", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("divisionsId")
                        .HasColumnType("int");

                    b.Property<string>("supervisor")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("id");

                    b.ToTable("DBCompanyStructure");
                });

            modelBuilder.Entity("MVP.Date.Models.Division", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("code")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("name")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("id");

                    b.ToTable("DBDivision");
                });

            modelBuilder.Entity("MVP.Date.Models.LogisticProject", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("CommitorId")
                        .HasColumnType("int");

                    b.Property<string>("allStages")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("arhive")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("comment")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<DateTime>("dateRedaction")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("link")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("priority")
                        .HasColumnType("int");

                    b.Property<int>("projectId")
                        .HasColumnType("int");

                    b.Property<string>("supervisor")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("id");

                    b.ToTable("DBLogistickProject");
                });

            modelBuilder.Entity("MVP.Date.Models.LogistickTask", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("CommitorId")
                        .HasColumnType("int");

                    b.Property<string>("ProjectCode")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("TaskCode")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("TaskId")
                        .HasColumnType("int");

                    b.Property<TimeSpan>("actualTime")
                        .HasColumnType("time(6)");

                    b.Property<string>("comment")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<DateTime>("dateRedaction")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("descTask")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<TimeSpan>("planedTime")
                        .HasColumnType("time(6)");

                    b.Property<int>("resipienId")
                        .HasColumnType("int");

                    b.Property<int>("supervisorId")
                        .HasColumnType("int");

                    b.Property<int>("taskStatusId")
                        .HasColumnType("int");

                    b.HasKey("id");

                    b.ToTable("DBLogistickTask");
                });

            modelBuilder.Entity("MVP.Date.Models.Post", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("code")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("name")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("roleCod")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("id");

                    b.ToTable("DBPost");
                });

            modelBuilder.Entity("MVP.Date.Models.Project", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("actualFinishDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("allStages")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("archive")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("code")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<DateTime>("dateStart")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("history")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("link")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("name")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("nowStage")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<DateTime>("plannedFinishDate")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("priority")
                        .HasColumnType("int");

                    b.Property<string>("shortName")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("supervisor")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("id");

                    b.ToTable("DBProject");
                });

            modelBuilder.Entity("MVP.Date.Models.Role", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("code")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("name")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("recipient")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("supervisor")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("id");

                    b.ToTable("DBRole");
                });

            modelBuilder.Entity("MVP.Date.Models.Staff", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("code")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("divisionId")
                        .HasColumnType("int");

                    b.Property<string>("login")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("name")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("passvord")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("post")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("roleId")
                        .HasColumnType("int");

                    b.Property<string>("supervisorCod")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("id");

                    b.ToTable("DBStaff");
                });

            modelBuilder.Entity("MVP.Date.Models.Stage", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("name")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("projectId")
                        .HasColumnType("int");

                    b.Property<int>("stageId")
                        .HasColumnType("int");

                    b.HasKey("id");

                    b.ToTable("DBStage");
                });

            modelBuilder.Entity("MVP.Date.Models.TaskStatus", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("name")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("id");

                    b.ToTable("DBTaskStatus");
                });

            modelBuilder.Entity("MVP.Date.Models.Tasks", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Stage")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("TaskCodeParent")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<TimeSpan>("actualTime")
                        .HasColumnType("time(6)");

                    b.Property<string>("code")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("comment")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("creator")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<DateTime>("date")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("desc")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<DateTime>("finish")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("liteTask")
                        .HasColumnType("tinyint(1)");

                    b.Property<TimeSpan>("plannedTime")
                        .HasColumnType("time(6)");

                    b.Property<int>("priority")
                        .HasColumnType("int");

                    b.Property<string>("projectCode")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("recipient")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<DateTime>("start")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("startWork")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("status")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("supervisor")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("id");

                    b.ToTable("DBTask");
                });
#pragma warning restore 612, 618
        }
    }
}
