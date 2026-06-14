IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260513003832_Init'
)
BEGIN
    CREATE TABLE [Empresas] (
        [IdEmpresa] int NOT NULL IDENTITY,
        [idResponsavel] int NOT NULL,
        [razaoSocial] nvarchar(80) NOT NULL,
        [nomeFantasia] nvarchar(50) NOT NULL,
        [telefoneEmp] nvarchar(max) NULL,
        [cnpjEmpresa] nvarchar(14) NOT NULL,
        [emailEmpresa] nvarchar(100) NULL,
        CONSTRAINT [PK_Empresas] PRIMARY KEY ([IdEmpresa])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260513003832_Init'
)
BEGIN
    CREATE TABLE [Responsaveis] (
        [IdResponsavel] int NOT NULL IDENTITY,
        [nomeResponsavel] nvarchar(20) NOT NULL,
        [sobrenomeResponsavel] nvarchar(80) NOT NULL,
        [telefoneResponsavel] nvarchar(max) NULL,
        [emailResponsavel] nvarchar(100) NOT NULL,
        CONSTRAINT [PK_Responsaveis] PRIMARY KEY ([IdResponsavel])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260513003832_Init'
)
BEGIN
    CREATE TABLE [TipoRecorrencias] (
        [Id] int NOT NULL IDENTITY,
        [Nome] nvarchar(50) NOT NULL,
        [PadraoSistema] bit NOT NULL,
        CONSTRAINT [PK_TipoRecorrencias] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260513003832_Init'
)
BEGIN
    CREATE TABLE [Usuarios] (
        [IdUsuario] int NOT NULL IDENTITY,
        [nomeUsuario] nvarchar(50) NOT NULL,
        [PasswordHash] varbinary(max) NULL,
        [PasswordSalt] varbinary(max) NULL,
        [IdEmpresa] int NOT NULL,
        [TipoUsuario] int NOT NULL,
        [Perfil] nvarchar(max) NULL DEFAULT N'Visitante',
        [sobrenome] nvarchar(50) NOT NULL,
        [emailUsuario] nvarchar(100) NOT NULL,
        [telefone] nvarchar(11) NOT NULL,
        CONSTRAINT [PK_Usuarios] PRIMARY KEY ([IdUsuario])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260513003832_Init'
)
BEGIN
    CREATE TABLE [Emprestimos] (
        [IdSimEmprestimo] int NOT NULL IDENTITY,
        [IdUsuario] int NOT NULL,
        [nomeEmprestimo] nvarchar(50) NOT NULL,
        [descricaoEmprestimo] nvarchar(200) NULL,
        [provedorEmprestimo] nvarchar(max) NULL,
        [valorEmprestimo] decimal(18,2) NOT NULL,
        [parcelasEmprestimo] int NOT NULL,
        [valorParcelas] decimal(18,2) NOT NULL,
        [IOFemprestimo] decimal(5,4) NOT NULL,
        [despesasEmprestimo] decimal(18,2) NOT NULL,
        [tarifasEmprestimo] decimal(18,2) NOT NULL,
        [Data] datetime2 NOT NULL,
        [DataCriacaoSE] datetime2 NOT NULL,
        [UsuarioResponsavel] int NULL,
        CONSTRAINT [PK_Emprestimos] PRIMARY KEY ([IdSimEmprestimo]),
        CONSTRAINT [FK_Emprestimos_Usuarios_IdUsuario] FOREIGN KEY ([IdUsuario]) REFERENCES [Usuarios] ([IdUsuario]) ON DELETE NO ACTION
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260513003832_Init'
)
BEGIN
    CREATE TABLE [Projecoes] (
        [IdProjecao] int NOT NULL IDENTITY,
        [IdUsuario] int NOT NULL,
        [Titulo] nvarchar(200) NOT NULL,
        [ValorPrevisto] decimal(18,2) NOT NULL,
        [DataReferencia] datetime2 NOT NULL,
        [DataCriacao] datetime2 NOT NULL,
        [UsuarioResponsavel] int NULL,
        CONSTRAINT [PK_Projecoes] PRIMARY KEY ([IdProjecao]),
        CONSTRAINT [FK_Projecoes_Usuarios_IdUsuario] FOREIGN KEY ([IdUsuario]) REFERENCES [Usuarios] ([IdUsuario]) ON DELETE NO ACTION
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260513003832_Init'
)
BEGIN
    CREATE TABLE [Recorrencias] (
        [IdRecorrencia] int NOT NULL IDENTITY,
        [IdUsuario] int NOT NULL,
        [IdProjecao] int NULL,
        [TipoLancamento] int NULL,
        [TipoRecorrenciaId] int NULL,
        [dsRecorrencia] nvarchar(50) NULL,
        [obRecorrencia] nvarchar(200) NULL,
        [dataInicio] datetime2 NOT NULL,
        [qtdeRecorrencia] int NULL,
        [Valor] decimal(18,2) NOT NULL,
        [momentoCriacao] datetime2 NOT NULL,
        [UsuarioResponsavel] int NULL,
        CONSTRAINT [PK_Recorrencias] PRIMARY KEY ([IdRecorrencia]),
        CONSTRAINT [FK_Recorrencias_TipoRecorrencias_TipoRecorrenciaId] FOREIGN KEY ([TipoRecorrenciaId]) REFERENCES [TipoRecorrencias] ([Id]),
        CONSTRAINT [FK_Recorrencias_Usuarios_IdUsuario] FOREIGN KEY ([IdUsuario]) REFERENCES [Usuarios] ([IdUsuario]) ON DELETE NO ACTION
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260513003832_Init'
)
BEGIN
    CREATE TABLE [Lancamentos] (
        [IdLancamento] int NOT NULL IDENTITY,
        [IdUsuario] int NOT NULL,
        [IdProjecao] int NULL,
        [IdSimEmprestimo] int NULL,
        [IdRecorrencia] int NULL,
        [TipoLancamento] int NULL,
        [OrigemLancamento] int NULL,
        [FrequenciaRecorrencia] int NULL,
        [ModoRecorrenciaMensal] int NULL,
        [StatusTransacao] int NULL,
        [QtdeRecorrencia] int NULL,
        [DescricaoLancamento] nvarchar(50) NOT NULL,
        [ObservacaoLancamento] nvarchar(200) NULL,
        [Valor] decimal(18,2) NOT NULL,
        [Data] datetime2 NOT NULL,
        [DataCriacao] datetime2 NOT NULL,
        [NumeroDaOcorrencia] int NULL,
        [TotalOcorrencia] int NULL,
        CONSTRAINT [PK_Lancamentos] PRIMARY KEY ([IdLancamento]),
        CONSTRAINT [FK_Lancamentos_Recorrencias_IdRecorrencia] FOREIGN KEY ([IdRecorrencia]) REFERENCES [Recorrencias] ([IdRecorrencia]),
        CONSTRAINT [FK_Lancamentos_Usuarios_IdUsuario] FOREIGN KEY ([IdUsuario]) REFERENCES [Usuarios] ([IdUsuario]) ON DELETE NO ACTION
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260513003832_Init'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'IdEmpresa', N'cnpjEmpresa', N'emailEmpresa', N'idResponsavel', N'nomeFantasia', N'razaoSocial', N'telefoneEmp') AND [object_id] = OBJECT_ID(N'[Empresas]'))
        SET IDENTITY_INSERT [Empresas] ON;
    EXEC(N'INSERT INTO [Empresas] ([IdEmpresa], [cnpjEmpresa], [emailEmpresa], [idResponsavel], [nomeFantasia], [razaoSocial], [telefoneEmp])
    VALUES (1, N''12345678901234'', N''empresa@teste.com'', 1, N''Teste'', N''Empresa Teste'', N''123456789'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'IdEmpresa', N'cnpjEmpresa', N'emailEmpresa', N'idResponsavel', N'nomeFantasia', N'razaoSocial', N'telefoneEmp') AND [object_id] = OBJECT_ID(N'[Empresas]'))
        SET IDENTITY_INSERT [Empresas] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260513003832_Init'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'IdResponsavel', N'emailResponsavel', N'nomeResponsavel', N'sobrenomeResponsavel', N'telefoneResponsavel') AND [object_id] = OBJECT_ID(N'[Responsaveis]'))
        SET IDENTITY_INSERT [Responsaveis] ON;
    EXEC(N'INSERT INTO [Responsaveis] ([IdResponsavel], [emailResponsavel], [nomeResponsavel], [sobrenomeResponsavel], [telefoneResponsavel])
    VALUES (1, N''joao.silva@teste.com'', N''João'', N''Silva'', N''987654321'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'IdResponsavel', N'emailResponsavel', N'nomeResponsavel', N'sobrenomeResponsavel', N'telefoneResponsavel') AND [object_id] = OBJECT_ID(N'[Responsaveis]'))
        SET IDENTITY_INSERT [Responsaveis] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260513003832_Init'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nome', N'PadraoSistema') AND [object_id] = OBJECT_ID(N'[TipoRecorrencias]'))
        SET IDENTITY_INSERT [TipoRecorrencias] ON;
    EXEC(N'INSERT INTO [TipoRecorrencias] ([Id], [Nome], [PadraoSistema])
    VALUES (1, N''Mensal'', CAST(1 AS bit)),
    (2, N''Semanal'', CAST(1 AS bit)),
    (3, N''Anual'', CAST(1 AS bit))');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Nome', N'PadraoSistema') AND [object_id] = OBJECT_ID(N'[TipoRecorrencias]'))
        SET IDENTITY_INSERT [TipoRecorrencias] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260513003832_Init'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'IdUsuario', N'IdEmpresa', N'PasswordHash', N'PasswordSalt', N'Perfil', N'TipoUsuario', N'emailUsuario', N'nomeUsuario', N'sobrenome', N'telefone') AND [object_id] = OBJECT_ID(N'[Usuarios]'))
        SET IDENTITY_INSERT [Usuarios] ON;
    EXEC(N'INSERT INTO [Usuarios] ([IdUsuario], [IdEmpresa], [PasswordHash], [PasswordSalt], [Perfil], [TipoUsuario], [emailUsuario], [nomeUsuario], [sobrenome], [telefone])
    VALUES (1, 0, 0x1C1CAFFA319AA96A8FDB1ECAA757CD4CC9B5E643F5326FD5AEFA495D36740BD340D99919794B4C0302A5D8F314DAD1C72A103324C39029921C39ED7AE489F896, 0xF5C07EA309B31645DA567D93A328C7466C20B2EE88E8B523AD857D4BAD17C528EEEA552BE9B6EB334341A1549823B40DD78D75CEE5A2E1F1F49351A383D26705F8351047278A8CB97D9599398FAAEA9241B782B73F25642D445E03E2756A5E839DA264F7F0AF5ECD5D000E375193538BCDFBBC5C7909CA31F604102FC9D0823D, N''Administrador'', 7, N'''', N''Admin'', N'''', N'''')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'IdUsuario', N'IdEmpresa', N'PasswordHash', N'PasswordSalt', N'Perfil', N'TipoUsuario', N'emailUsuario', N'nomeUsuario', N'sobrenome', N'telefone') AND [object_id] = OBJECT_ID(N'[Usuarios]'))
        SET IDENTITY_INSERT [Usuarios] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260513003832_Init'
)
BEGIN
    CREATE INDEX [IX_Emprestimos_IdUsuario] ON [Emprestimos] ([IdUsuario]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260513003832_Init'
)
BEGIN
    CREATE INDEX [IX_Lancamentos_IdRecorrencia] ON [Lancamentos] ([IdRecorrencia]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260513003832_Init'
)
BEGIN
    CREATE INDEX [IX_Lancamentos_IdUsuario] ON [Lancamentos] ([IdUsuario]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260513003832_Init'
)
BEGIN
    CREATE INDEX [IX_Projecoes_IdUsuario] ON [Projecoes] ([IdUsuario]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260513003832_Init'
)
BEGIN
    CREATE INDEX [IX_Recorrencias_IdUsuario] ON [Recorrencias] ([IdUsuario]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260513003832_Init'
)
BEGIN
    CREATE INDEX [IX_Recorrencias_TipoRecorrenciaId] ON [Recorrencias] ([TipoRecorrenciaId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260513003832_Init'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260513003832_Init', N'10.0.5');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260611174712_AddCategoriaGasto'
)
BEGIN
    ALTER TABLE [Projecoes] ADD [CategoriaGasto] int NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260611174712_AddCategoriaGasto'
)
BEGIN
    ALTER TABLE [Lancamentos] ADD [CategoriaGasto] int NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260611174712_AddCategoriaGasto'
)
BEGIN
    ALTER TABLE [Emprestimos] ADD [CategoriaGasto] int NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260611174712_AddCategoriaGasto'
)
BEGIN
    EXEC(N'UPDATE [Usuarios] SET [PasswordHash] = 0x1FB04EF6189E59F06EA2DFBB5E68B00342EE93E704BACA53A5B848A6D871F2D39E264ACE2328FA055E4E902232D6F15EF1A58BC10F23DE7D16F7467E463666BC, [PasswordSalt] = 0x7CAD480F9415DCA727040DE395BA46FD7FF7ED81E66F973CD70E943C18B5FCD4A66FDF4608079B8E65D7FDD43A75C351CF87BA050521748C47F5682894A434E73D3D06FD8F198745A8FF0C86264B6338737E21C5C0058E73BB9026136F3D496182B34FFFE9DB615CCCCA16832D583E45F4290C526AE8EC0C3A3BEF53357A8F64
    WHERE [IdUsuario] = 1;
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260611174712_AddCategoriaGasto'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260611174712_AddCategoriaGasto', N'10.0.5');
END;

COMMIT;
GO

