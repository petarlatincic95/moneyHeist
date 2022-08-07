﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MoneyHeistAPI.Data;

#nullable disable

namespace MoneyHeistAPI.Migrations
{
    [DbContext(typeof(HeistDbContext))]
    [Migration("20220412175517_Members_Skills_MtoN_Relationship_Setting")]
    partial class Members_Skills_MtoN_Relationship_Setting
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("HeistMemberMemberSkill", b =>
                {
                    b.Property<int>("HeistMembersId")
                        .HasColumnType("int");

                    b.Property<int>("MemberSkillsId")
                        .HasColumnType("int");

                    b.HasKey("HeistMembersId", "MemberSkillsId");

                    b.HasIndex("MemberSkillsId");

                    b.ToTable("HeistMemberMemberSkill");
                });

            modelBuilder.Entity("MoneyHeistAPI.Model.HeistMember", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MainSkill")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Sex")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StatusField")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("HeistMembers");
                });

            modelBuilder.Entity("MoneyHeistAPI.Model.MemberSkill", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("SkillLevel")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SkillName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("MemberSkills");
                });

            modelBuilder.Entity("HeistMemberMemberSkill", b =>
                {
                    b.HasOne("MoneyHeistAPI.Model.HeistMember", null)
                        .WithMany()
                        .HasForeignKey("HeistMembersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MoneyHeistAPI.Model.MemberSkill", null)
                        .WithMany()
                        .HasForeignKey("MemberSkillsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
