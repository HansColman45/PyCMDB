USE master;
GO
DECLARE @Created bit
SET @Created = 0

IF NOT EXISTS(SELECT * FROM sys.databases WHERE [name] = '{{DatabaseName}}')
BEGIN
        Set @Created = 1
        CREATE DATABASE [{{DatabaseName}}]
        COLLATE Latin1_General_CI_AS
END

SELECT @Created
