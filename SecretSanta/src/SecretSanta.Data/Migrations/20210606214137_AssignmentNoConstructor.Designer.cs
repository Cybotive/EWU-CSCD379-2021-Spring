﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SecretSanta.Data;

namespace SecretSanta.Data.Migrations
{
    [DbContext(typeof(SecretSantaContext))]
    [Migration("20210606214137_AssignmentNoConstructor")]
    partial class AssignmentNoConstructor
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.6");

            modelBuilder.Entity("GroupUser", b =>
                {
                    b.Property<int>("GroupsId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("UsersId")
                        .HasColumnType("INTEGER");

                    b.HasKey("GroupsId", "UsersId");

                    b.HasIndex("UsersId");

                    b.ToTable("GroupUser");
                });

            modelBuilder.Entity("SecretSanta.Data.Assignment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("GiverId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("GroupId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("ReceiverId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("GiverId");

                    b.HasIndex("GroupId");

                    b.HasIndex("ReceiverId");

                    b.ToTable("Assignment");
                });

            modelBuilder.Entity("SecretSanta.Data.Gift", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<int>("Priority")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("ReceiverId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Url")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ReceiverId");

                    b.ToTable("Gifts");
                });

            modelBuilder.Entity("SecretSanta.Data.Group", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("SecretSanta.Data.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasAlternateKey("FirstName", "LastName");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("GroupUser", b =>
                {
                    b.HasOne("SecretSanta.Data.Group", null)
                        .WithMany()
                        .HasForeignKey("GroupsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SecretSanta.Data.User", null)
                        .WithMany()
                        .HasForeignKey("UsersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SecretSanta.Data.Assignment", b =>
                {
                    b.HasOne("SecretSanta.Data.User", "Giver")
                        .WithMany()
                        .HasForeignKey("GiverId");

                    b.HasOne("SecretSanta.Data.Group", null)
                        .WithMany("Assignments")
                        .HasForeignKey("GroupId");

                    b.HasOne("SecretSanta.Data.User", "Receiver")
                        .WithMany()
                        .HasForeignKey("ReceiverId");

                    b.Navigation("Giver");

                    b.Navigation("Receiver");
                });

            modelBuilder.Entity("SecretSanta.Data.Gift", b =>
                {
                    b.HasOne("SecretSanta.Data.User", "Receiver")
                        .WithMany()
                        .HasForeignKey("ReceiverId");

                    b.Navigation("Receiver");
                });

            modelBuilder.Entity("SecretSanta.Data.Group", b =>
                {
                    b.Navigation("Assignments");
                });
#pragma warning restore 612, 618
        }
    }
}
