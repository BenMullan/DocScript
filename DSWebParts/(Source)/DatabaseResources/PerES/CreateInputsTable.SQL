USE [DocScript_Test0]
GO

/****** Object:  Table [dbo].[HELLO_AH42_Inputs]    Script Date: 09/18/2022 12:01:43 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HELLO_AH42_Inputs]') AND type in (N'U'))
DROP TABLE [dbo].[HELLO_AH42_Inputs]
GO

USE [DocScript_Test0]
GO

/****** Object:  Table [dbo].[HELLO_AH42_Inputs]    Script Date: 09/18/2022 12:01:44 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[HELLO_AH42_Inputs](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[TimeSubmitted] [datetime] NOT NULL,
	[InputPrompt] [nvarchar](max) NOT NULL,
	[InputResponse] [nvarchar](max) NULL,
	[RespondedToTime] [datetime] NULL,
 CONSTRAINT [PK_HELLO_AH42_Inputs] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'DSI ES Inputs Example Table' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'HELLO_AH42_Inputs'
GO