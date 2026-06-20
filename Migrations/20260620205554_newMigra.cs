using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjetoMidasAPI.Migrations
{
    /// <inheritdoc />
    public partial class newMigra : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "DescricaoLancamento",
                table: "Lancamentos",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "IdUsuario",
                keyValue: 1,
                columns: new[] { "PasswordHash", "PasswordSalt" },
                values: new object[] { new byte[] { 148, 219, 246, 190, 170, 216, 190, 47, 42, 103, 100, 64, 134, 32, 188, 244, 82, 253, 92, 189, 233, 236, 85, 123, 100, 41, 203, 169, 230, 226, 48, 157, 78, 80, 177, 160, 254, 50, 84, 165, 105, 208, 113, 228, 246, 17, 198, 16, 154, 68, 144, 41, 232, 90, 248, 178, 101, 239, 196, 143, 18, 59, 165, 89 }, new byte[] { 107, 104, 115, 176, 228, 5, 2, 65, 152, 248, 197, 107, 126, 133, 118, 42, 19, 174, 189, 139, 125, 164, 112, 219, 1, 191, 22, 68, 188, 114, 226, 46, 83, 92, 34, 59, 182, 188, 162, 97, 202, 48, 229, 85, 82, 245, 240, 125, 228, 133, 75, 239, 136, 88, 180, 122, 59, 137, 34, 55, 155, 249, 32, 99, 209, 202, 148, 103, 221, 14, 33, 134, 64, 168, 9, 8, 105, 94, 17, 25, 93, 174, 157, 55, 143, 73, 106, 49, 75, 125, 131, 181, 83, 114, 172, 38, 190, 97, 170, 74, 17, 211, 221, 89, 227, 255, 1, 48, 178, 236, 185, 106, 63, 147, 124, 85, 210, 238, 51, 210, 123, 233, 61, 29, 46, 131, 225, 49 } });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "DescricaoLancamento",
                table: "Lancamentos",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "IdUsuario",
                keyValue: 1,
                columns: new[] { "PasswordHash", "PasswordSalt" },
                values: new object[] { new byte[] { 225, 249, 201, 16, 77, 41, 70, 199, 240, 104, 204, 82, 29, 207, 95, 76, 67, 219, 110, 54, 90, 135, 195, 86, 63, 34, 42, 11, 100, 178, 233, 116, 239, 163, 162, 37, 204, 17, 37, 214, 2, 119, 56, 64, 237, 84, 133, 30, 136, 193, 180, 154, 78, 140, 70, 9, 117, 241, 7, 78, 195, 184, 157, 102 }, new byte[] { 164, 106, 24, 88, 210, 75, 169, 207, 227, 130, 222, 150, 139, 187, 210, 140, 1, 51, 250, 197, 71, 247, 129, 181, 219, 253, 138, 145, 24, 86, 154, 169, 229, 8, 198, 93, 95, 51, 33, 240, 242, 254, 189, 190, 120, 149, 97, 71, 248, 24, 166, 100, 188, 95, 219, 203, 240, 72, 189, 223, 130, 157, 184, 53, 66, 53, 160, 65, 52, 175, 42, 31, 165, 176, 58, 254, 125, 140, 22, 125, 234, 177, 17, 105, 178, 67, 113, 232, 166, 200, 65, 252, 227, 168, 35, 226, 93, 9, 70, 218, 37, 155, 221, 249, 73, 98, 60, 39, 162, 215, 225, 22, 66, 34, 56, 185, 85, 199, 56, 14, 157, 245, 245, 102, 178, 61, 133, 223 } });
        }
    }
}
