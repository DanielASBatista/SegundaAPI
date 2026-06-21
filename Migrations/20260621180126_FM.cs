using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjetoMidasAPI.Migrations
{
    /// <inheritdoc />
    public partial class FM : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "TipoRecorrencias",
                keyColumn: "Id",
                keyValue: 1,
                column: "Nome",
                value: "Diária");

            migrationBuilder.UpdateData(
                table: "TipoRecorrencias",
                keyColumn: "Id",
                keyValue: 3,
                column: "Nome",
                value: "Mensal");

            migrationBuilder.InsertData(
                table: "TipoRecorrencias",
                columns: new[] { "Id", "Nome", "PadraoSistema" },
                values: new object[] { 4, "Anual", true });

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "IdUsuario",
                keyValue: 1,
                columns: new[] { "PasswordHash", "PasswordSalt" },
                values: new object[] { new byte[] { 222, 216, 41, 212, 92, 86, 251, 223, 174, 99, 63, 70, 82, 188, 253, 242, 232, 51, 238, 249, 188, 48, 57, 231, 99, 2, 100, 51, 123, 7, 115, 133, 150, 145, 198, 207, 177, 15, 20, 215, 231, 143, 49, 55, 70, 65, 231, 206, 58, 73, 65, 235, 251, 174, 170, 80, 11, 132, 114, 79, 50, 177, 152, 189 }, new byte[] { 96, 212, 164, 102, 163, 45, 167, 196, 35, 167, 191, 228, 161, 56, 101, 64, 97, 48, 210, 99, 245, 144, 52, 178, 134, 233, 212, 149, 159, 14, 223, 159, 202, 153, 120, 229, 88, 179, 137, 94, 37, 172, 233, 170, 50, 74, 57, 198, 228, 190, 79, 164, 32, 195, 68, 105, 36, 98, 226, 23, 229, 56, 36, 216, 25, 179, 6, 94, 237, 102, 13, 88, 242, 224, 68, 68, 60, 81, 145, 180, 17, 4, 10, 119, 201, 201, 158, 248, 99, 105, 238, 159, 26, 139, 158, 71, 17, 92, 127, 130, 65, 238, 149, 10, 27, 10, 79, 53, 212, 99, 25, 255, 85, 16, 68, 37, 75, 230, 46, 25, 198, 157, 227, 171, 218, 31, 126, 28 } });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "TipoRecorrencias",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.UpdateData(
                table: "TipoRecorrencias",
                keyColumn: "Id",
                keyValue: 1,
                column: "Nome",
                value: "Mensal");

            migrationBuilder.UpdateData(
                table: "TipoRecorrencias",
                keyColumn: "Id",
                keyValue: 3,
                column: "Nome",
                value: "Anual");

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "IdUsuario",
                keyValue: 1,
                columns: new[] { "PasswordHash", "PasswordSalt" },
                values: new object[] { new byte[] { 29, 181, 226, 114, 108, 145, 2, 48, 230, 57, 135, 208, 93, 214, 254, 244, 25, 149, 3, 160, 243, 143, 73, 140, 3, 75, 78, 15, 197, 57, 228, 78, 181, 252, 65, 159, 177, 28, 251, 76, 199, 237, 228, 112, 112, 72, 63, 6, 159, 70, 44, 142, 13, 95, 23, 166, 134, 178, 193, 231, 101, 15, 72, 146 }, new byte[] { 7, 165, 143, 2, 60, 238, 255, 112, 189, 33, 100, 13, 62, 18, 186, 93, 251, 149, 206, 87, 46, 218, 186, 210, 3, 110, 242, 1, 119, 216, 197, 131, 95, 141, 85, 118, 101, 144, 38, 23, 251, 210, 109, 14, 66, 85, 12, 93, 107, 61, 131, 228, 26, 12, 203, 228, 64, 52, 160, 32, 73, 7, 117, 219, 40, 140, 82, 179, 224, 131, 199, 197, 56, 119, 35, 135, 31, 12, 11, 199, 91, 216, 44, 165, 164, 176, 169, 241, 119, 174, 21, 108, 238, 14, 135, 202, 63, 245, 26, 80, 188, 151, 113, 128, 11, 85, 101, 42, 61, 153, 0, 91, 209, 237, 177, 118, 47, 225, 155, 94, 250, 10, 120, 70, 120, 120, 230, 155 } });
        }
    }
}
