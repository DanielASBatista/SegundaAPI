using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ProjetoMidasAPI.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Empresas",
                columns: table => new
                {
                    IdEmpresa = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idResponsavel = table.Column<int>(type: "int", nullable: false),
                    razaoSocial = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    nomeFantasia = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    telefoneEmp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    cnpjEmpresa = table.Column<string>(type: "nvarchar(14)", maxLength: 14, nullable: false),
                    emailEmpresa = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Empresas", x => x.IdEmpresa);
                });

            migrationBuilder.CreateTable(
                name: "Responsaveis",
                columns: table => new
                {
                    IdResponsavel = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nomeResponsavel = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    sobrenomeResponsavel = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    telefoneResponsavel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    emailResponsavel = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Responsaveis", x => x.IdResponsavel);
                });

            migrationBuilder.CreateTable(
                name: "TipoRecorrencias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PadraoSistema = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoRecorrencias", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    IdUsuario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nomeUsuario = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    PasswordSalt = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    IdEmpresa = table.Column<int>(type: "int", nullable: false),
                    TipoUsuario = table.Column<int>(type: "int", nullable: false),
                    Perfil = table.Column<string>(type: "nvarchar(max)", nullable: true, defaultValue: "Visitante"),
                    sobrenome = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    emailUsuario = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    telefone = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.IdUsuario);
                });

            migrationBuilder.CreateTable(
                name: "Emprestimos",
                columns: table => new
                {
                    IdSimEmprestimo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdUsuario = table.Column<int>(type: "int", nullable: false),
                    nomeEmprestimo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    descricaoEmprestimo = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    provedorEmprestimo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    valorEmprestimo = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    parcelasEmprestimo = table.Column<int>(type: "int", nullable: false),
                    valorParcelas = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IOFemprestimo = table.Column<decimal>(type: "decimal(5,4)", nullable: false),
                    despesasEmprestimo = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    tarifasEmprestimo = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Data = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataCriacaoSE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsuarioResponsavel = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Emprestimos", x => x.IdSimEmprestimo);
                    table.ForeignKey(
                        name: "FK_Emprestimos_Usuarios_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "Usuarios",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Projecoes",
                columns: table => new
                {
                    IdProjecao = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdUsuario = table.Column<int>(type: "int", nullable: false),
                    Titulo = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ValorPrevisto = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DataReferencia = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsuarioResponsavel = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projecoes", x => x.IdProjecao);
                    table.ForeignKey(
                        name: "FK_Projecoes_Usuarios_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "Usuarios",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Recorrencias",
                columns: table => new
                {
                    IdRecorrencia = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdUsuario = table.Column<int>(type: "int", nullable: false),
                    IdProjecao = table.Column<int>(type: "int", nullable: true),
                    TipoLancamento = table.Column<int>(type: "int", nullable: true),
                    TipoRecorrenciaId = table.Column<int>(type: "int", nullable: true),
                    dsRecorrencia = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    obRecorrencia = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    dataInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    qtdeRecorrencia = table.Column<int>(type: "int", nullable: true),
                    Valor = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    momentoCriacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsuarioResponsavel = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recorrencias", x => x.IdRecorrencia);
                    table.ForeignKey(
                        name: "FK_Recorrencias_TipoRecorrencias_TipoRecorrenciaId",
                        column: x => x.TipoRecorrenciaId,
                        principalTable: "TipoRecorrencias",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Recorrencias_Usuarios_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "Usuarios",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Lancamentos",
                columns: table => new
                {
                    IdLancamento = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdUsuario = table.Column<int>(type: "int", nullable: false),
                    IdProjecao = table.Column<int>(type: "int", nullable: true),
                    IdSimEmprestimo = table.Column<int>(type: "int", nullable: true),
                    IdRecorrencia = table.Column<int>(type: "int", nullable: true),
                    TipoLancamento = table.Column<int>(type: "int", nullable: true),
                    OrigemLancamento = table.Column<int>(type: "int", nullable: true),
                    FrequenciaRecorrencia = table.Column<int>(type: "int", nullable: true),
                    ModoRecorrenciaMensal = table.Column<int>(type: "int", nullable: true),
                    StatusTransacao = table.Column<int>(type: "int", nullable: true),
                    QtdeRecorrencia = table.Column<int>(type: "int", nullable: true),
                    DescricaoLancamento = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ObservacaoLancamento = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Valor = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Data = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NumeroDaOcorrencia = table.Column<int>(type: "int", nullable: true),
                    TotalOcorrencia = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lancamentos", x => x.IdLancamento);
                    table.ForeignKey(
                        name: "FK_Lancamentos_Recorrencias_IdRecorrencia",
                        column: x => x.IdRecorrencia,
                        principalTable: "Recorrencias",
                        principalColumn: "IdRecorrencia");
                    table.ForeignKey(
                        name: "FK_Lancamentos_Usuarios_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "Usuarios",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Empresas",
                columns: new[] { "IdEmpresa", "cnpjEmpresa", "emailEmpresa", "idResponsavel", "nomeFantasia", "razaoSocial", "telefoneEmp" },
                values: new object[] { 1, "12345678901234", "empresa@teste.com", 1, "Teste", "Empresa Teste", "123456789" });

            migrationBuilder.InsertData(
                table: "Responsaveis",
                columns: new[] { "IdResponsavel", "emailResponsavel", "nomeResponsavel", "sobrenomeResponsavel", "telefoneResponsavel" },
                values: new object[] { 1, "joao.silva@teste.com", "João", "Silva", "987654321" });

            migrationBuilder.InsertData(
                table: "TipoRecorrencias",
                columns: new[] { "Id", "Nome", "PadraoSistema" },
                values: new object[,]
                {
                    { 1, "Mensal", true },
                    { 2, "Semanal", true },
                    { 3, "Anual", true }
                });

            migrationBuilder.InsertData(
                table: "Usuarios",
                columns: new[] { "IdUsuario", "IdEmpresa", "PasswordHash", "PasswordSalt", "Perfil", "TipoUsuario", "emailUsuario", "nomeUsuario", "sobrenome", "telefone" },
                values: new object[] { 1, 0, new byte[] { 28, 28, 175, 250, 49, 154, 169, 106, 143, 219, 30, 202, 167, 87, 205, 76, 201, 181, 230, 67, 245, 50, 111, 213, 174, 250, 73, 93, 54, 116, 11, 211, 64, 217, 153, 25, 121, 75, 76, 3, 2, 165, 216, 243, 20, 218, 209, 199, 42, 16, 51, 36, 195, 144, 41, 146, 28, 57, 237, 122, 228, 137, 248, 150 }, new byte[] { 245, 192, 126, 163, 9, 179, 22, 69, 218, 86, 125, 147, 163, 40, 199, 70, 108, 32, 178, 238, 136, 232, 181, 35, 173, 133, 125, 75, 173, 23, 197, 40, 238, 234, 85, 43, 233, 182, 235, 51, 67, 65, 161, 84, 152, 35, 180, 13, 215, 141, 117, 206, 229, 162, 225, 241, 244, 147, 81, 163, 131, 210, 103, 5, 248, 53, 16, 71, 39, 138, 140, 185, 125, 149, 153, 57, 143, 170, 234, 146, 65, 183, 130, 183, 63, 37, 100, 45, 68, 94, 3, 226, 117, 106, 94, 131, 157, 162, 100, 247, 240, 175, 94, 205, 93, 0, 14, 55, 81, 147, 83, 139, 205, 251, 188, 92, 121, 9, 202, 49, 246, 4, 16, 47, 201, 208, 130, 61 }, "Administrador", 7, "", "Admin", "", "" });

            migrationBuilder.CreateIndex(
                name: "IX_Emprestimos_IdUsuario",
                table: "Emprestimos",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_Lancamentos_IdRecorrencia",
                table: "Lancamentos",
                column: "IdRecorrencia");

            migrationBuilder.CreateIndex(
                name: "IX_Lancamentos_IdUsuario",
                table: "Lancamentos",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_Projecoes_IdUsuario",
                table: "Projecoes",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_Recorrencias_IdUsuario",
                table: "Recorrencias",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_Recorrencias_TipoRecorrenciaId",
                table: "Recorrencias",
                column: "TipoRecorrenciaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Empresas");

            migrationBuilder.DropTable(
                name: "Emprestimos");

            migrationBuilder.DropTable(
                name: "Lancamentos");

            migrationBuilder.DropTable(
                name: "Projecoes");

            migrationBuilder.DropTable(
                name: "Responsaveis");

            migrationBuilder.DropTable(
                name: "Recorrencias");

            migrationBuilder.DropTable(
                name: "TipoRecorrencias");

            migrationBuilder.DropTable(
                name: "Usuarios");
        }
    }
}
