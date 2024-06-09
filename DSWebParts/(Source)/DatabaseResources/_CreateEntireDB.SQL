/*
	DSI enables the execution of DocScript Programs in Real-Time, Multi-Client, Execution-Sessions.

	-----------------------------------------
	DocScript-Interactive Setup Instructions:
	-----------------------------------------
		1) Install MS-SQL-Server (ideally as the Default-Instance)
		2) Run _CreateEntireDB.SQL, [Ctrl+H]-ing {DocScript_DatabaseName_Placeholder} with the chosen name e.g. "DSInteractive"
		3) Specify the Database-Name and SQL-Server in \DSWebParts\API\DSInteractive.config
		4) Either: Ctrl+F5 the WebParts Project in VS,
		5) ...Or host the DSWebParts\ directory with IIS, and give the IIS AppPool Database Permissions (https://stackoverflow.com/questions/7698286/login-failed-for-user-iis-apppool-asp-net-v4-0)
			These are the permissions:
				- (db_datareader, db_datawriter, and db_owner needed for CREATE TABLE ability)
				- SQL Server on same machine: "IIS APPPOOL\ASP .NET v4.0"
				- SQL Server on different server: "DOMAIN\COMPUTER$"

	This Script creates the Entire DocScript-Interactive DataBase, with all its Tables.
	This is essentially the same as running each of the individual .SQL Files.
	Note: The tables for each ExecutionSession are created by DSI automatically.
	-
	!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
	Unfortunatly, Variables can't be used for the DataBase Name:
	So - before running this file - Ctrl + H {DocScript_DatabaseName_Placeholder} with the actual name.
	!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
	-
	IIS: Remember to grant the IIS AppPool User permission to access the DB:
	https://stackoverflow.com/questions/7698286/login-failed-for-user-iis-apppool-asp-net-v4-0
	-
	...And remember to grant the just-created-Login permissions to CREATE TABLEs:
	Go into the database, security, users, right-click on the user, properties, Membership, check db_owner.
*/








/*
	!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
	CreateDSDB.SQL
	!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
*/

USE [master]
GO

/****** Object:  Database [{DocScript_DatabaseName_Placeholder}]    Script Date: 09/17/2022 20:41:15 ******/
IF  EXISTS (SELECT name FROM sys.databases WHERE name = N'{DocScript_DatabaseName_Placeholder}')
DROP DATABASE [{DocScript_DatabaseName_Placeholder}]
GO

USE [master]
GO

CREATE DATABASE [{DocScript_DatabaseName_Placeholder}]
GO

ALTER DATABASE [{DocScript_DatabaseName_Placeholder}] SET COMPATIBILITY_LEVEL = 100
GO

IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [{DocScript_DatabaseName_Placeholder}].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO

ALTER DATABASE [{DocScript_DatabaseName_Placeholder}] SET ANSI_NULL_DEFAULT OFF 
GO

ALTER DATABASE [{DocScript_DatabaseName_Placeholder}] SET ANSI_NULLS OFF 
GO

ALTER DATABASE [{DocScript_DatabaseName_Placeholder}] SET ANSI_PADDING OFF 
GO

ALTER DATABASE [{DocScript_DatabaseName_Placeholder}] SET ANSI_WARNINGS OFF 
GO

ALTER DATABASE [{DocScript_DatabaseName_Placeholder}] SET ARITHABORT OFF 
GO

ALTER DATABASE [{DocScript_DatabaseName_Placeholder}] SET AUTO_CLOSE OFF 
GO

ALTER DATABASE [{DocScript_DatabaseName_Placeholder}] SET AUTO_CREATE_STATISTICS ON 
GO

ALTER DATABASE [{DocScript_DatabaseName_Placeholder}] SET AUTO_SHRINK OFF 
GO

ALTER DATABASE [{DocScript_DatabaseName_Placeholder}] SET AUTO_UPDATE_STATISTICS ON 
GO

ALTER DATABASE [{DocScript_DatabaseName_Placeholder}] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO

ALTER DATABASE [{DocScript_DatabaseName_Placeholder}] SET CURSOR_DEFAULT  GLOBAL 
GO

ALTER DATABASE [{DocScript_DatabaseName_Placeholder}] SET CONCAT_NULL_YIELDS_NULL OFF 
GO

ALTER DATABASE [{DocScript_DatabaseName_Placeholder}] SET NUMERIC_ROUNDABORT OFF 
GO

ALTER DATABASE [{DocScript_DatabaseName_Placeholder}] SET QUOTED_IDENTIFIER OFF 
GO

ALTER DATABASE [{DocScript_DatabaseName_Placeholder}] SET RECURSIVE_TRIGGERS OFF 
GO

ALTER DATABASE [{DocScript_DatabaseName_Placeholder}] SET  ENABLE_BROKER 
GO

ALTER DATABASE [{DocScript_DatabaseName_Placeholder}] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO

ALTER DATABASE [{DocScript_DatabaseName_Placeholder}] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO

ALTER DATABASE [{DocScript_DatabaseName_Placeholder}] SET TRUSTWORTHY OFF
GO

ALTER DATABASE [{DocScript_DatabaseName_Placeholder}] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO

ALTER DATABASE [{DocScript_DatabaseName_Placeholder}] SET PARAMETERIZATION SIMPLE 
GO

ALTER DATABASE [{DocScript_DatabaseName_Placeholder}] SET READ_COMMITTED_SNAPSHOT OFF 
GO

ALTER DATABASE [{DocScript_DatabaseName_Placeholder}] SET HONOR_BROKER_PRIORITY OFF 
GO

ALTER DATABASE [{DocScript_DatabaseName_Placeholder}] SET  READ_WRITE 
GO

ALTER DATABASE [{DocScript_DatabaseName_Placeholder}] SET RECOVERY SIMPLE 
GO

ALTER DATABASE [{DocScript_DatabaseName_Placeholder}] SET  MULTI_USER 
GO

ALTER DATABASE [{DocScript_DatabaseName_Placeholder}] SET PAGE_VERIFY CHECKSUM
GO

ALTER DATABASE [{DocScript_DatabaseName_Placeholder}] SET DB_CHAINING OFF 
GO







/*
	!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
	CreateUploadedProgramsTable.SQL
	!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
*/

USE [{DocScript_DatabaseName_Placeholder}]
GO

/****** Object:  Table [dbo].[UploadedPrograms]    Script Date: 09/17/2022 20:43:13 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UploadedPrograms]') AND type in (N'U'))
DROP TABLE [dbo].[UploadedPrograms]
GO

USE [{DocScript_DatabaseName_Placeholder}]
GO

/****** Object:  Table [dbo].[UploadedPrograms]    Script Date: 09/17/2022 20:43:13 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[UploadedPrograms](
	[ProgramName] [varchar](100) NOT NULL,
	[TimeUploaded] [datetime] NOT NULL,
	[Source] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_UploadedPrograms] PRIMARY KEY CLUSTERED 
(
	[ProgramName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'DSI Uploaded Programs' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UploadedPrograms'
GO













/*
	!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
	CreateExecutionSessionsTable.SQL
	!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
*/

USE [{DocScript_DatabaseName_Placeholder}]
GO

/****** Object:  Table [dbo].[ExecutionSessions]    Script Date: 09/17/2022 20:42:31 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ExecutionSessions]') AND type in (N'U'))
DROP TABLE [dbo].[ExecutionSessions]
GO

USE [{DocScript_DatabaseName_Placeholder}]
GO

/****** Object:  Table [dbo].[ExecutionSessions]    Script Date: 09/17/2022 20:42:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[ExecutionSessions] (
	[ESID] [varchar](100) NOT NULL,
	[ProgramName] [varchar](100) NOT NULL,
	[TimeStarted] [datetime] NULL,
	[TimeEnded] [datetime] NULL,
	[State] [varchar](100) NOT NULL,
	[ExitReason] [nvarchar](max) NULL,
	CONSTRAINT [PK_ExecutionSessions] PRIMARY KEY CLUSTERED	(
		[ESID] ASC
	) WITH (
		PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON
	) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

SET ANSI_PADDING OFF
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'DSI ExecutionSessions' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ExecutionSessions'
GO