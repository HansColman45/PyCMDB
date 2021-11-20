@echo off
IF EXIST "%1" CD "%1"

SET environment=LOCAL
SET connectionstring="Server=.;Database=CMDB;User Id=sa;Password=Gr7k6VKW92dteZ5n"

SET sql.files.directory=.\db\

echo *** Running RoundhousE
echo.
rh.exe /f=%sql.files.directory% /cs=%connectionstring% /vf="sql" /env=%environment% /ct:3600 /silent
echo.
IF NOT EXIST "%1" PAUSE
