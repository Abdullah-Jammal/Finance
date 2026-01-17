using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Finance.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class adddeletedAtincompany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                schema: "core",
                table: "companies",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedAt",
                schema: "core",
                table: "companies");
        }
    }
}
