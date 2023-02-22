USE [DocScript_Test0]
GO

/****** Object:  Table [dbo].[HELLO_AH42_CEPs]    Script Date: 09/18/2022 12:07:07 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HELLO_AH42_CEPs]') AND type in (N'U'))
DROP TABLE [dbo].[HELLO_AH42_CEPs]
GO

USE [DocScript_Test0]
GO

/****** Object:  Table [dbo].[HELLO_AH42_CEPs]    Script Date: 09/18/2022 12:07:07 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[HELLO_AH42_CEPs](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[TimeSubmitted] [datetime] NOT NULL,
	[JavaScriptToRun] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_HELLO_AH42_CEPs] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'DSI ES CEPs Example Table' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'HELLO_AH42_CEPs'
GO