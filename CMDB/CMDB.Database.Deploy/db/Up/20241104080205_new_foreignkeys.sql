BEGIN TRANSACTION;
GO

ALTER TABLE [IdenAccount] DROP CONSTRAINT [FK_IdenAccount_Account_AccountId];
GO

ALTER TABLE [IdenAccount] DROP CONSTRAINT [FK_IdenAccount_Identity_IdentityId];
GO

ALTER TABLE [IdenAccount] ADD CONSTRAINT [FK_IdenAccount_Account] FOREIGN KEY ([AccountId]) REFERENCES [Account] ([AccID]) ON DELETE NO ACTION;
GO

ALTER TABLE [IdenAccount] ADD CONSTRAINT [FK_IdenAccount_Identity] FOREIGN KEY ([IdentityId]) REFERENCES [Identity] ([IdenId]) ON DELETE NO ACTION;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20241104080205_new_foreignkeys', N'7.0.20');
GO

COMMIT;
GO