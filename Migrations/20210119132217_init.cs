using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace AppTest.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Login = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Email = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "bytea", nullable: true),
                    PasswordSalt = table.Column<byte[]>(type: "bytea", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RoleUser",
                columns: table => new
                {
                    RolesId = table.Column<int>(type: "integer", nullable: false),
                    UsersId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleUser", x => new { x.RolesId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_RoleUser_Roles_RolesId",
                        column: x => x.RolesId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoleUser_Users_UsersId",
                        column: x => x.UsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Admin" },
                    { 2, "Redactor" },
                    { 3, "Customer" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "Login", "Name", "PasswordHash", "PasswordSalt" },
                values: new object[] { 1, "admin@admin.ru", "admin", "Name", new byte[] { 148, 24, 237, 69, 73, 90, 155, 15, 222, 84, 15, 2, 30, 6, 254, 14, 227, 158, 183, 167, 16, 218, 30, 247, 165, 88, 0, 227, 194, 172, 65, 10, 253, 241, 91, 18, 239, 114, 140, 88, 213, 132, 33, 1, 249, 18, 1, 83, 34, 86, 55, 148, 53, 173, 13, 227, 29, 105, 166, 11, 12, 6, 224, 241 }, new byte[] { 222, 30, 170, 27, 58, 113, 187, 59, 98, 246, 178, 55, 77, 129, 106, 154, 200, 12, 240, 139, 116, 109, 93, 34, 51, 232, 28, 42, 203, 50, 201, 236, 36, 162, 227, 189, 108, 234, 230, 163, 48, 147, 76, 26, 41, 157, 127, 169, 172, 13, 178, 86, 228, 194, 231, 227, 74, 154, 60, 183, 164, 104, 15, 77, 220, 51, 27, 253, 52, 56, 134, 67, 157, 87, 108, 215, 158, 167, 72, 186, 171, 33, 200, 58, 27, 70, 213, 160, 126, 0, 196, 240, 90, 222, 82, 70, 178, 29, 169, 109, 106, 87, 247, 91, 70, 80, 136, 52, 43, 33, 33, 202, 160, 193, 65, 9, 243, 205, 45, 216, 117, 54, 39, 83, 239, 98, 64, 67 } });

            migrationBuilder.CreateIndex(
                name: "IX_RoleUser_UsersId",
                table: "RoleUser",
                column: "UsersId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoleUser");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
