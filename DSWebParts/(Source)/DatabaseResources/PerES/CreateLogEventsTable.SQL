USE [DocScript_Test0]
GO

/****** Object:  Table [dbo].[HELLO_AH42_LogEvents]    Script Date: 09/18/2022 12:04:26 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HELLO_AH42_LogEvents]') AND type in (N'U'))
DROP TABLE [dbo].[HELLO_AH42_LogEvents]
GO

USE [DocScript_Test0]
GO

/****** Object:  Table [dbo].[HELLO_AH42_LogEvents]    Script Date: 09/18/2022 12:04:26 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[HELLO_AH42_LogEvents](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[TimeSubmitted] [datetime] NOT NULL,
	[Message] [nvarchar](max) NOT NULL,
	[Severity] [varchar](100) NOT NULL,
	[Category] [varchar](100) NOT NULL,
 CONSTRAINT [PK_HELLO_AH42_LogEvents] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'DSI ES LogEvents Table Example' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'HELLO_AH42_LogEvents'
GO

