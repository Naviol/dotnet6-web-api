﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using comp2001hk_assessment2_part1.Data;

#nullable disable

namespace comp2001hk_assessment2_part1.Migrations
{
    [DbContext(typeof(comp2001hk_assessment2_part1Context))]
    [Migration("20220208190325_AddProgrammeStudent")]
    partial class AddProgrammeStudent
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("comp2001hk_assessment2_part1.Models.Programme", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Programme_code")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Programme_title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("Programme_code")
                        .IsUnique();

                    b.ToTable("Programmes");
                });

            modelBuilder.Entity("comp2001hk_assessment2_part1.Models.Student", b =>
                {
                    b.Property<int>("Student_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Student_id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Student_id");

                    b.ToTable("Students");
                });

            modelBuilder.Entity("ProgrammeStudent", b =>
                {
                    b.Property<int>("ProgrammesId")
                        .HasColumnType("int");

                    b.Property<int>("StudentsStudent_id")
                        .HasColumnType("int");

                    b.HasKey("ProgrammesId", "StudentsStudent_id");

                    b.HasIndex("StudentsStudent_id");

                    b.ToTable("ProgrammeStudent");
                });

            modelBuilder.Entity("ProgrammeStudent", b =>
                {
                    b.HasOne("comp2001hk_assessment2_part1.Models.Programme", null)
                        .WithMany()
                        .HasForeignKey("ProgrammesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("comp2001hk_assessment2_part1.Models.Student", null)
                        .WithMany()
                        .HasForeignKey("StudentsStudent_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
