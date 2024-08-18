using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FirstDemo.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class InsertCourseData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Courses",
                columns: new[] { "Id", "Description", "Fees", "Title" },
                values: new object[,]
                {
                    { new Guid("003805c3-938c-43b7-a768-03d6c0242ece"), "Test", 3000L, "Test Course 2" },
                    { new Guid("aff7c36d-6dc1-48b6-9793-323d66ddeb35"), "Test", 2000L, "Test Course 1" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: new Guid("003805c3-938c-43b7-a768-03d6c0242ece"));

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: new Guid("aff7c36d-6dc1-48b6-9793-323d66ddeb35"));
        }
    }
}
