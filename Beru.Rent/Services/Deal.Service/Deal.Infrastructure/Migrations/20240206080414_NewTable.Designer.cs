﻿// <auto-generated />
using System;
using Deal.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Deal.Infrastructure.Migrations
{
    [DbContext(typeof(DealContext))]
    [Migration("20240206080414_NewTable")]
    partial class NewTable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Deal.Domain.Models.Booking", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("AdId")
                        .HasColumnType("uuid");

                    b.Property<string>("BookingState")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime?>("CancelAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<decimal?>("Cost")
                        .HasColumnType("numeric");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("Dbeg")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("Dend")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("OwnerId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("TenantId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Bookings");
                });

            modelBuilder.Entity("Deal.Domain.Models.ContractStorage", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("DealId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("SignedByOwnerPath")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("SignedByTenanPath")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("TemplatePath")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("ContractStorages");
                });

            modelBuilder.Entity("Deal.Domain.Models.Deal", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("AdId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("BookingId")
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("CancelAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid>("ChatId")
                        .HasColumnType("uuid");

                    b.Property<decimal?>("Cost")
                        .HasColumnType("numeric");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("Dbeg")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("DealState")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("Dend")
                        .HasColumnType("timestamp without time zone");

                    b.Property<decimal>("Deposit")
                        .HasColumnType("numeric");

                    b.Property<string>("OwnerId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("TenantId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Deals");
                });
#pragma warning restore 612, 618
        }
    }
}