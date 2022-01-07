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
INSERT INTO accounttype (Type, Description) Values
('Normal User','Normal User no aditional wrights'),
('Administrator','Admnistrator Account'),
('Servive User','User used to run services');
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
-- IdentityType
insert into identitytype (Type,Description) values
('Werknemer','Werknemer'),
('Exeterne','Externe medewerker');
-- roletype
insert into roletype (Type,Description) values
('Application','The Application Role'),
('System','The System Role');
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
INSERT INTO account (UserID,TypeId,ApplicationId) VALUES ('Root',2,2);
--Identity
Insert into [Identity] (Name,LanguageCode,EMail,Company,UserID,TypeId) Values('Stock','NL','root@cmdb.com','CMDB','ROOT',1);
--role
insert into role (Name, Description, TypeId) VALUES ('Administrator','The administrator of the Application',1);
-- idenaccount
insert into IdenAccount (IdentityId,AccountId,ValidFrom,ValidUntil) values
(1,1,'2012-01-01 00:00:00','9999-12-31 23:59:00');
-- Admin
INSERT INTO Admin (AccountId,Level,PassWord,DateSet) VALUES
(1,9,'61a99380acad7d202889ecfa941a38e6','2012-01-01 00:00:00');
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
('Overview','Permission',49);--50
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
('MenuOverview','This Permission is to see the Menu');
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
(9, 5, 49, 1);
insert into log (AssetCategoryId, LogText, LogDate) values
(1,'Kensington Created by SQL Import','2012-01-01 00:00:00'),
(2,'Mobile Created by SQL Import','2012-01-01 00:00:00'),
(3,'Mobile Subscription Created by SQL Import','2012-01-01 00:00:00'),
(4,'Internet Subscription Created by SQL Import','2012-01-01 00:00:00'),
(5,'Laptop Created by SQL Import','2012-01-01 00:00:00'),
(6,'Desktop Created by SQL Import','2012-01-01 00:00:00'),
(7,'Token Created by SQL Import','2012-01-01 00:00:00');
insert into log (IdentityTypeId, LogText, LogDate) values
(1,'Werknemer Created by SQL Import','2012-01-01 00:00:00'),
(2,'Exeterne Medewerker Created by SQL Import','2012-01-01 00:00:00');
insert into log (SubscriptionTypeId, LogText, LogDate) values
(1,'Telenet Expresnet Created by SQL Import','2012-01-01 00:00:00'),
(2,'Telenet Fibernet Created by SQL Import','2012-01-01 00:00:00'),
(3,'Belgacom ADSL Created by SQL Import','2012-01-01 00:00:00'),
(4,'Belgacom VDSL Created by SQL Import','2012-01-01 00:00:00'),
(5,'Proximus Split Created by SQL Import','2012-01-01 00:00:00'),
(6,'Proximus EUR 25 Created by SQL Import','2012-01-01 00:00:00'),
(7,'Proximus FULL Created by SQL Import','2012-01-01 00:00:00');
insert into log (IdentityId, LogText, LogDate) values
(1,'Identity Stock Created by SQL Import','2012-01-01 00:00:00');
insert into log(AccountId,LogText, LogDate) values
(1,'Account Root Created by SQL Import','2012-01-01 00:00:00');
insert into log (AccountTypeId, LogText, LogDate) values
(1,'Accounttype Normal User Created by SQL Import','2012-01-01 00:00:00'),
(2,'Accounttype Administrator Created by SQL Import','2012-01-01 00:00:00'),
(3,'Accounttype Servive User Created by SQL Import','2012-01-01 00:00:00');
insert into log (ApplicationId, LogText, LogDate) values
(1,'Application Active Directory Created by SQL Import','2012-01-01 00:00:00'),
(2,'Application CMDB Created by SQL Import','2012-01-01 00:00:00');
insert into log (RoleTypeId, LogText, LogDate) values
(1,'Roletype Application Created by SQL Import','2012-01-01 00:00:00'),
(2,'Roletype System Created by SQL Import','2012-01-01 00:00:00');
insert into log (RoleId, LogText, LogDate) values
(1, 'The Role Administrator is created by SQL Import', '2012-01-01 00:00:00');
insert into log (PermissionId, LogText, LogDate) values
(1,'The permission Read Created by SQL Import','2012-01-01 00:00:00'),
(2,'The permission Update Created by SQL Import','2012-01-01 00:00:00'),
(3,'The permission Delete Created by SQL Import','2012-01-01 00:00:00'),
(4,'The permission Activate Created by SQL Import','2012-01-01 00:00:00'),
(5,'The permission Add Created by SQL Import','2012-01-01 00:00:00'),
(6,'The permission Assign account Created by SQL Import','2012-01-01 00:00:00'),
(7,'The permission Account overview Created by SQL Import','2012-01-01 00:00:00'),
(8,'The permission Assign a device Created by SQL Import','2012-01-01 00:00:00'),
(9,'The permission Device overview Created by SQL Import','2012-01-01 00:00:00'),
(10,'The permission Assign mobile Created by SQL Import','2012-01-01 00:00:00'),
(11,'The permission Mobile overview Created by SQL Import','2012-01-01 00:00:00'),
(12,'The permission Assign Subscription Created by SQL Import','2012-01-01 00:00:00'),
(13,'The permission Subscription overview Created by SQL Import','2012-01-01 00:00:00'),
(14,'The permission Assign Idenity Created by SQL Import','2012-01-01 00:00:00'),
(15,'The permission Identity overview Created by SQL Import','2012-01-01 00:00:00'),
(16,'The permission Assign application role Created by SQL Import','2012-01-01 00:00:00'),
(17,'The permission Application role overview Created by SQL Import','2012-01-01 00:00:00'),
(18,'The permission Assign system role Created by SQL Import','2012-01-01 00:00:00'),
(19,'The permission System role overiew Created by SQL Import','2012-01-01 00:00:00'),
(20,'The permission Assign appliction Created by SQL Import','2012-01-01 00:00:00'),
(21,'The permission Application overview Created by SQL Import','2012-01-01 00:00:00'),
(22,'The permission Assign System Created by SQL Import','2012-01-01 00:00:00'),
(23,'The permission System overview Created by SQL Import','2012-01-01 00:00:00'),
(24,'The permission Assign Kensington Created by SQL Import','2012-01-01 00:00:00'),
(25,'The permission Kensington overview Created by SQL Import','2012-01-01 00:00:00'),
(26,'The permission Release device Created by SQL Import','2012-01-01 00:00:00'),
(27,'The permission Release account Created by SQL Import','2012-01-01 00:00:00'),
(28,'The permission Release mobile Created by SQL Import','2012-01-01 00:00:00'),
(29,'The permission Release subscription Created by SQL Import','2012-01-01 00:00:00'),
(30,'The permission Release identity Created by SQL Import','2012-01-01 00:00:00'),
(31,'The permission Release appliction role Created by SQL Import','2012-01-01 00:00:00'),
(32,'The permission Release system role Created by SQL Import','2012-01-01 00:00:00'),
(33,'The permission Release application Created by SQL Import','2012-01-01 00:00:00'),
(34,'The permission Release system Created by SQL Import','2012-01-01 00:00:00'),
(35,'The permission Release kensington Created by SQL Import','2012-01-01 00:00:00'),
(36,'The permission Assign Level Created by SQL Import','2012-01-01 00:00:00'),
(37,'The permission Permission overview Created by SQL Import','2012-01-01 00:00:00'),
(38,'The permission Menu overview Created by SQL Import','2012-01-01 00:00:00');