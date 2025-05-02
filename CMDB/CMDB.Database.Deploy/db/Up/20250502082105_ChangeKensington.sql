BEGIN TRANSACTION;
GO

ALTER TABLE [Kensington] DROP CONSTRAINT [FK_Key_Device];
GO

DROP INDEX [IX_Kensington_DeviceAssetTag] ON [Kensington];
GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Kensington]') AND [c].[name] = N'DeviceAssetTag');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [Kensington] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [Kensington] DROP COLUMN [DeviceAssetTag];
GO

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Kensington]') AND [c].[name] = N'AssetTag');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [Kensington] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [Kensington] ALTER COLUMN [AssetTag] nvarchar(450) NULL;
GO

CREATE UNIQUE INDEX [IX_Kensington_AssetTag] ON [Kensington] ([AssetTag]) WHERE [AssetTag] IS NOT NULL;
GO

ALTER TABLE [Kensington] ADD CONSTRAINT [FK_Key_Device] FOREIGN KEY ([AssetTag]) REFERENCES [asset] ([AssetTag]) ON DELETE SET NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250502082105_ChangeKensington', N'8.0.11');
GO

COMMIT;
GO