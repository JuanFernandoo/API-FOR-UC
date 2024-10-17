using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace For_u.Migrations
{
    /// <inheritdoc />
    public partial class AddUsuarioToUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Apellido = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Usuario = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Contraseña = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Comunidades",
                columns: table => new
                {
                    ComunidadId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TituloComunidad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DescripcionComunidad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ComunidadCreadaPor = table.Column<int>(type: "int", nullable: false),
                    FechaCreacionComunidad = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comunidades", x => x.ComunidadId);
                    table.ForeignKey(
                        name: "FK_Comunidades_Users_ComunidadCreadaPor",
                        column: x => x.ComunidadCreadaPor,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Posts",
                columns: table => new
                {
                    postId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tituloPost = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    descripcionPost = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    postCreadoPor = table.Column<int>(type: "int", nullable: false),
                    comunidadId = table.Column<int>(type: "int", nullable: false),
                    fechaCreacionPost = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.postId);
                    table.ForeignKey(
                        name: "FK_Posts_Comunidades_comunidadId",
                        column: x => x.comunidadId,
                        principalTable: "Comunidades",
                        principalColumn: "ComunidadId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Posts_Users_postCreadoPor",
                        column: x => x.postCreadoPor,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Comentarios",
                columns: table => new
                {
                    ComentarioId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ComentarioTexto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ComentarioCreadoPor = table.Column<int>(type: "int", nullable: false),
                    PostId = table.Column<int>(type: "int", nullable: false),
                    FechaCreacionComentario = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comentarios", x => x.ComentarioId);
                    table.ForeignKey(
                        name: "FK_Comentarios_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "postId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Comentarios_Users_ComentarioCreadoPor",
                        column: x => x.ComentarioCreadoPor,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comentarios_ComentarioCreadoPor",
                table: "Comentarios",
                column: "ComentarioCreadoPor");

            migrationBuilder.CreateIndex(
                name: "IX_Comentarios_PostId",
                table: "Comentarios",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_Comunidades_ComunidadCreadaPor",
                table: "Comunidades",
                column: "ComunidadCreadaPor");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_comunidadId",
                table: "Posts",
                column: "comunidadId");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_postCreadoPor",
                table: "Posts",
                column: "postCreadoPor");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comentarios");

            migrationBuilder.DropTable(
                name: "Posts");

            migrationBuilder.DropTable(
                name: "Comunidades");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
