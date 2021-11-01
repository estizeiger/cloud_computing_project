﻿// <auto-generated />
using IceCreamProject.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace IceCreamProject.Migrations
{
    [DbContext(typeof(IceCreamProjectContext))]
    [Migration("20211022125541_OrderDetail_Create")]
    partial class OrderDetail_Create
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.11")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("IceCreamProject.Models.IceCream", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Flavor")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImgLocation")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.Property<string>("Temperature")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("IceCream");
                });

            modelBuilder.Entity("IceCreamProject.Models.OrderDetail", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("City")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Day")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("House")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Month")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.Property<string>("Street")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TasteId")
                        .HasColumnType("int");

                    b.Property<double>("Temperature")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.ToTable("OrderDetail");
                });

            modelBuilder.Entity("IceCreamProject.Models.OrderOfUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("City")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Day")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("House")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Month")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.Property<string>("Street")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TasteId")
                        .HasColumnType("int");

                    b.Property<double>("Temperature")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.ToTable("OrderOfUser");
                });

            modelBuilder.Entity("IceCreamProject.Models.Taste", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImgLocation")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Taste");
                });
#pragma warning restore 612, 618
        }
    }
}
