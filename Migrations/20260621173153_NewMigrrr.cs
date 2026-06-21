using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ProjetoMidasAPI.Migrations
{
    /// <inheritdoc />
    public partial class NewMigrrr : Migration
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
                    UsuarioResponsavel = table.Column<int>(type: "int", nullable: true),
                    CategoriaGasto = table.Column<int>(type: "int", nullable: true)
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
                    UsuarioResponsavel = table.Column<int>(type: "int", nullable: true),
                    CategoriaGasto = table.Column<int>(type: "int", nullable: true)
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
                    CategoriaGasto = table.Column<int>(type: "int", nullable: true),
                    StatusTransacao = table.Column<int>(type: "int", nullable: true),
                    QtdeRecorrencia = table.Column<int>(type: "int", nullable: true),
                    DescricaoLancamento = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
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
                values: new object[] { 1, 0, new byte[] { 229, 210, 191, 104, 74, 144, 154, 164, 1, 138, 70, 184, 169, 194, 204, 54, 237, 84, 254, 69, 151, 253, 236, 78, 165, 250, 136, 143, 142, 235, 164, 130, 102, 90, 203, 120, 79, 206, 57, 61, 194, 119, 207, 83, 4, 88, 62, 227, 82, 143, 138, 240, 171, 90, 184, 65, 35, 43, 62, 91, 195, 186, 136, 227 }, new byte[] { 133, 150, 240, 118, 51, 92, 188, 150, 165, 147, 35, 230, 110, 8, 92, 191, 4, 86, 8, 12, 129, 244, 39, 175, 182, 203, 98, 74, 132, 183, 43, 207, 7, 216, 177, 5, 117, 6, 241, 53, 94, 166, 168, 132, 82, 249, 227, 253, 22, 157, 122, 107, 15, 182, 186, 196, 234, 42, 197, 199, 186, 251, 167, 0, 57, 236, 177, 29, 183, 234, 106, 112, 120, 55, 186, 81, 128, 31, 198, 39, 193, 70, 121, 134, 49, 240, 51, 149, 129, 183, 202, 76, 95, 112, 220, 70, 26, 188, 178, 157, 203, 26, 156, 20, 138, 122, 227, 220, 142, 238, 3, 78, 139, 200, 219, 153, 133, 123, 35, 68, 74, 70, 79, 33, 173, 90, 244, 198 }, "Administrador", 7, "", "Admin", "", "" });

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
