@Echo off

IF [%1] neq [] chdir %1

call deploy.bat Development template_web postgres "C:\Program Files\PostgreSQL\12\bin\psql.exe" true
if %errorlevel% neq 0 exit /b %errorlevel%

Echo.
::call q:\Install\DB\DBPatch\Disig.DBPatch.exe -c "Host=localhost;Database=mba;Username=postgres;Password={0}" -u "mbauser"
