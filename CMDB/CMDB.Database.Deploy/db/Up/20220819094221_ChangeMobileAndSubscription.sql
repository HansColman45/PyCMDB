BEGIN TRANSACTION;
GO

EXEC sp_rename N'[Subscription].[Id]', N'SubscriptionId', N'COLUMN';
GO

EXEC sp_rename N'[Mobile].[Id]', N'MobileId', N'COLUMN';
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20220819094221_ChangeMobileAndSubscription', N'6.0.3');
GO

COMMIT;
GO