using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProyectoVentaMusical.Data.Migrations
{
    /// <inheritdoc />
    public partial class IdUsuarioString : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Ventas__IdUsuari__3D5E1FD2",
                table: "Ventas");

            //migrationBuilder.DropIndex(
            //    name: "IX_Ventas_IdUsuario",
            //    table: "Ventas");

            migrationBuilder.AlterColumn<string>(
                name: "IdUsuario",
                table: "Ventas",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "UsuarioIdUsuario",
                table: "Ventas",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ventas_UsuarioIdUsuario",
                table: "Ventas",
                column: "UsuarioIdUsuario");

            migrationBuilder.AddForeignKey(
                name: "FK_Ventas_Usuario_UsuarioIdUsuario",
                table: "Ventas",
                column: "UsuarioIdUsuario",
                principalTable: "Usuario",
                principalColumn: "IdUsuario");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ventas_Usuario_UsuarioIdUsuario",
                table: "Ventas");

            migrationBuilder.DropIndex(
                name: "IX_Ventas_UsuarioIdUsuario",
                table: "Ventas");

            migrationBuilder.DropColumn(
                name: "UsuarioIdUsuario",
                table: "Ventas");

            migrationBuilder.AlterColumn<int>(
                name: "IdUsuario",
                table: "Ventas",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Ventas_IdUsuario",
                table: "Ventas",
                column: "IdUsuario");

            migrationBuilder.AddForeignKey(
                name: "FK__Ventas__IdUsuari__3D5E1FD2",
                table: "Ventas",
                column: "IdUsuario",
                principalTable: "Usuario",
                principalColumn: "IdUsuario");
        }
    }
}
