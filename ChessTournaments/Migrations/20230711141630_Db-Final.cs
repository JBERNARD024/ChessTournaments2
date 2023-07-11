using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChessTournaments.Migrations
{
    /// <inheritdoc />
    public partial class DbFinal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fotografia_Equipa_EquipaFK",
                table: "Fotografia");

            migrationBuilder.DropForeignKey(
                name: "FK_Fotografia_Pessoa_PessoaFK",
                table: "Fotografia");

            migrationBuilder.AlterColumn<string>(
                name: "CodPostal",
                table: "Pessoa",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<int>(
                name: "PessoaFK",
                table: "Fotografia",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "EquipaFK",
                table: "Fotografia",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Fotografia_Equipa_EquipaFK",
                table: "Fotografia",
                column: "EquipaFK",
                principalTable: "Equipa",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Fotografia_Pessoa_PessoaFK",
                table: "Fotografia",
                column: "PessoaFK",
                principalTable: "Pessoa",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fotografia_Equipa_EquipaFK",
                table: "Fotografia");

            migrationBuilder.DropForeignKey(
                name: "FK_Fotografia_Pessoa_PessoaFK",
                table: "Fotografia");

            migrationBuilder.AlterColumn<string>(
                name: "CodPostal",
                table: "Pessoa",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<int>(
                name: "PessoaFK",
                table: "Fotografia",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "EquipaFK",
                table: "Fotografia",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Fotografia_Equipa_EquipaFK",
                table: "Fotografia",
                column: "EquipaFK",
                principalTable: "Equipa",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Fotografia_Pessoa_PessoaFK",
                table: "Fotografia",
                column: "PessoaFK",
                principalTable: "Pessoa",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
