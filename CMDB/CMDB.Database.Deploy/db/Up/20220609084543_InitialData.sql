-- Language
insert into Language (CODE, Description) values
('NL', 'Dutch'),
('FR', 'French'),
('EN', 'English');
-- Application
insert into application (Name) values
('Active Directory'),
('CMDB');
-- AccountType
INSERT INTO type (Discriminator,Type, Description) Values
('AccountType','Normal User','Normal User no aditional wrights'),
('AccountType','Administrator','Admnistrator Account'),
('AccountType','Servive User','User used to run services');
-- IdentityType
INSERT INTO type (Discriminator,Type,Description) values
('IdentityType','Werknemer','Werknemer'),
('IdentityType','Exeterne','Externe medewerker');
-- roletype
INSERT INTO type (Discriminator,Type,Description) values
('RoleType','Application','The Application Role'),
('RoleType','System','The System Role');
-- AssetCat
INSERT INTO category (Category,prefix) VALUES 
('Kensington',NULL),
('Mobile',NULL),
('Mobile Subscription',NULL),
('Internet Subscription',NULL),
('Laptop','LPT'),
('Desktop','DST'),
('Token',NULL),
('Monitor','SCR'),
('Docking station','DOC');
-- ram
INSERT INTO ram (Value, Display) VALUES
(128, '128 Kb'),
(256, '256 Kb'),
(512, '512 Kb'),
(1024, '1 Gb'),
(1536, '1,5 Gb'),
(2048, '2 Gb'),
(2560, '2,5 Gb'),
(3072, '3 Gb'),
(3584, '3,5 Gb'),
(4096, '4 Gb'),
(5120, '5 Gb');
-- configuration
INSERT INTO configuration (CODE,SubCode,CFN_Tekst,Description) values
('General','DateFormat','dd/MM/yyyy','This is the data format used in the application'),
('General','LogDateFormat','dd/MM/yyyy HH:mm:ss','This is the data format used in the application'),
('General','Company','Brightest','This is the company for who the Website is build for');
-- Account
INSERT INTO account (UserID,TypeId,ApplicationId) VALUES ('Root',(select TypeId from [Type] where Discriminator = 'AccountType' and Type='Administrator'),2);
--Identity
Insert into [Identity] (Name,LanguageCode,EMail,Company,UserID,TypeId) Values('Stock','NL','root@cmdb.com','CMDB','ROOT',(select TypeId from [Type] where Discriminator = 'IdentityType' and Type='Werknemer'));
--role
insert into role (Name, Description, TypeId) VALUES ('Administrator','The administrator of the Application',(select TypeId from [Type] where Discriminator = 'RoleType' and Type='Application'));
-- idenaccount
insert into IdenAccount (IdentityId,AccountId,ValidFrom,ValidUntil) values
(1,1,'2012-01-01 00:00:00','9999-12-31 23:59:00');
-- Admin
INSERT INTO Admin (AccountId,Level,PassWord,DateSet) VALUES
(1,9,'109799de5567dae0b0f17deef5516a8b','2012-01-01 00:00:00');
-- SubscriptionType
insert into SubscriptionType (Type,Description, Provider, AssetCategoryId) values
('Expresnet','Telenet Expresnet','Telenet',4),
('Fibernet','Telenet Fibernet','Telenet',4),
('ADSL','ADSL','Belgacom',4),
('VDSL','VDSL','Belgacom',4),
('Split','Company pays back wath the user indicates','Proximus',3),
('EUR 25','Company pays EUR 25 back per month','Proximus',3),
('FULL','Company pays everyting back','Proximus',3);
-- Menu
insert into menu (label) VALUES ('Identity'); --1
insert into menu (label,ParentId) VALUES ('Identity',1); --2
insert into menu (label,URL,ParentId) VALUES ('Overview','Identity',2); --3
insert into menu (label) VALUES ('Account'); --4
insert into menu (label,ParentId) VALUES ('Account',4);--5
insert into menu (label,URL,ParentId) VALUES ('Overview','Account',5);--6
insert into menu (label) VALUES ('Role');--7
insert into menu (label,ParentId) VALUES ('Role',7);--8
insert into menu (label,URL,ParentId) VALUES ('Overview','Role',8);--9
-- menu devices
INSERT INTO menu (label, URL, ParentId) VALUES
('Devices', '#', null),--10
('Laptop', '#', 10),--11
('Overview', 'Laptop', 11),--12
('Desktop', '#', 10),--13
('Overview', 'Desktop', 13),--14
('Monitor', '#', 10),--15
('Overview', 'Monitor', 15),--16
('Docking station','#',10),--17
('Overview','Docking',17),--18
('Token', '#', 10),--19
('Overview', 'Token', 19),--20
('Kensington', '#', 10),--21
('Overview', 'Kensington', 21),--22
('Mobile', '#', 10),--223
('Overview', 'Mobile', 23),--24
('Subscription', '#', 10),--25
('Overview', 'Subscription', 25);--26
-- menu Types
INSERT INTO menu (label, URL, ParentId) VALUES
('Types', '#', null),--27
('Asset Type', '#',27),--28
('Overview', 'AssetType',28),--29
('Asset Category', '#',27),--30
('Overview', 'AssetCategory',30),--31
('Identity Type', '#',27),--32
('Overview', 'IdentityType',32),--33
('Account Type', '#',27),--34
('Overview', 'AccountType',34),--35
('Role Type', '#',27),--36
('Overview', 'RoleType',36),--37
('Subscription Type', '#', 27),--38
('Overview', 'SubscriptionType', 38);--39
-- menu others
INSERT INTO menu (label, URL, ParentId) VALUES
('System','#',null), --40
('System','#',40),--41
('Overview','Sytem',41),--42
('Application','#',NULL),--43
('Application','#',43),--44
('Overview','Application',44),--45
('Admin','#',null),--46
('Admin','#',46),--47
('Overview','Admin',47),--48
('Permissions','#',46),--49
('Overview','Permission',49),--50
('Role permission','#',46),--51
('Overview','RolePermission',51);--52
-- permisions
INSERT INTO Permission (Rights,description) VALUES
('Read',NULL),
('Update',NULL),
('Delete',NULL),
('Activate',NULL),
('Add',NULL),
('AssignAccount','This Permission is for assigning Accounts'),
('AccountOverview','This Permission is to see The assigned Accounts'),
('AssignDevice','This Permission is to Assign a Device'),
('DeviceOverview','This Permission is see the assigned Device'),
('AssignMobile','This Permission is to Assign a Mobile'),
('MobileOverview','This Permission is to see the assigned Mobiles'),
('AssignSubscription','This Permission is to Assign a Subscription'),
('SubscriptionOverview','This Permission is to see the assigned Subscription'),
('AssignIdentity','This Permission is to Assign a Identity'),
('IdentityOverview','This Permission is see the assigned Identities'),
('AssignAppRole','This Permission is to Assign a Application Role'),
('AppRoleOverview','This Permission is to  see the assigned Application Roles'),
('AssignSysRole','This Permission is to Assign a System Role'),
('SysRoleOverview','This Permission is to see the assigned System Roles'),
('AssignApplication','This Permission is to Assign an Application'),
('ApplicationOverview','This Permission is to see the assigned Application'),
('AssignSystem','This Permission is to Assign a System'),
('SystemOverview','This Permission is to see the assigned Systems'),
('AssignKensington','This Permission is to Assign a Kensington'),
('KeyOverview','This Permission is to see the assigned Kensington'),
('ReleaseDevice','This Permission is to release Device'),
('ReleaseAccount','This Permission is to release Account'),
('ReleaseMobile','This Permission is to release Mobile'),
('ReleaseSubscription','This Permission is to release Subscription'),
('ReleaseIdentity','This Permission is to release Identity'),
('ReleaseAppRole','This Permission is to release Application Role'),
('ReleaseSysRole','This Permission is to release System Role'),
('ReleaseApplication','This Permission is to release Application'),
('ReleaseSystem','This Permission is to release System'),
('ReleaseKensington','This Permission is to release Kensington'),
('AssignLevel','This Permission is to Assign a Level'),
('PermissionOverview','This Permission is to see the Permissions'),
('MenuOverview','This Permission is to see the Menu'),
('RorePermOverview','This Permission is to see the RolePermission overview');
-- role permision
-- Identity =2
INSERT INTO roleperm (level, PermissionId, menuId, LastModifiedAdminId) VALUES
(9, 1, 2, 1),
(9, 2, 2, 1),
(9, 3, 2, 1),
(9, 4, 2, 1),
(9, 5, 2, 1),
(9, 6, 2, 1),
(9, 7, 2, 1),
(9, 8, 2, 1),
(9, 9, 2, 1),
(9, 10, 2, 1),
(9, 11, 2, 1),
(9, 12, 2, 1),
(9, 13, 2, 1),
(9, 26, 2, 1),
(9, 27, 2, 1);
-- IdentityType =30
INSERT INTO roleperm (level, PermissionId, menuId, LastModifiedAdminId) VALUES
(9, 1, 30, 1),
(9, 2, 30, 1),
(9, 3, 30, 1),
(9, 4, 30, 1),
(9, 5, 30, 1);
-- Account =5
INSERT INTO roleperm (level, PermissionId, menuId, LastModifiedAdminId) VALUES
(9, 1, 5, 1),
(9, 2, 5, 1),
(9, 3, 5, 1),
(9, 4, 5, 1),
(9, 5, 5, 1),
(9, 14, 5, 1),
(9, 15, 5, 1),
(9, 30, 5, 1);
-- Role = 8
INSERT INTO roleperm (level, PermissionId, menuId, LastModifiedAdminId) VALUES
(9, 1, 8, 1),
(9, 2, 8, 1),
(9, 3, 8, 1),
(9, 4, 8, 1),
(9, 5, 8, 1),
(9, 20, 8, 1),
(9, 21, 8, 1),
(9, 33, 8, 1),
(9, 34, 8, 1);
-- Laptop = 11
INSERT INTO roleperm (level, PermissionId, menuId, LastModifiedAdminId) VALUES
(9, 1, 11, 1),
(9, 2, 11, 1),
(9, 3, 11, 1),
(9, 4, 11, 1),
(9, 5, 11, 1),
(9, 14, 11, 1),
(9, 15, 11, 1),
(9, 24, 11, 1),
(9, 25, 11, 1),
(9, 30, 11, 1),
(9, 35, 11, 1);
-- Desktop = 13
INSERT INTO roleperm (level, PermissionId, menuId, LastModifiedAdminId) VALUES
(9, 1, 13, 1),
(9, 2, 13, 1),
(9, 3, 13, 1),
(9, 4, 13, 1),
(9, 5, 13, 1),
(9, 14, 13, 1),
(9, 15, 13, 1),
(9, 24, 13, 1),
(9, 25, 13, 1),
(9, 30, 13, 1),
(9, 35, 13, 1);
-- Monitor = 15
INSERT INTO roleperm (level, PermissionId, menuId, LastModifiedAdminId) VALUES
(9, 1, 15, 1),
(9, 2, 15, 1),
(9, 3, 15, 1),
(9, 4, 15, 1),
(9, 5, 15, 1),
(9, 14, 15, 1),
(9, 15, 15, 1),
(9, 24, 15, 1),
(9, 25, 15, 1),
(9, 30, 15, 1),
(9, 35, 15, 1);
-- Docking = 17
INSERT INTO roleperm (level, PermissionId, menuId, LastModifiedAdminId) VALUES
(9, 1, 17, 1),
(9, 2, 17, 1),
(9, 3, 17, 1),
(9, 4, 17, 1),
(9, 5, 17, 1),
(9, 14, 17, 1),
(9, 15, 17, 1),
(9, 24, 15, 1),
(9, 25, 15, 1),
(9, 30, 17, 1),
(9, 35, 17, 1);
-- Token = 19
INSERT INTO roleperm (level, PermissionId, menuId, LastModifiedAdminId) VALUES
(9, 1, 19,1),
(9, 2, 19,1),
(9, 3, 19,1),
(9, 4, 19,1),
(9, 5, 19,1),
(9, 14, 19, 1),
(9, 15, 19, 1),
(9, 30, 19, 1);
-- Kensington = 21
INSERT INTO roleperm (level, PermissionId, menuId, LastModifiedAdminId) VALUES
(9, 1, 21, 1),
(9, 2, 21, 1),
(9, 3, 21, 1),
(9, 4, 21, 1),
(9, 5, 21, 1),
(9, 8, 21, 1),
(9, 9, 21, 1),
(9, 26, 21, 1);
-- Mobile = 23
INSERT INTO roleperm (level, PermissionId, menuId, LastModifiedAdminId) VALUES
(9, 1, 23, 1),
(9, 2, 23, 1),
(9, 3, 23, 1),
(9, 4, 23, 1),
(9, 5, 23, 1),
(9, 14, 23, 1),
(9, 15, 23, 1),
(9, 12, 23, 1),
(9, 13, 23, 1),
(9, 29, 23, 1),
(9, 30, 23, 1);
-- Subscription = 25
INSERT INTO roleperm (level, PermissionId, menuId, LastModifiedAdminId) VALUES
(9, 1, 25, 1),
(9, 2, 25, 1),
(9, 3, 25, 1),
(9, 4, 25, 1),
(9, 5, 25, 1),
(9, 10, 25, 1),
(9, 11, 25, 1),
(9, 14, 25, 1),
(9, 15, 25, 1),
(9, 28, 25, 1),
(9, 30, 25, 1);
-- Asset Type = 28
INSERT INTO roleperm (level, PermissionId, menuId, LastModifiedAdminId) VALUES
(9, 1, 28, 1),
(9, 2, 28, 1),
(9, 3, 28, 1),
(9, 4, 28, 1),
(9, 5, 28, 1);
-- Asset Category = 30
INSERT INTO roleperm (level, PermissionId, menuId, LastModifiedAdminId) VALUES
(9, 1, 30, 1),
(9, 2, 30, 1),
(9, 3, 30, 1),
(9, 4, 30, 1),
(9, 5, 30, 1);
-- Identity Type = 32
INSERT INTO roleperm (level, PermissionId, menuId, LastModifiedAdminId) VALUES
(9, 1, 32, 1),
(9, 2, 32, 1),
(9, 3, 32, 1),
(9, 4, 32, 1),
(9, 5, 32, 1);
-- ACCOUNT Type = 34
INSERT INTO roleperm (level, PermissionId, menuId, LastModifiedAdminId) VALUES
(9, 1, 34, 1),
(9, 2, 34, 1),
(9, 3, 34, 1),
(9, 4, 34, 1),
(9, 5, 34, 1);
-- Role Type = 36
INSERT INTO roleperm (level, PermissionId, menuId, LastModifiedAdminId) VALUES
(9, 1, 36, 1),
(9, 2, 36, 1),
(9, 3, 36, 1),
(9, 4, 36, 1),
(9, 5, 36, 1);
-- Subscription type = 38
INSERT INTO roleperm (level, PermissionId, menuId, LastModifiedAdminId) VALUES
(9, 1, 38, 1),
(9, 2, 38, 1),
(9, 3, 38, 1),
(9, 4, 38, 1),
(9, 5, 38, 1);
-- System = 41
INSERT INTO roleperm (level, PermissionId, menuId, LastModifiedAdminId) VALUES
(9, 1, 41, 1),
(9, 2, 41, 1),
(9, 3, 41, 1),
(9, 4, 41, 1),
(9, 5, 41, 1),
(9, 6, 41, 1),
(9, 7, 41, 1),
(9, 32, 41, 1);
-- Application = 44
INSERT INTO roleperm (level, PermissionId, menuId, LastModifiedAdminId) VALUES
(9, 1, 44, 1),
(9, 2, 44, 1),
(9, 3, 44, 1),
(9, 4, 44, 1),
(9, 5, 44, 1),
(9, 6, 44, 1),
(9, 7, 44, 1),
(9, 31, 44, 1);
-- Admin = 47
INSERT INTO roleperm (level, PermissionId, menuId, LastModifiedAdminId) VALUES
(9, 1, 47, 1),
(9, 2, 47, 1),
(9, 3, 47, 1),
(9, 4, 47, 1),
(9, 5, 47, 1),
(9, 6, 47, 1),
(9, 7, 47, 1),
(9, 27, 47, 1);
-- Permission = 49
INSERT INTO roleperm (level, PermissionId, menuId, LastModifiedAdminId) VALUES
(9, 1, 49, 1),
(9, 2, 49, 1),
(9, 3, 49, 1),
(9, 5, 49, 1),
(9, 39, 49, 1);
-- RolePermission = 51
INSERT INTO roleperm (level, PermissionId, menuId, LastModifiedAdminId) VALUES
(9, 1, 51, 1),
(9, 2, 51, 1),
(9, 3, 51, 1),
(9, 5, 51, 1);