BEGIN TRANSACTION;
GO

DROP INDEX [IX_Mobile_IMEI] ON [Mobile];

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Mobile]') AND [c].[name] = N'IMEI');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [Mobile] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [Mobile] ALTER COLUMN [IMEI] bigint NOT NULL;
CREATE UNIQUE INDEX [IX_Mobile_IMEI] ON [Mobile] ([IMEI]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20220626082821_UpdateMobile', N'6.0.3');
GO

COMMIT;
GO