using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TemplateManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HtmlTemplates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HtmlTemplates", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "HtmlTemplates",
                columns: new[] { "Id", "Content", "Name" },
                values: new object[,]
                {
                    { new Guid("a1b2c3d4-5e6f-7a8b-9c0d-1e2f3a4b5c6d"), "<div><h2>Certificate of Completion</h2><p>This certifies that {{Name}} completed the course on {{Date}}</p></div>", "Certificate Template" },
                    { new Guid("d3f6a1e4-8c2f-4a4d-a8e1-1b2c3d4e5f60"), "<h1>Invoice for {{Name}}</h1><p>Date: {{Date}}</p><p>Amount: {{Amount}}</p>", "Invoice Template" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HtmlTemplates");
        }
    }
}
