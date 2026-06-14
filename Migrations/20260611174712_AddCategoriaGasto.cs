using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjetoMidasAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddCategoriaGasto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoriaGasto",
                table: "Projecoes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CategoriaGasto",
                table: "Lancamentos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CategoriaGasto",
                table: "Emprestimos",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "IdUsuario",
                keyValue: 1,
                columns: new[] { "PasswordHash", "PasswordSalt" },
                values: new object[] { new byte[] { 31, 176, 78, 246, 24, 158, 89, 240, 110, 162, 223, 187, 94, 104, 176, 3, 66, 238, 147, 231, 4, 186, 202, 83, 165, 184, 72, 166, 216, 113, 242, 211, 158, 38, 74, 206, 35, 40, 250, 5, 94, 78, 144, 34, 50, 214, 241, 94, 241, 165, 139, 193, 15, 35, 222, 125, 22, 247, 70, 126, 70, 54, 102, 188 }, new byte[] { 124, 173, 72, 15, 148, 21, 220, 167, 39, 4, 13, 227, 149, 186, 70, 253, 127, 247, 237, 129, 230, 111, 151, 60, 215, 14, 148, 60, 24, 181, 252, 212, 166, 111, 223, 70, 8, 7, 155, 142, 101, 215, 253, 212, 58, 117, 195, 81, 207, 135, 186, 5, 5, 33, 116, 140, 71, 245, 104, 40, 148, 164, 52, 231, 61, 61, 6, 253, 143, 25, 135, 69, 168, 255, 12, 134, 38, 75, 99, 56, 115, 126, 33, 197, 192, 5, 142, 115, 187, 144, 38, 19, 111, 61, 73, 97, 130, 179, 79, 255, 233, 219, 97, 92, 204, 202, 22, 131, 45, 88, 62, 69, 244, 41, 12, 82, 106, 232, 236, 12, 58, 59, 239, 83, 53, 122, 143, 100 } });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CategoriaGasto",
                table: "Projecoes");

            migrationBuilder.DropColumn(
                name: "CategoriaGasto",
                table: "Lancamentos");

            migrationBuilder.DropColumn(
                name: "CategoriaGasto",
                table: "Emprestimos");

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "IdUsuario",
                keyValue: 1,
                columns: new[] { "PasswordHash", "PasswordSalt" },
                values: new object[] { new byte[] { 28, 28, 175, 250, 49, 154, 169, 106, 143, 219, 30, 202, 167, 87, 205, 76, 201, 181, 230, 67, 245, 50, 111, 213, 174, 250, 73, 93, 54, 116, 11, 211, 64, 217, 153, 25, 121, 75, 76, 3, 2, 165, 216, 243, 20, 218, 209, 199, 42, 16, 51, 36, 195, 144, 41, 146, 28, 57, 237, 122, 228, 137, 248, 150 }, new byte[] { 245, 192, 126, 163, 9, 179, 22, 69, 218, 86, 125, 147, 163, 40, 199, 70, 108, 32, 178, 238, 136, 232, 181, 35, 173, 133, 125, 75, 173, 23, 197, 40, 238, 234, 85, 43, 233, 182, 235, 51, 67, 65, 161, 84, 152, 35, 180, 13, 215, 141, 117, 206, 229, 162, 225, 241, 244, 147, 81, 163, 131, 210, 103, 5, 248, 53, 16, 71, 39, 138, 140, 185, 125, 149, 153, 57, 143, 170, 234, 146, 65, 183, 130, 183, 63, 37, 100, 45, 68, 94, 3, 226, 117, 106, 94, 131, 157, 162, 100, 247, 240, 175, 94, 205, 93, 0, 14, 55, 81, 147, 83, 139, 205, 251, 188, 92, 121, 9, 202, 49, 246, 4, 16, 47, 201, 208, 130, 61 } });
        }
    }
}
