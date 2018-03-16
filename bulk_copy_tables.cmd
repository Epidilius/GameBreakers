@ECHO OFF
SET MYPATH=%~dp0
IF EXIST %MYPATH%bulk_copy_errors.log del /F %MYPATH%bulk_copy_errors.log
set arg_source_password=""
SET DADADIR=%TEMP%\
set arg_target_password="Rainbow1"
pushd %DADADIR%
echo [0 %%] Creating directory dump_GameBreakers
mkdir dump_GameBreakers
pushd dump_GameBreakers
copy NUL import_GameBreakers.sql
echo SET SESSION UNIQUE_CHECKS=0; >> import_GameBreakers.sql
echo SET SESSION FOREIGN_KEY_CHECKS=0; >> import_GameBreakers.sql
echo use GameBreakers; >> import_GameBreakers.sql
echo [10 %%] Start dumping tables
bcp "SELECT [ID], CAST([Expansion] as NVARCHAR(MAX)) as [Expansion], CAST([Category] as NVARCHAR(MAX)) as [Category], CAST([Year] as NVARCHAR(MAX)) as [Year], CAST([Sport] as NVARCHAR(MAX)) as [Sport], CAST([Brand] as NVARCHAR(MAX)) as [Brand], CAST([Number] as NVARCHAR(MAX)) as [Number], CAST([Name] as NVARCHAR(MAX)) as [Name], CAST([Team] as NVARCHAR(MAX)) as [Team], CAST([PrintRun] as NVARCHAR(MAX)) as [PrintRun], CAST([Odds] as NVARCHAR(MAX)) as [Odds], [Inventory], CAST([ExtraData] as NVARCHAR(MAX)) as [ExtraData], CAST([MD5Hash] as NVARCHAR(MAX)) as [MD5Hash] FROM [GameBreakers].[dbo].[Non_Mtg]" queryout Non_Mtg.csv -c -t, -T -S .\SQLEXPRESSJOEL -U  -P %arg_source_passwords 2>> "%MYPATH%bulk_copy_errors.log"
if %ERRORLEVEL% GEQ 1 (
    echo Script has failed. See the log file for details.
    exit /b 1
)
echo LOAD DATA INFILE 'GameBreakers_#####_import/Non_Mtg.csv' INTO TABLE Non_Mtg FIELDS TERMINATED BY ',' ENCLOSED BY ''; >> import_GameBreakers.sql
echo [20 %%] Dumped table Non_Mtg
bcp "SELECT [ID], CAST([AttemptedAction] as NVARCHAR(MAX)) as [AttemptedAction], CAST([Error] as NVARCHAR(MAX)) as [Error], CAST([ExtraData] as NVARCHAR(MAX)) as [ExtraData], CAST([TimeOfError] as NVARCHAR(MAX)) as [TimeOfError], CAST([ParentFunction] as NVARCHAR(MAX)) as [ParentFunction] FROM [GameBreakers].[dbo].[Errors]" queryout Errors.csv -c -t, -T -S .\SQLEXPRESSJOEL -U  -P %arg_source_passwords 2>> "%MYPATH%bulk_copy_errors.log"
if %ERRORLEVEL% GEQ 1 (
    echo Script has failed. See the log file for details.
    exit /b 1
)
echo LOAD DATA INFILE 'GameBreakers_#####_import/Errors.csv' INTO TABLE Errors FIELDS TERMINATED BY ',' ENCLOSED BY ''; >> import_GameBreakers.sql
echo [30 %%] Dumped table Errors
bcp "SELECT CAST([Sport] as NVARCHAR(MAX)) as [Sport] FROM [GameBreakers].[dbo].[SportList]" queryout SportList.csv -c -t, -T -S .\SQLEXPRESSJOEL -U  -P %arg_source_passwords 2>> "%MYPATH%bulk_copy_errors.log"
if %ERRORLEVEL% GEQ 1 (
    echo Script has failed. See the log file for details.
    exit /b 1
)
echo LOAD DATA INFILE 'GameBreakers_#####_import/SportList.csv' INTO TABLE SportList FIELDS TERMINATED BY ',' ENCLOSED BY ''; >> import_GameBreakers.sql
echo [40 %%] Dumped table SportList
bcp "SELECT [ID], CAST([CardIDs] as NVARCHAR(MAX)) as [CardIDs], CAST([CardNames] as NVARCHAR(MAX)) as [CardNames], CAST([CardExpansions] as NVARCHAR(MAX)) as [CardExpansions], CAST([CardAmounts] as NVARCHAR(MAX)) as [CardAmounts], CAST([CustomerName] as NVARCHAR(MAX)) as [CustomerName], CAST([CustomerNumber] as NVARCHAR(MAX)) as [CustomerNumber], CAST([CustomerEmail] as NVARCHAR(MAX)) as [CustomerEmail], CAST([LastUpdated] as NVARCHAR(MAX)) as [LastUpdated], CAST([CardPrices] as NVARCHAR(MAX)) as [CardPrices], CAST([Subtotal] as NVARCHAR(MAX)) as [Subtotal], CAST([Taxes] as NVARCHAR(MAX)) as [Taxes], CAST([Total] as NVARCHAR(MAX)) as [Total], CAST([Status] as NVARCHAR(50)) as [Status] FROM [GameBreakers].[dbo].[Carts]" queryout Carts.csv -c -t, -T -S .\SQLEXPRESSJOEL -U  -P %arg_source_passwords 2>> "%MYPATH%bulk_copy_errors.log"
if %ERRORLEVEL% GEQ 1 (
    echo Script has failed. See the log file for details.
    exit /b 1
)
echo LOAD DATA INFILE 'GameBreakers_#####_import/Carts.csv' INTO TABLE Carts FIELDS TERMINATED BY ',' ENCLOSED BY ''; >> import_GameBreakers.sql
echo [50 %%] Dumped table Carts
bcp "SELECT [ID], CAST([cardID] as NVARCHAR(50)) as [cardID], CAST([layout] as NVARCHAR(50)) as [layout], CAST([name] as NVARCHAR(MAX)) as [name], CAST([manaCost] as NVARCHAR(50)) as [manaCost], [cmc], CAST([colors] as NVARCHAR(50)) as [colors], CAST([rarity] as NVARCHAR(50)) as [rarity], CAST([type] as NVARCHAR(50)) as [type], CAST([types] as NVARCHAR(50)) as [types], CAST([subtypes] as NVARCHAR(50)) as [subtypes], CAST([text] as NVARCHAR(MAX)) as [text], CAST([flavorText] as NVARCHAR(MAX)) as [flavorText], CAST([power] as NVARCHAR(50)) as [power], CAST([toughness] as NVARCHAR(50)) as [toughness], CAST([colorIdentity] as NVARCHAR(50)) as [colorIdentity], CAST([multiverseID] as NVARCHAR(50)) as [multiverseID], CAST([expansion] as NVARCHAR(50)) as [expansion], [price], [inventory], [foilPrice], [foilInventory], [priceLastUpdated], [onlineOnlyVersion], [backroom], [showcase], [foilBackroom], [foilShowcase] FROM [GameBreakers].[dbo].[MtG]" queryout MtG.csv -c -t, -T -S .\SQLEXPRESSJOEL -U  -P %arg_source_passwords 2>> "%MYPATH%bulk_copy_errors.log"
if %ERRORLEVEL% GEQ 1 (
    echo Script has failed. See the log file for details.
    exit /b 1
)
echo LOAD DATA INFILE 'GameBreakers_#####_import/MtG.csv' INTO TABLE MtG FIELDS TERMINATED BY ',' ENCLOSED BY ''; >> import_GameBreakers.sql
echo [60 %%] Dumped table MtG
bcp "SELECT CAST([Action] as NVARCHAR(MAX)) as [Action], CAST([TimeOfActivity] as NVARCHAR(MAX)) as [TimeOfActivity], CAST([ExtraData] as NVARCHAR(MAX)) as [ExtraData] FROM [GameBreakers].[dbo].[ActivityLog]" queryout ActivityLog.csv -c -t, -T -S .\SQLEXPRESSJOEL -U  -P %arg_source_passwords 2>> "%MYPATH%bulk_copy_errors.log"
if %ERRORLEVEL% GEQ 1 (
    echo Script has failed. See the log file for details.
    exit /b 1
)
echo LOAD DATA INFILE 'GameBreakers_#####_import/ActivityLog.csv' INTO TABLE ActivityLog FIELDS TERMINATED BY ',' ENCLOSED BY ''; >> import_GameBreakers.sql
echo [70 %%] Dumped table ActivityLog
bcp "SELECT [ID], CAST([Name] as NVARCHAR(50)) as [Name], CAST([Abbreviation] as NVARCHAR(50)) as [Abbreviation], [Symbol], [Locked], [SetID], CAST([Type] as NVARCHAR(50)) as [Type] FROM [GameBreakers].[dbo].[Sets]" queryout Sets.csv -c -t, -T -S .\SQLEXPRESSJOEL -U  -P %arg_source_passwords 2>> "%MYPATH%bulk_copy_errors.log"
if %ERRORLEVEL% GEQ 1 (
    echo Script has failed. See the log file for details.
    exit /b 1
)
echo LOAD DATA INFILE 'GameBreakers_#####_import/Sets.csv' INTO TABLE Sets FIELDS TERMINATED BY ',' ENCLOSED BY ''; >> import_GameBreakers.sql
echo [80 %%] Dumped table Sets
copy NUL import_GameBreakers.sh
(echo #!/bin/bash) >> import_GameBreakers.sh
(echo MYPATH=\`pwd\`) >> import_GameBreakers.sh
(echo if [ -f \$MYPATH/import_errors.log ] ; then) >> import_GameBreakers.sh
(echo     rm \$MYPATH/import_errors.log) >> import_GameBreakers.sh
(echo fi) >> import_GameBreakers.sh
(echo TARGET_DIR=\`MYSQL_PWD=$arg_target_password mysql -h127.0.0.1 -P3306 -uGBMan -s -N information_schema -e 'SELECT Variable_Value FROM GLOBAL_VARIABLES WHERE Variable_Name = \"datadir\"'\` 2>> \$MYPATH/import_errors.log) >> import_GameBreakers.sh
(echo if [ \$? -ne 0 ];then) >> import_GameBreakers.sh
(echo    echo Script has failed. See the log file for details.) >> import_GameBreakers.sh
(echo    exit 1) >> import_GameBreakers.sh
(echo fi) >> import_GameBreakers.sh
(echo pushd \$TARGET_DIR) >> import_GameBreakers.sh
(echo mkdir GameBreakers_#####_import) >> import_GameBreakers.sh
(echo cp \$MYPATH/*.csv GameBreakers_#####_import/ 2>> \$MYPATH/import_errors.log) >> import_GameBreakers.sh
(echo if [ \$? -ne 0 ];then) >> import_GameBreakers.sh
(echo    echo Script has failed. See the log file for details.) >> import_GameBreakers.sh
(echo    exit 1) >> import_GameBreakers.sh
(echo fi) >> import_GameBreakers.sh
(echo cp \$MYPATH/*.sql GameBreakers_#####_import/ 2>> \$MYPATH/import_errors.log) >> import_GameBreakers.sh
(echo if [ \$? -ne 0 ];then) >> import_GameBreakers.sh
(echo    echo Script has failed. See the log file for details.) >> import_GameBreakers.sh
(echo    exit 1) >> import_GameBreakers.sh
(echo fi) >> import_GameBreakers.sh
(echo echo Started load data. Please wait.) >> import_GameBreakers.sh
(echo MYSQL_PWD=$arg_target_password mysql -h127.0.0.1 -P3306 -uGBMan < GameBreakers_#####_import/import_GameBreakers.sql 2>> \$MYPATH/import_errors.log) >> import_GameBreakers.sh
(echo if [ \$? -ne 0 ];then) >> import_GameBreakers.sh
(echo    echo Script has failed. See the log file for details.) >> import_GameBreakers.sh
(echo    exit 1) >> import_GameBreakers.sh
(echo fi) >> import_GameBreakers.sh
(echo echo Finished load data) >> import_GameBreakers.sh
(echo rm -rf GameBreakers_#####_import) >> import_GameBreakers.sh
(echo popd) >> import_GameBreakers.sh
echo [90 %%] Generated import script import_GameBreakers.sh
popd
set TEMPDIR=%DADADIR%dump_GameBreakers
echo Set fso = CreateObject("Scripting.FileSystemObject") > _zipIt.vbs
echo InputFolder = fso.GetAbsolutePathName(WScript.Arguments.Item(0)) >> _zipIt.vbs
echo ZipFile = fso.GetAbsolutePathName(WScript.Arguments.Item(1)) >> _zipIt.vbs
echo CreateObject("Scripting.FileSystemObject").CreateTextFile(ZipFile, True).Write "PK" ^& Chr(5) ^& Chr(6) ^& String(18, vbNullChar) >> _zipIt.vbs
echo Set objShell = CreateObject("Shell.Application") >> _zipIt.vbs
echo Set source = objShell.NameSpace(InputFolder).Items >> _zipIt.vbs
echo objShell.NameSpace(ZipFile).CopyHere(source) >> _zipIt.vbs
echo Do Until objShell.NameSpace( ZipFile ).Items.Count ^= objShell.NameSpace( InputFolder ).Items.Count >> _zipIt.vbs
echo wScript.Sleep 200 >> _zipIt.vbs
echo Loop >> _zipIt.vbs
CScript  _zipIt.vbs  "%TEMPDIR%"  "%DADADIR%dump_GameBreakers.zip" 2>> "%MYPATH%bulk_copy_errors.log"
if %ERRORLEVEL% GEQ 1 (
    echo Script has failed. See the log file for details.
    exit /b 1
)
echo [100 %%] Zipped all files to dump_GameBreakers.zip file
xcopy dump_GameBreakers.zip %MYPATH% 2>> "%MYPATH%bulk_copy_errors.log"
if %ERRORLEVEL% GEQ 1 (
    echo Script has failed. See the log file for details.
    exit /b 1
)
del dump_GameBreakers.zip
del _zipIt.vbs
del /F /Q dump_GameBreakers\*.*
rmdir dump_GameBreakers
popd
echo Now you can copy %MYPATH%dump_GameBreakers.zip file to the target server and run the import script.
pause
