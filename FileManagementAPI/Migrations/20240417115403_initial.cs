using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FileManagementAPI.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "createdAt",
                table: "DocumentAttachments",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "parent",
                table: "DocumentAttachments",
                newName: "Path");

            migrationBuilder.RenameColumn(
                name: "FilePath",
                table: "DocumentAttachments",
                newName: "ParentId");

            migrationBuilder.RenameColumn(
                name: "FileName",
                table: "DocumentAttachments",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "FileExtension",
                table: "DocumentAttachments",
                newName: "CreatedBy");

            migrationBuilder.AddColumn<byte[]>(
                name: "Data",
                table: "DocumentAttachments",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "DocumentAttachments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Data",
                table: "DocumentAttachments");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "DocumentAttachments");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "DocumentAttachments",
                newName: "createdAt");

            migrationBuilder.RenameColumn(
                name: "Path",
                table: "DocumentAttachments",
                newName: "parent");

            migrationBuilder.RenameColumn(
                name: "ParentId",
                table: "DocumentAttachments",
                newName: "FilePath");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "DocumentAttachments",
                newName: "FileName");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "DocumentAttachments",
                newName: "FileExtension");
        }
    }
}
