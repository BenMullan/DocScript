/*
	DSI enables the execution of DocScript Programs in Real-Time, Multi-Client, Execution-Sessions.

	-----------------------------------------
	DocScript-Interactive Setup Instructions:
	-----------------------------------------
		1) Install MS-SQL-Server (ideally as the Default-Instance)
		2) Run _CreateEntireDB.SQL (this file) to create a DB called "DSInteractive"
		3) Specify the Database-Name and SQL-Server in \DSWebParts\API\DSInteractive.config
		4) Either:
				Ctrl+F5 the WebParts Project in VS,
		   Or...
				Host the DSWebParts\ directory with IIS, and give the IIS AppPool Database Permissions
				(see [Allow IIS to access the Database] below...)
				
	
	-----------------------------------------
	Misc Notes for DSI Setup:
	-----------------------------------------
		This Script creates the Entire DocScript-Interactive DataBase, with all its Tables.
		This is affectively the same as running each of the individual .SQL Files.
		Note: The tables for each ExecutionSession are created by DSI automatically.
		-
		Unfortunatly, Variables can't be used for the DataBase Name, so to call
		the Database something other than "DSInteractive", Ctrl + H to replace.
	

	-----------------------------------------
	Allow IIS to access the Database:
	-----------------------------------------
		If using IIS: Grant the IIS AppPool User permission to access the DB...
		https://stackoverflow.com/questions/7698286/login-failed-for-user-iis-apppool-asp-net-v4-0
		-
		...Then grant the just-created Login these permissions to CREATE TABLEs:
		Go into the database, security, users, right-click on the user, properties, Membership, check db_owner.
		These are the permissions:
			- (db_datareader, db_datawriter, and db_owner needed for CREATE TABLE ability)
			- SQL Server on same machine: "IIS APPPOOL\ASP .NET v4.0"
			- SQL Server on different server: "DOMAIN\COMPUTER$"
*/








/*
	!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
	CreateDSDB.SQL
	!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
*/

USE [master]
GO

/****** Object:  Database [DSInteractive]    Script Date: 09/17/2022 20:41:15 ******/
IF  EXISTS (SELECT name FROM sys.databases WHERE name = N'DSInteractive')
DROP DATABASE [DSInteractive]
GO

USE [master]
GO

CREATE DATABASE [DSInteractive]
GO

ALTER DATABASE [DSInteractive] SET COMPATIBILITY_LEVEL = 100
GO

IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [DSInteractive].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO

ALTER DATABASE [DSInteractive] SET ANSI_NULL_DEFAULT OFF 
GO

ALTER DATABASE [DSInteractive] SET ANSI_NULLS OFF 
GO

ALTER DATABASE [DSInteractive] SET ANSI_PADDING OFF 
GO

ALTER DATABASE [DSInteractive] SET ANSI_WARNINGS OFF 
GO

ALTER DATABASE [DSInteractive] SET ARITHABORT OFF 
GO

ALTER DATABASE [DSInteractive] SET AUTO_CLOSE OFF 
GO

ALTER DATABASE [DSInteractive] SET AUTO_CREATE_STATISTICS ON 
GO

ALTER DATABASE [DSInteractive] SET AUTO_SHRINK OFF 
GO

ALTER DATABASE [DSInteractive] SET AUTO_UPDATE_STATISTICS ON 
GO

ALTER DATABASE [DSInteractive] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO

ALTER DATABASE [DSInteractive] SET CURSOR_DEFAULT  GLOBAL 
GO

ALTER DATABASE [DSInteractive] SET CONCAT_NULL_YIELDS_NULL OFF 
GO

ALTER DATABASE [DSInteractive] SET NUMERIC_ROUNDABORT OFF 
GO

ALTER DATABASE [DSInteractive] SET QUOTED_IDENTIFIER OFF 
GO

ALTER DATABASE [DSInteractive] SET RECURSIVE_TRIGGERS OFF 
GO

ALTER DATABASE [DSInteractive] SET  ENABLE_BROKER 
GO

ALTER DATABASE [DSInteractive] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO

ALTER DATABASE [DSInteractive] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO

ALTER DATABASE [DSInteractive] SET TRUSTWORTHY OFF
GO

ALTER DATABASE [DSInteractive] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO

ALTER DATABASE [DSInteractive] SET PARAMETERIZATION SIMPLE 
GO

ALTER DATABASE [DSInteractive] SET READ_COMMITTED_SNAPSHOT OFF 
GO

ALTER DATABASE [DSInteractive] SET HONOR_BROKER_PRIORITY OFF 
GO

ALTER DATABASE [DSInteractive] SET  READ_WRITE 
GO

ALTER DATABASE [DSInteractive] SET RECOVERY SIMPLE 
GO

ALTER DATABASE [DSInteractive] SET  MULTI_USER 
GO

ALTER DATABASE [DSInteractive] SET PAGE_VERIFY CHECKSUM
GO

ALTER DATABASE [DSInteractive] SET DB_CHAINING OFF 
GO







/*
	!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
	CreateUploadedProgramsTable.SQL
	!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
*/

USE [DSInteractive]
GO

/****** Object:  Table [dbo].[UploadedPrograms]    Script Date: 09/17/2022 20:43:13 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UploadedPrograms]') AND type in (N'U'))
DROP TABLE [dbo].[UploadedPrograms]
GO

USE [DSInteractive]
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

USE [DSInteractive]
GO

/****** Object:  Table [dbo].[ExecutionSessions]    Script Date: 09/17/2022 20:42:31 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ExecutionSessions]') AND type in (N'U'))
DROP TABLE [dbo].[ExecutionSessions]
GO

USE [DSInteractive]
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