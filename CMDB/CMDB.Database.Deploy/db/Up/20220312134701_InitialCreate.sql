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

CREATE TABLE [Configuration] (
    [Code] varchar(255) NOT NULL,
    [SubCode] varchar(255) NOT NULL,
    [CFN_Date] datetime2(0) NULL,
    [CFN_Number] int NULL,
    [CFN_Tekst] varchar(255) NULL,
    [Description] varchar(255) NULL,
    CONSTRAINT [PK_Configuration] PRIMARY KEY ([Code], [SubCode])
);
GO

CREATE TABLE [Language] (
    [Code] nvarchar(450) NOT NULL,
    [Description] varchar(255) NOT NULL,
    CONSTRAINT [PK_Language] PRIMARY KEY ([Code])
);
GO

CREATE TABLE [Menu] (
    [MenuId] int NOT NULL IDENTITY,
    [Label] varchar(255) NOT NULL,
    [URL] varchar(255) NULL,
    [ParentId] int NULL,
    CONSTRAINT [PK_Menu] PRIMARY KEY ([MenuId]),
    CONSTRAINT [FK_Menu_Menu] FOREIGN KEY ([ParentId]) REFERENCES [Menu] ([MenuId])
);
GO

CREATE TABLE [RAM] (
    [Id] int NOT NULL IDENTITY,
    [Value] int NOT NULL,
    [Display] varchar(255) NOT NULL,
    CONSTRAINT [PK_RAM] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Admin] (
    [Admin_id] int NOT NULL IDENTITY,
    [AccountId] int NOT NULL,
    [Level] int NOT NULL,
    [Password] varchar(255) NOT NULL,
    [DateSet] datetime2(0) NOT NULL,
    [active] int NOT NULL DEFAULT 1,
    [Deactivate_reason] varchar(255) NULL,
    [LastModifiedAdminId] int NULL,
    CONSTRAINT [PK_Admin] PRIMARY KEY ([Admin_id]),
    CONSTRAINT [FK_Admin_LastModiefiedAdmin] FOREIGN KEY ([LastModifiedAdminId]) REFERENCES [Admin] ([Admin_id]) ON DELETE NO ACTION
);
GO

CREATE TABLE [Application] (
    [AppID] int NOT NULL IDENTITY,
    [Name] varchar(255) NOT NULL,
    [active] int NOT NULL DEFAULT 1,
    [Deactivate_reason] varchar(255) NULL,
    [LastModifiedAdminId] int NULL,
    CONSTRAINT [PK_Application] PRIMARY KEY ([AppID]),
    CONSTRAINT [FK_Application_LastModifiedAdmin] FOREIGN KEY ([LastModifiedAdminId]) REFERENCES [Admin] ([Admin_id]) ON DELETE SET NULL
);
GO

CREATE TABLE [category] (
    [Id] int NOT NULL IDENTITY,
    [Category] varchar(255) NOT NULL,
    [Prefix] varchar(5) NULL,
    [active] int NOT NULL DEFAULT 1,
    [Deactivate_reason] varchar(255) NULL,
    [LastModifiedAdminId] int NULL,
    CONSTRAINT [PK_AssetCagegory] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AssetCategory_LastModifiedAdmin] FOREIGN KEY ([LastModifiedAdminId]) REFERENCES [Admin] ([Admin_id]) ON DELETE SET NULL
);
GO

CREATE TABLE [Permission] (
    [Id] int NOT NULL IDENTITY,
    [Rights] varchar(255) NOT NULL,
    [Description] varchar(255) NULL,
    [LastModifiedAdminId] int NULL,
    CONSTRAINT [PK_PPermission] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Permission_CreatedAdmin] FOREIGN KEY ([LastModifiedAdminId]) REFERENCES [Admin] ([Admin_id]) ON DELETE SET NULL
);
GO

CREATE TABLE [Type] (
    [TypeId] int NOT NULL IDENTITY,
    [Type] varchar(255) NOT NULL,
    [Description] varchar(255) NOT NULL,
    [Discriminator] nvarchar(max) NOT NULL,
    [AdminId] int NULL,
    [active] int NOT NULL DEFAULT 1,
    [Deactivate_reason] varchar(255) NULL,
    [LastModifiedAdminId] int NULL,
    CONSTRAINT [PK_Type] PRIMARY KEY ([TypeId]),
    CONSTRAINT [FK_Type_Admin_AdminId] FOREIGN KEY ([AdminId]) REFERENCES [Admin] ([Admin_id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Type_LastModifiedAdmin] FOREIGN KEY ([LastModifiedAdminId]) REFERENCES [Admin] ([Admin_id]) ON DELETE SET NULL
);
GO

CREATE TABLE [AssetType] (
    [TypeID] int NOT NULL IDENTITY,
    [Vendor] varchar(255) NOT NULL,
    [Type] varchar(255) NOT NULL,
    [CategoryId] int NOT NULL,
    [active] int NOT NULL DEFAULT 1,
    [Deactivate_reason] varchar(255) NULL,
    [LastModifiedAdminId] int NULL,
    CONSTRAINT [PK_AssetType] PRIMARY KEY ([TypeID]),
    CONSTRAINT [FK_AssetType_category_CategoryId] FOREIGN KEY ([CategoryId]) REFERENCES [category] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_AssetType_LastModifiedAdmin] FOREIGN KEY ([LastModifiedAdminId]) REFERENCES [Admin] ([Admin_id]) ON DELETE SET NULL
);
GO

CREATE TABLE [SubscriptionType] (
    [Id] int NOT NULL IDENTITY,
    [Type] nvarchar(max) NULL,
    [Description] nvarchar(max) NULL,
    [Provider] nvarchar(max) NULL,
    [AssetCategoryId] int NOT NULL,
    [active] int NOT NULL DEFAULT 1,
    [Deactivate_reason] varchar(255) NULL,
    [LastModifiedAdminId] int NULL,
    CONSTRAINT [PK_SubscriptionType] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AssetType_Category] FOREIGN KEY ([AssetCategoryId]) REFERENCES [category] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_SubscriptionType_LastModifiedAdmin] FOREIGN KEY ([LastModifiedAdminId]) REFERENCES [Admin] ([Admin_id]) ON DELETE SET NULL
);
GO

CREATE TABLE [RolePerm] (
    [Id] int NOT NULL IDENTITY,
    [Level] int NOT NULL,
    [LastModifiedAdminId] int NULL,
    [MenuId] int NOT NULL,
    [PermissionId] int NOT NULL,
    CONSTRAINT [PK_RolePerm] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_RolePerm_LastModifiedAdmin] FOREIGN KEY ([LastModifiedAdminId]) REFERENCES [Admin] ([Admin_id]) ON DELETE SET NULL,
    CONSTRAINT [FK_RolePerm_Menu] FOREIGN KEY ([MenuId]) REFERENCES [Menu] ([MenuId]) ON DELETE NO ACTION,
    CONSTRAINT [FK_RolePerm_Permission] FOREIGN KEY ([PermissionId]) REFERENCES [Permission] ([Id]) ON DELETE NO ACTION
);
GO

CREATE TABLE [Account] (
    [AccID] int NOT NULL IDENTITY,
    [TypeId] int NOT NULL,
    [ApplicationId] int NOT NULL,
    [UserID] varchar(255) NOT NULL,
    [active] int NOT NULL DEFAULT 1,
    [Deactivate_reason] varchar(255) NULL,
    [LastModifiedAdminId] int NULL,
    CONSTRAINT [PK_Account] PRIMARY KEY ([AccID]),
    CONSTRAINT [FK_Account_Application] FOREIGN KEY ([ApplicationId]) REFERENCES [Application] ([AppID]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Account_LastModifiedAdmin] FOREIGN KEY ([LastModifiedAdminId]) REFERENCES [Admin] ([Admin_id]) ON DELETE SET NULL,
    CONSTRAINT [FK_Account_Type] FOREIGN KEY ([TypeId]) REFERENCES [Type] ([TypeId]) ON DELETE NO ACTION
);
GO

CREATE TABLE [Identity] (
    [IdenId] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NOT NULL,
    [EMail] nvarchar(max) NOT NULL,
    [UserID] nvarchar(max) NOT NULL,
    [Company] nvarchar(max) NOT NULL,
    [LanguageCode] nvarchar(450) NOT NULL,
    [TypeId] int NOT NULL,
    [active] int NOT NULL DEFAULT 1,
    [Deactivate_reason] varchar(255) NULL,
    [LastModifiedAdminId] int NULL,
    CONSTRAINT [PK_Identity] PRIMARY KEY ([IdenId]),
    CONSTRAINT [FK_Identity_Language] FOREIGN KEY ([LanguageCode]) REFERENCES [Language] ([Code]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Identity_LastModifiedAdmin] FOREIGN KEY ([LastModifiedAdminId]) REFERENCES [Admin] ([Admin_id]) ON DELETE SET NULL,
    CONSTRAINT [FK_Identity_Type] FOREIGN KEY ([TypeId]) REFERENCES [Type] ([TypeId]) ON DELETE NO ACTION
);
GO

CREATE TABLE [Role] (
    [RoleId] int NOT NULL IDENTITY,
    [Name] varchar(255) NOT NULL,
    [Description] varchar(255) NOT NULL,
    [TypeId] int NOT NULL,
    [active] int NOT NULL DEFAULT 1,
    [Deactivate_reason] varchar(255) NULL,
    [LastModifiedAdminId] int NULL,
    CONSTRAINT [PK_Role] PRIMARY KEY ([RoleId]),
    CONSTRAINT [FK_Role_LastModifiedAdmin] FOREIGN KEY ([LastModifiedAdminId]) REFERENCES [Admin] ([Admin_id]) ON DELETE SET NULL,
    CONSTRAINT [FK_Role_Type] FOREIGN KEY ([TypeId]) REFERENCES [Type] ([TypeId]) ON DELETE NO ACTION
);
GO

CREATE TABLE [asset] (
    [AssetTag] nvarchar(450) NOT NULL,
    [SerialNumber] varchar(255) NOT NULL,
    [TypeId] int NOT NULL,
    [CategoryId] int NOT NULL,
    [IdentityId] int NULL,
    [Discriminator] nvarchar(max) NOT NULL,
    [MAC] varchar(255) NULL,
    [RAM] varchar(255) NULL,
    [active] int NOT NULL DEFAULT 1,
    [Deactivate_reason] varchar(255) NULL,
    [LastModifiedAdminId] int NULL,
    CONSTRAINT [PK_Device_AssetTag] PRIMARY KEY ([AssetTag]),
    CONSTRAINT [FK_Device_Category] FOREIGN KEY ([CategoryId]) REFERENCES [category] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Device_Identity] FOREIGN KEY ([IdentityId]) REFERENCES [Identity] ([IdenId]) ON DELETE SET NULL,
    CONSTRAINT [FK_Device_LastModfiedAdmin] FOREIGN KEY ([LastModifiedAdminId]) REFERENCES [Admin] ([Admin_id]) ON DELETE SET NULL,
    CONSTRAINT [FK_Device_Type] FOREIGN KEY ([TypeId]) REFERENCES [AssetType] ([TypeID]) ON DELETE NO ACTION
);
GO

CREATE TABLE [IdenAccount] (
    [ID] int NOT NULL IDENTITY,
    [ValidFrom] datetime2(0) NOT NULL,
    [ValidUntil] datetime2(0) NOT NULL,
    [IdentityId] int NULL,
    [AccountId] int NOT NULL,
    [LastModifiedAdminId] int NULL,
    CONSTRAINT [PK_IdenAccount] PRIMARY KEY ([ID]),
    CONSTRAINT [FK_IdenAccount_Account_AccountId] FOREIGN KEY ([AccountId]) REFERENCES [Account] ([AccID]) ON DELETE NO ACTION,
    CONSTRAINT [FK_IdenAccount_Identity_IdentityId] FOREIGN KEY ([IdentityId]) REFERENCES [Identity] ([IdenId]) ON DELETE NO ACTION,
    CONSTRAINT [FK_IdenAccount_LastModifiedAdmin] FOREIGN KEY ([LastModifiedAdminId]) REFERENCES [Admin] ([Admin_id]) ON DELETE SET NULL
);
GO

CREATE TABLE [Mobile] (
    [IMEI] int NOT NULL IDENTITY,
    [TypeId] int NULL,
    [IdentityId] int NULL,
    [CategoryId] int NOT NULL,
    [active] int NOT NULL DEFAULT 1,
    [Deactivate_reason] varchar(255) NULL,
    [LastModifiedAdminId] int NULL,
    CONSTRAINT [PK_Mobile] PRIMARY KEY ([IMEI]),
    CONSTRAINT [FK_Mobile_Category] FOREIGN KEY ([CategoryId]) REFERENCES [category] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Mobile_Identity] FOREIGN KEY ([IdentityId]) REFERENCES [Identity] ([IdenId]) ON DELETE SET NULL,
    CONSTRAINT [FK_Mobile_LastModifiedAdmin] FOREIGN KEY ([LastModifiedAdminId]) REFERENCES [Admin] ([Admin_id]) ON DELETE SET NULL,
    CONSTRAINT [FK_Mobile_Type] FOREIGN KEY ([TypeId]) REFERENCES [AssetType] ([TypeID]) ON DELETE NO ACTION
);
GO

CREATE TABLE [Kensington] (
    [KeyID] int NOT NULL IDENTITY,
    [SerialNumber] varchar(255) NOT NULL,
    [DeviceAssetTag] nvarchar(450) NULL,
    [AmountOfKeys] int NOT NULL,
    [HasLock] bit NOT NULL,
    [TypeId] int NOT NULL,
    [CategoryId] int NOT NULL,
    [active] int NOT NULL DEFAULT 1,
    [Deactivate_reason] varchar(255) NULL,
    [LastModifiedAdminId] int NULL,
    CONSTRAINT [PK_Kensington] PRIMARY KEY ([KeyID]),
    CONSTRAINT [FK_Kensington_Category] FOREIGN KEY ([CategoryId]) REFERENCES [category] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Kensington_LastModifiedAdmin] FOREIGN KEY ([LastModifiedAdminId]) REFERENCES [Admin] ([Admin_id]) ON DELETE SET NULL,
    CONSTRAINT [FK_Kensington_Type] FOREIGN KEY ([TypeId]) REFERENCES [AssetType] ([TypeID]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Key_Device] FOREIGN KEY ([DeviceAssetTag]) REFERENCES [asset] ([AssetTag]) ON DELETE SET NULL
);
GO

CREATE TABLE [Subscription] (
    [Id] int NOT NULL IDENTITY,
    [SubsctiptionTypeId] int NOT NULL,
    [PhoneNumber] nvarchar(max) NULL,
    [IdentityId] int NULL,
    [MobileId] int NULL,
    [AssetCategoryId] int NOT NULL,
    [active] int NOT NULL DEFAULT 1,
    [Deactivate_reason] varchar(255) NULL,
    [LastModifiedAdminId] int NULL,
    CONSTRAINT [PK_Subscription] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Subscription_Category] FOREIGN KEY ([AssetCategoryId]) REFERENCES [category] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Subscription_Identity] FOREIGN KEY ([IdentityId]) REFERENCES [Identity] ([IdenId]) ON DELETE SET NULL,
    CONSTRAINT [FK_Subscription_LastModifiedAdmin] FOREIGN KEY ([LastModifiedAdminId]) REFERENCES [Admin] ([Admin_id]) ON DELETE SET NULL,
    CONSTRAINT [FK_Subscription_Mobile] FOREIGN KEY ([MobileId]) REFERENCES [Mobile] ([IMEI]) ON DELETE SET NULL,
    CONSTRAINT [FK_Subscription_Type] FOREIGN KEY ([SubsctiptionTypeId]) REFERENCES [SubscriptionType] ([Id]) ON DELETE NO ACTION
);
GO

CREATE TABLE [Log] (
    [Id] int NOT NULL IDENTITY,
    [LogDate] datetime2 NOT NULL,
    [LogText] nvarchar(max) NOT NULL,
    [AccountId] int NULL,
    [TypeId] int NULL,
    [AdminId] int NULL,
    [ApplicationId] int NULL,
    [AssetCategoryId] int NULL,
    [AssetTypeId] int NULL,
    [AssetTag] nvarchar(450) NULL,
    [IdentityId] int NULL,
    [KensingtonId] int NULL,
    [MenuId] int NULL,
    [MobileId] int NULL,
    [PermissionId] int NULL,
    [SubsriptionId] int NULL,
    [SubscriptionTypeId] int NULL,
    [RoleId] int NULL,
    CONSTRAINT [PK_Log] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Device_Asset] FOREIGN KEY ([AssetTag]) REFERENCES [asset] ([AssetTag]) ON DELETE SET NULL,
    CONSTRAINT [FK_Log_Account] FOREIGN KEY ([AccountId]) REFERENCES [Account] ([AccID]) ON DELETE SET NULL,
    CONSTRAINT [FK_Log_Admin] FOREIGN KEY ([AdminId]) REFERENCES [Admin] ([Admin_id]) ON DELETE SET NULL,
    CONSTRAINT [FK_Log_Application] FOREIGN KEY ([ApplicationId]) REFERENCES [Application] ([AppID]) ON DELETE SET NULL,
    CONSTRAINT [FK_Log_AssetType] FOREIGN KEY ([AssetTypeId]) REFERENCES [AssetType] ([TypeID]) ON DELETE SET NULL,
    CONSTRAINT [FK_Log_Category] FOREIGN KEY ([AssetCategoryId]) REFERENCES [category] ([Id]) ON DELETE SET NULL,
    CONSTRAINT [FK_Log_Identity] FOREIGN KEY ([IdentityId]) REFERENCES [Identity] ([IdenId]) ON DELETE SET NULL,
    CONSTRAINT [FK_Log_KensingTone] FOREIGN KEY ([KensingtonId]) REFERENCES [Kensington] ([KeyID]) ON DELETE SET NULL,
    CONSTRAINT [FK_Log_Menu] FOREIGN KEY ([MenuId]) REFERENCES [Menu] ([MenuId]) ON DELETE SET NULL,
    CONSTRAINT [FK_Log_Mobile] FOREIGN KEY ([MobileId]) REFERENCES [Mobile] ([IMEI]) ON DELETE SET NULL,
    CONSTRAINT [FK_Log_Permission] FOREIGN KEY ([PermissionId]) REFERENCES [Permission] ([Id]) ON DELETE SET NULL,
    CONSTRAINT [FK_Log_Role] FOREIGN KEY ([RoleId]) REFERENCES [Role] ([RoleId]) ON DELETE SET NULL,
    CONSTRAINT [FK_Log_Subscription] FOREIGN KEY ([SubsriptionId]) REFERENCES [Subscription] ([Id]) ON DELETE SET NULL,
    CONSTRAINT [FK_Log_SubscriptionType] FOREIGN KEY ([SubscriptionTypeId]) REFERENCES [SubscriptionType] ([Id]) ON DELETE SET NULL,
    CONSTRAINT [FK_Log_Type] FOREIGN KEY ([TypeId]) REFERENCES [Type] ([TypeId]) ON DELETE SET NULL
);
GO

CREATE INDEX [IX_Account_ApplicationId] ON [Account] ([ApplicationId]);
GO

CREATE INDEX [IX_Account_LastModifiedAdminId] ON [Account] ([LastModifiedAdminId]);
GO

CREATE INDEX [IX_Account_TypeId] ON [Account] ([TypeId]);
GO

CREATE INDEX [IX_Admin_AccountId] ON [Admin] ([AccountId]);
GO

CREATE INDEX [IX_Admin_LastModifiedAdminId] ON [Admin] ([LastModifiedAdminId]);
GO

CREATE INDEX [IX_Application_LastModifiedAdminId] ON [Application] ([LastModifiedAdminId]);
GO

CREATE INDEX [IX_asset_CategoryId] ON [asset] ([CategoryId]);
GO

CREATE INDEX [IX_asset_IdentityId] ON [asset] ([IdentityId]);
GO

CREATE INDEX [IX_asset_LastModifiedAdminId] ON [asset] ([LastModifiedAdminId]);
GO

CREATE INDEX [IX_asset_TypeId] ON [asset] ([TypeId]);
GO

CREATE INDEX [IX_AssetType_CategoryId] ON [AssetType] ([CategoryId]);
GO

CREATE INDEX [IX_AssetType_LastModifiedAdminId] ON [AssetType] ([LastModifiedAdminId]);
GO

CREATE INDEX [IX_category_LastModifiedAdminId] ON [category] ([LastModifiedAdminId]);
GO

CREATE UNIQUE INDEX [IX_IdenAccount_AccountId_IdentityId_ValidFrom_ValidUntil] ON [IdenAccount] ([AccountId], [IdentityId], [ValidFrom], [ValidUntil]) WHERE [IdentityId] IS NOT NULL;
GO

CREATE INDEX [IX_IdenAccount_IdentityId] ON [IdenAccount] ([IdentityId]);
GO

CREATE INDEX [IX_IdenAccount_LastModifiedAdminId] ON [IdenAccount] ([LastModifiedAdminId]);
GO

CREATE INDEX [IX_Identity_LanguageCode] ON [Identity] ([LanguageCode]);
GO

CREATE INDEX [IX_Identity_LastModifiedAdminId] ON [Identity] ([LastModifiedAdminId]);
GO

CREATE INDEX [IX_Identity_TypeId] ON [Identity] ([TypeId]);
GO

CREATE INDEX [IX_Kensington_CategoryId] ON [Kensington] ([CategoryId]);
GO

CREATE INDEX [IX_Kensington_DeviceAssetTag] ON [Kensington] ([DeviceAssetTag]);
GO

CREATE INDEX [IX_Kensington_LastModifiedAdminId] ON [Kensington] ([LastModifiedAdminId]);
GO

CREATE INDEX [IX_Kensington_TypeId] ON [Kensington] ([TypeId]);
GO

CREATE INDEX [IX_Log_AccountId] ON [Log] ([AccountId]);
GO

CREATE INDEX [IX_Log_AdminId] ON [Log] ([AdminId]);
GO

CREATE INDEX [IX_Log_ApplicationId] ON [Log] ([ApplicationId]);
GO

CREATE INDEX [IX_Log_AssetCategoryId] ON [Log] ([AssetCategoryId]);
GO

CREATE INDEX [IX_Log_AssetTag] ON [Log] ([AssetTag]);
GO

CREATE INDEX [IX_Log_AssetTypeId] ON [Log] ([AssetTypeId]);
GO

CREATE INDEX [IX_Log_IdentityId] ON [Log] ([IdentityId]);
GO

CREATE INDEX [IX_Log_KensingtonId] ON [Log] ([KensingtonId]);
GO

CREATE INDEX [IX_Log_MenuId] ON [Log] ([MenuId]);
GO

CREATE INDEX [IX_Log_MobileId] ON [Log] ([MobileId]);
GO

CREATE INDEX [IX_Log_PermissionId] ON [Log] ([PermissionId]);
GO

CREATE INDEX [IX_Log_RoleId] ON [Log] ([RoleId]);
GO

CREATE INDEX [IX_Log_SubscriptionTypeId] ON [Log] ([SubscriptionTypeId]);
GO

CREATE INDEX [IX_Log_SubsriptionId] ON [Log] ([SubsriptionId]);
GO

CREATE INDEX [IX_Log_TypeId] ON [Log] ([TypeId]);
GO

CREATE INDEX [IX_Menu_ParentId] ON [Menu] ([ParentId]);
GO

CREATE INDEX [IX_Mobile_CategoryId] ON [Mobile] ([CategoryId]);
GO

CREATE INDEX [IX_Mobile_IdentityId] ON [Mobile] ([IdentityId]);
GO

CREATE INDEX [IX_Mobile_LastModifiedAdminId] ON [Mobile] ([LastModifiedAdminId]);
GO

CREATE INDEX [IX_Mobile_TypeId] ON [Mobile] ([TypeId]);
GO

CREATE INDEX [IX_Permission_LastModifiedAdminId] ON [Permission] ([LastModifiedAdminId]);
GO

CREATE INDEX [IX_Role_LastModifiedAdminId] ON [Role] ([LastModifiedAdminId]);
GO

CREATE INDEX [IX_Role_TypeId] ON [Role] ([TypeId]);
GO

CREATE INDEX [IX_RolePerm_LastModifiedAdminId] ON [RolePerm] ([LastModifiedAdminId]);
GO

CREATE INDEX [IX_RolePerm_MenuId] ON [RolePerm] ([MenuId]);
GO

CREATE INDEX [IX_RolePerm_PermissionId] ON [RolePerm] ([PermissionId]);
GO

CREATE INDEX [IX_Subscription_AssetCategoryId] ON [Subscription] ([AssetCategoryId]);
GO

CREATE INDEX [IX_Subscription_IdentityId] ON [Subscription] ([IdentityId]);
GO

CREATE INDEX [IX_Subscription_LastModifiedAdminId] ON [Subscription] ([LastModifiedAdminId]);
GO

CREATE INDEX [IX_Subscription_MobileId] ON [Subscription] ([MobileId]);
GO

CREATE INDEX [IX_Subscription_SubsctiptionTypeId] ON [Subscription] ([SubsctiptionTypeId]);
GO

CREATE INDEX [IX_SubscriptionType_AssetCategoryId] ON [SubscriptionType] ([AssetCategoryId]);
GO

CREATE INDEX [IX_SubscriptionType_LastModifiedAdminId] ON [SubscriptionType] ([LastModifiedAdminId]);
GO

CREATE INDEX [IX_Type_AdminId] ON [Type] ([AdminId]);
GO

CREATE INDEX [IX_Type_LastModifiedAdminId] ON [Type] ([LastModifiedAdminId]);
GO

ALTER TABLE [Admin] ADD CONSTRAINT [FK_Admin_Account] FOREIGN KEY ([AccountId]) REFERENCES [Account] ([AccID]) ON DELETE NO ACTION;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20220324160525_InitialCreate', N'5.0.10');
GO

COMMIT;
GO