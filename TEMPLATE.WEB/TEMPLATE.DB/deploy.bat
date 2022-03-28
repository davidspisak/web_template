@Echo off

SET PGCLIENTENCODING=utf-8 

SetLocal
	Set err="%~dp0error.txt"
	Set log="%~dp0log.txt"

	Set db="%~dp0db.sql"
	Set configuredb="%~dp0configuredb.sql"
	Set schemas="%~dp0schemas.sql"
	Set tables="%~dp0tables.sql"
	Set views="%~dp0views.sql"
	Set perms="%~dp0permissions.sql"
	Set init="%~dp0data_initial.sql"
	Set entities="%~dp0Entities_InsertScript.sql"
	Set activities="%~dp0Activities_InsertScript.sql"
	Set operations="%~dp0Operations_InsertScript.sql"
	Set operations_update="%~dp0Operations_UpdateScript.sql"
	Set samples="%~dp0data_sample.sql"
	Set procedures="%~dp0procedures.sql"

	Set dbusr=%3
	IF [%3] == [] Set dbusr="postgres"
	
	Set PSQL=%4
	IF [%4] == [] Set PSQL="C:\Program Files\PostgreSQL\12\bin\psql.exe"
	Set PGOPTIONS=--client-min-messages=warning

	If exist %err% ( Del %err% )
	If exist %log% ( Del %log% )

	Echo NOTICE: Database Deployment started {Mode: %1} 
		
	Echo NOTICE: Drop database

	%PSQL% -c "SELECT pg_terminate_backend(pg_stat_activity.pid) FROM pg_stat_activity WHERE pg_stat_activity.datname = '%2' AND pid <> pg_backend_pid()" -U %dbusr% -h localhost -d %2 -q 2 > %err%
  
	%PSQL% -c "DROP DATABASE IF EXISTS %2" -U %dbusr% -h localhost -d postgres -q 2 > %err%

	For %%I In (%err%) Do Set /a size=%%~zI
	If %size% GTR 0 (
		Echo.
		Echo FAILURE: Error while dropping the database
		Goto :error
	)

	Echo SUCCESS: Drop database
		
	Echo NOTICE: Create database

	%PSQL% -c "CREATE DATABASE %2 WITH OWNER = postgres TEMPLATE = template0 ENCODING = 'UTF8' LC_COLLATE = 'Slovak_Slovakia.1250' LC_CTYPE = 'Slovak_Slovakia.1250' TABLESPACE = pg_default CONNECTION LIMIT = -1" -U %dbusr% -h localhost -d postgres -q 2 > %err%

	For %%I In (%err%) Do Set /a size=%%~zI
	If %size% GTR 0 (
		Echo.
		Echo FAILURE: Error while creating the database
		Goto :error
	)

	Echo SUCCESS: Create database

	If Not Exist %configuredb% (
		Echo NOTICE: No configuredb.sql found.
		Goto :schemas
	)
	
	Echo NOTICE: Deploying DB Configuration

	REM add "-v ON_ERROR_STOP=1" to stop on first error
	%PSQL% -f %configuredb% -U %dbusr% -h localhost -d %2 -q 2> %err%
  
	For %%I In (%err%) Do Set /a size=%%~zI
	If %size% GTR 0 (
		Echo.
		Echo FAILURE: Error in configuredb
		Goto :error
	)

	Echo SUCCESS: DB Configuration deployed

	:schemas
	If Not Exist %schemas% (
		Echo NOTICE: No Schemas found.
		Goto :tables
	)
	
	Echo NOTICE: Deploying Schemas

	REM add "-v ON_ERROR_STOP=1" to stop on first error
	%PSQL% -f %schemas% -U %dbusr% -h localhost -d %2 -q 2> %err%
  
	For %%I In (%err%) Do Set /a size=%%~zI
	If %size% GTR 0 (
		Echo.
		Echo FAILURE: Error in Schemas
		Goto :error
	)

	Echo SUCCESS: Schemas deployed

	:tables
	If Not Exist %tables% (
		Echo NOTICE: No Tables found.
		Goto :views
	)
	
	Echo NOTICE: Deploying Tables

	%PSQL% -f %tables% -U %dbusr% -h localhost -d %2 -q 2> %err%
  
	For %%I In (%err%) Do Set /a size=%%~zI
	If %size% GTR 0 (
		Echo.
		Echo FAILURE: Error in Tables
		Goto :error
	)

	Echo SUCCESS: Tables deployed

	:views
	If Not Exist %views% (
		Echo NOTICE: No Views found.
		Goto :perms
	)

	Echo NOTICE: Deploying Views

	%PSQL% -f %views% -U %dbusr% -h localhost -d %2 -q 2> %err%
  
	For %%I In (%err%) Do Set /a size=%%~zI
	If %size% GTR 0 (
		Echo.
		Echo FAILURE: Error in Views
		Goto :error
	)

	Echo SUCCESS: Views deployed

	:perms
	If Not Exist %perms% (
		Echo NOTICE: No Permissions found.
		Goto :init
	)

	Echo NOTICE: Creating Users and setting Permissions

	%PSQL% -f %perms% -U %dbusr% -h localhost -d %2 -q 2> %err%
  
	For %%I In (%err%) Do Set /a size=%%~zI
	If %size% GTR 0 (
		Echo.
		Echo FAILURE: Error in Users and setting Permissions
		Goto :error
	)

	Echo SUCCESS: Users created and Permissions set
	

	:init
	If Not Exist %init% (
		Echo NOTICE: No Initial Data found.
		Goto :entities
	)

	Echo NOTICE: Deploying Initial Data

	%PSQL% -f %init% -U %dbusr% -h localhost -d %2 -q 2> %err%
  
	For %%I In (%err%) Do Set /a size=%%~zI
	If %size% GTR 0 (
		Echo.
		Echo FAILURE: Error in Initial Data
		Goto :error
	)

	Echo SUCCESS: Initial data deployed
	

	:entities
	If Not Exist %entities% (
		Echo NOTICE: No Entities_InsertScript found.
		Goto :activities
	)

	Echo NOTICE: Deploying Entities_InsertScript

	%PSQL% -f %entities% -U %dbusr% -h localhost -d %2 -q 2> %err%
  
	For %%I In (%err%) Do Set /a size=%%~zI
	If %size% GTR 0 (
		Echo.
		Echo FAILURE: Error in Entities_InsertScript
		Goto :error
	)

	Echo SUCCESS: Entities_InsertScript deployed
	

	:activities
	If Not Exist %activities% (
		Echo NOTICE: No Activities_InsertScript found.
		Goto :operations
	)

	Echo NOTICE: Deploying Activities_InsertScript

	%PSQL% -f %activities% -U %dbusr% -h localhost -d %2 -q 2> %err%
  
	For %%I In (%err%) Do Set /a size=%%~zI
	If %size% GTR 0 (
		Echo.
		Echo FAILURE: Error in Activities_InsertScript
		Goto :error
	)

	Echo SUCCESS: Activities_InsertScript deployed
	

	:operations
	If Not Exist %operations% (
		Echo NOTICE: No Operations_InsertScript found.
		Goto :samples
	)

	Echo NOTICE: Deploying Operations_InsertScript

	%PSQL% -f %operations% -U %dbusr% -h localhost -d %2 -q 2> %err%
  
	For %%I In (%err%) Do Set /a size=%%~zI
	If %size% GTR 0 (
		Echo.
		Echo FAILURE: Error in Operations_InsertScript
		Goto :error
	)

	Echo SUCCESS: Operations_InsertScript deployed

	:operations_update
	If Not Exist %operations_update% (
		Echo NOTICE: No Operations_UpdateScript found.
		Goto :procedures
	)

	Echo NOTICE: Deploying Operations_UpdateScript

	%PSQL% -f %operations_update% -U %dbusr% -h localhost -d %2 -q 2> %err%
  
	For %%I In (%err%) Do Set /a size=%%~zI
	If %size% GTR 0 (
		Echo.
		Echo FAILURE: Error in Operations_UpdateScript
		Goto :error
	)

	Echo SUCCESS: Operations_UpdateScript deployed
	
	
	:procedures
	If Not Exist %procedures% (
		Echo NOTICE: No Procedures found.
		Goto :samples
	)

	Echo NOTICE: Deploying Procedures

	%PSQL% -f %procedures% -U %dbusr% -h localhost -d %2 -q 2> %err%
  
	For %%I In (%err%) Do Set /a size=%%~zI
	If %size% GTR 0 (
		Echo.
		Echo FAILURE: Error in Procedures
		Goto :error
	)

	Echo SUCCESS: Procedures deployed


	:samples
	If Not Exist %samples% (
		Echo NOTICE: No Sample data found.
		Goto :success
	)

	If NOT %1 == Debug If NOT %1 == Development Goto :success

	Echo NOTICE: Deploying sample data

	%PSQL% -f %samples% -U %dbusr% -h localhost -d %2 -q 2> %err%

	For %%I In (%err%) Do Set /a size=%%~zI
	If %size% GTR 0 (
		Echo.
		Echo FAILURE: Error in sample data
		Goto :error
	)

	Echo SUCCESS: Sample data deployed


EndLocal

:success
Echo.
Echo SUCCESS: Database deployment successfully completed {Mode: %1}
IF "%5"=="true" exit /b 0
Goto :endandexit

:error
Echo ----------------------------------
Type %err%
Echo ----------------------------------
Echo.
Echo FAILURE: Database deployment failed
IF "%5"=="true" exit /b 1

:endandexit
