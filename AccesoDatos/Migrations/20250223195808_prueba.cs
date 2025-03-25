using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProyectoVentaMusical.Data.Migrations
{
    /// <inheritdoc />
    public partial class prueba : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarritoCompras_AspNetUsers",
                table: "CarritoCompras");

            migrationBuilder.DropForeignKey(
                name: "FK__Ventas__IdUsuari__3D5E1FD2",
                table: "Ventas");

            migrationBuilder.DropTable(
                name: "identityUsers");

            migrationBuilder.DropTable(
                name: "Usuario");

            //migrationBuilder.DropIndex(
            //    name: "IX_CarritoCompras_IdUsuario",
            //    table: "CarritoCompras");

            migrationBuilder.DropIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles");

            migrationBuilder.AlterColumn<string>(
                name: "IdUsuario",
                table: "CarritoCompras",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");            

            migrationBuilder.AddColumn<int>(
                name: "UsuarioIdUsuario",
                table: "CarritoCompras",
                type: "int",
                nullable: true);


            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUserTokens",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserTokens",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "AspNetUserLogins",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserLogins",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.CreateTable(
                name: "Usuario",
                columns: table => new
                {
                    IdUsuario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumeroIdentificacion = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    NombreCompleto = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    Genero = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    CorreoElectronico = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    TipoTarjeta = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    DineroDisponible = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    NumeroTarjeta = table.Column<string>(type: "varchar(19)", unicode: false, maxLength: 19, nullable: false),
                    Contraseña = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    IdPerfil = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Usuarios__5B65BF971D6D212C", x => x.IdUsuario);
                    table.ForeignKey(
                        name: "FK__Usuarios__IdPerf__2B3F6F97",
                        column: x => x.IdPerfil,
                        principalTable: "Perfiles",
                        principalColumn: "IdPerfil");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CarritoCompras_UsuarioIdUsuario",
                table: "CarritoCompras",
                column: "UsuarioIdUsuario");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_IdPerfil",
                table: "Usuario",
                column: "IdPerfil");

            migrationBuilder.CreateIndex(
                name: "UQ__Usuarios__531402F345CE6B0B",
                table: "Usuario",
                column: "CorreoElectronico",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__Usuarios__FCA68D91346B8688",
                table: "Usuario",
                column: "NumeroIdentificacion",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CarritoCompras_Usuario_UsuarioIdUsuario",
                table: "CarritoCompras",
                column: "UsuarioIdUsuario",
                principalTable: "Usuario",
                principalColumn: "IdUsuario");

            migrationBuilder.AddForeignKey(
                name: "FK__Ventas__IdUsuari__3D5E1FD2",
                table: "Ventas",
                column: "IdUsuario",
                principalTable: "Usuario",
                principalColumn: "IdUsuario");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarritoCompras_Usuario_UsuarioIdUsuario",
                table: "CarritoCompras");

            migrationBuilder.DropForeignKey(
                name: "FK__Ventas__IdUsuari__3D5E1FD2",
                table: "Ventas");

            migrationBuilder.DropTable(
                name: "Usuario");

            migrationBuilder.DropIndex(
                name: "IX_CarritoCompras_UsuarioIdUsuario",
                table: "CarritoCompras");

            migrationBuilder.DropIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles");


            migrationBuilder.DropColumn(
                name: "UsuarioIdUsuario",
                table: "CarritoCompras");

            migrationBuilder.DropColumn(
                name: "fotoCancion",
                table: "Canciones");

            migrationBuilder.AlterColumn<int>(
                name: "IdUsuario",
                table: "CarritoCompras",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUserTokens",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserTokens",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "AspNetUserLogins",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserLogins",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateTable(
                name: "identityUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Discriminator = table.Column<string>(type: "nvarchar(21)", maxLength: 21, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ciudad = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Direccion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Pais = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_identityUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Usuario",   
                columns: table => new
                {
                    IdUsuario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdPerfil = table.Column<int>(type: "int", nullable: false),
                    Contraseña = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    CorreoElectronico = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    DineroDisponible = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Genero = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    NombreCompleto = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    NumeroIdentificacion = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    NumeroTarjeta = table.Column<string>(type: "varchar(19)", unicode: false, maxLength: 19, nullable: false),
                    TipoTarjeta = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Usuarios__5B65BF971D6D212C", x => x.IdUsuario);
                    table.ForeignKey(
                        name: "FK__Usuarios__IdPerf__2B3F6F97",
                        column: x => x.IdPerfil,
                        principalTable: "Perfiles",
                        principalColumn: "IdPerfil");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CarritoCompras_IdUsuario",
                table: "CarritoCompras",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "([NormalizedName] IS NOT NULL)");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_IdPerfil",
                table: "Usuario",
                column: "IdPerfil");

            migrationBuilder.CreateIndex(
                name: "UQ__Usuarios__531402F345CE6B0B",
                table: "Usuario",
                column: "CorreoElectronico",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__Usuarios__FCA68D91346B8688",
                table: "Usuario",
                column: "NumeroIdentificacion",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK__CarritoCo__IdUsu__38996AB5",
                table: "CarritoCompras",
                column: "IdUsuario",
                principalTable: "Usuario",
                principalColumn: "IdUsuario");

            migrationBuilder.AddForeignKey(
                name: "FK__Ventas__IdUsuari__3D5E1FD2",
                table: "Ventas",
                column: "IdUsuario",
                principalTable: "Usuario",
                principalColumn: "IdUsuario");
        }
    }
}
