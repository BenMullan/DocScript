USE [DocScript_Test0]
GO

/****** Object:  Table [dbo].[ExecutionSessions]    Script Date: 09/17/2022 20:42:31 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ExecutionSessions]') AND type in (N'U'))
DROP TABLE [dbo].[ExecutionSessions]
GO

USE [DocScript_Test0]
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