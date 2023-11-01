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
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231101122806_MigF')
BEGIN
    CREATE TABLE [Players] (
        [PlayerID] int NOT NULL IDENTITY,
        [Club] nvarchar(max) NOT NULL,
        [Country] nvarchar(max) NOT NULL,
        [Name] nvarchar(max) NOT NULL,
        [Age] int NOT NULL,
        [Price] float NOT NULL,
        CONSTRAINT [PK_Players] PRIMARY KEY ([PlayerID])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231101122806_MigF')
BEGIN
    CREATE TABLE [Users] (
        [UID] int NOT NULL IDENTITY,
        [Username] nvarchar(max) NOT NULL,
        [Email] nvarchar(max) NOT NULL,
        [Club] nvarchar(max) NOT NULL,
        [Password] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_Users] PRIMARY KEY ([UID])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231101122806_MigF')
BEGIN
    CREATE TABLE [Wishlists] (
        [UserID] int NOT NULL,
        [PlayerID] int NOT NULL,
        CONSTRAINT [PK_Wishlists] PRIMARY KEY ([UserID], [PlayerID])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231101122806_MigF')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20231101122806_MigF', N'7.0.13');
END;
GO

COMMIT;
GO

