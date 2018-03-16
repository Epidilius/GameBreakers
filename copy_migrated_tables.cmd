@ECHO OFF
set arg_source_password=
set arg_target_password="Rainbow1"

IF [%arg_source_password%] == [] (
    IF [%arg_target_password%] == [] (
        ECHO WARNING: Both source and target RDBMSes passwords are empty. You should edit this file to set them.
    )
)
set arg_worker_count=2

set table_file="%TMP%\wb_tables_to_migrate.txt"
TYPE NUL > "%TMP%\wb_tables_to_migrate.txt"
ECHO [GameBreakers]	[dbo].[Non_Mtg]	`GameBreakers`	`Non_Mtg`	-	-	[ID], CAST([Expansion] as NVARCHAR(MAX)) as [Expansion], CAST([Category] as NVARCHAR(MAX)) as [Category], CAST([Year] as NVARCHAR(MAX)) as [Year], CAST([Sport] as NVARCHAR(MAX)) as [Sport], CAST([Brand] as NVARCHAR(MAX)) as [Brand], CAST([Number] as NVARCHAR(MAX)) as [Number], CAST([Name] as NVARCHAR(MAX)) as [Name], CAST([Team] as NVARCHAR(MAX)) as [Team], CAST([PrintRun] as NVARCHAR(MAX)) as [PrintRun], CAST([Odds] as NVARCHAR(MAX)) as [Odds], [Inventory], CAST([ExtraData] as NVARCHAR(MAX)) as [ExtraData], CAST([MD5Hash] as NVARCHAR(MAX)) as [MD5Hash] >> "%TMP%\wb_tables_to_migrate.txt"
ECHO [GameBreakers]	[dbo].[Errors]	`GameBreakers`	`Errors`	-	-	[ID], CAST([AttemptedAction] as NVARCHAR(MAX)) as [AttemptedAction], CAST([Error] as NVARCHAR(MAX)) as [Error], CAST([ExtraData] as NVARCHAR(MAX)) as [ExtraData], CAST([TimeOfError] as NVARCHAR(MAX)) as [TimeOfError], CAST([ParentFunction] as NVARCHAR(MAX)) as [ParentFunction] >> "%TMP%\wb_tables_to_migrate.txt"
ECHO [GameBreakers]	[dbo].[SportList]	`GameBreakers`	`SportList`	-	-	CAST([Sport] as NVARCHAR(MAX)) as [Sport] >> "%TMP%\wb_tables_to_migrate.txt"
ECHO [GameBreakers]	[dbo].[Carts]	`GameBreakers`	`Carts`	-	-	[ID], CAST([CardIDs] as NVARCHAR(MAX)) as [CardIDs], CAST([CardNames] as NVARCHAR(MAX)) as [CardNames], CAST([CardExpansions] as NVARCHAR(MAX)) as [CardExpansions], CAST([CardAmounts] as NVARCHAR(MAX)) as [CardAmounts], CAST([CustomerName] as NVARCHAR(MAX)) as [CustomerName], CAST([CustomerNumber] as NVARCHAR(MAX)) as [CustomerNumber], CAST([CustomerEmail] as NVARCHAR(MAX)) as [CustomerEmail], CAST([LastUpdated] as NVARCHAR(MAX)) as [LastUpdated], CAST([CardPrices] as NVARCHAR(MAX)) as [CardPrices], CAST([Subtotal] as NVARCHAR(MAX)) as [Subtotal], CAST([Taxes] as NVARCHAR(MAX)) as [Taxes], CAST([Total] as NVARCHAR(MAX)) as [Total], CAST([Status] as NVARCHAR(50)) as [Status] >> "%TMP%\wb_tables_to_migrate.txt"
ECHO [GameBreakers]	[dbo].[MtG]	`GameBreakers`	`MtG`	[ID]	`ID`	[ID], CAST([cardID] as NVARCHAR(50)) as [cardID], CAST([layout] as NVARCHAR(50)) as [layout], CAST([name] as NVARCHAR(MAX)) as [name], CAST([manaCost] as NVARCHAR(50)) as [manaCost], [cmc], CAST([colors] as NVARCHAR(50)) as [colors], CAST([rarity] as NVARCHAR(50)) as [rarity], CAST([type] as NVARCHAR(50)) as [type], CAST([types] as NVARCHAR(50)) as [types], CAST([subtypes] as NVARCHAR(50)) as [subtypes], CAST([text] as NVARCHAR(MAX)) as [text], CAST([flavorText] as NVARCHAR(MAX)) as [flavorText], CAST([power] as NVARCHAR(50)) as [power], CAST([toughness] as NVARCHAR(50)) as [toughness], CAST([colorIdentity] as NVARCHAR(50)) as [colorIdentity], CAST([multiverseID] as NVARCHAR(50)) as [multiverseID], CAST([expansion] as NVARCHAR(50)) as [expansion], [price], [inventory], [foilPrice], [foilInventory], [priceLastUpdated], [onlineOnlyVersion], [backroom], [showcase], [foilBackroom], [foilShowcase] >> "%TMP%\wb_tables_to_migrate.txt"
ECHO [GameBreakers]	[dbo].[ActivityLog]	`GameBreakers`	`ActivityLog`	-	-	CAST([Action] as NVARCHAR(MAX)) as [Action], CAST([TimeOfActivity] as NVARCHAR(MAX)) as [TimeOfActivity], CAST([ExtraData] as NVARCHAR(MAX)) as [ExtraData] >> "%TMP%\wb_tables_to_migrate.txt"
ECHO [GameBreakers]	[dbo].[Sets]	`GameBreakers`	`Sets`	[ID]	`ID`	[ID], CAST([Name] as NVARCHAR(50)) as [Name], CAST([Abbreviation] as NVARCHAR(50)) as [Abbreviation], [Symbol], [Locked], [SetID], CAST([Type] as NVARCHAR(50)) as [Type] >> "%TMP%\wb_tables_to_migrate.txt"


wbcopytables.exe ^
 --odbc-source="DRIVER={SQL Server};SERVER=JOELSPC\SQLEXPRESSJOEL;DATABASE={};UID=" ^
 --target="GBMan@parallelzodiac.com:3306" ^
 --source-password="%arg_source_password%" ^
 --target-password="%arg_target_password%" ^
 --table-file="%table_file%" --thread-count=%arg_worker_count% ^
 %arg_truncate_target% ^
 %arg_debug_output%

DEL "%TMP%\wb_tables_to_migrate.txt"


