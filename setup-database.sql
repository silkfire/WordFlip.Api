IF (NOT EXISTS(SELECT 1 FROM sys.databases WHERE name = 'WORDSMITH'))

BEGIN

	CREATE DATABASE [WORDSMITH]
END

GO


USE [WORDSMITH]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


IF (OBJECT_ID(N'dbo.FlippedSentences', N'U') IS NULL)

BEGIN

	CREATE TABLE [dbo].[FlippedSentences](
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[Value] [text] NOT NULL,
		[Created] [datetimeoffset](7) NOT NULL,
	 CONSTRAINT [PK_FlippedSentences] PRIMARY KEY CLUSTERED 
	(
		[id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

	CREATE NONCLUSTERED INDEX [Created_desc] ON [dbo].[FlippedSentences]
	(
		[Created] DESC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

	ALTER TABLE [dbo].[FlippedSentences] ADD  CONSTRAINT [DF_FlippedSentences_Created]  DEFAULT (sysdatetimeoffset()) FOR [Created]

END

GO


