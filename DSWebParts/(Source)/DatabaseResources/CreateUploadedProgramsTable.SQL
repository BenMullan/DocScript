USE [DocScript_Test0]
GO

/****** Object:  Table [dbo].[UploadedPrograms]    Script Date: 09/17/2022 20:43:13 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UploadedPrograms]') AND type in (N'U'))
DROP TABLE [dbo].[UploadedPrograms]
GO

USE [DocScript_Test0]
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