﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.super_chatbot.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumeroTelefonico = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Id_Telefone_Meta = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TokenMeta = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WebHookToken = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Clients");
        }
    }
}
