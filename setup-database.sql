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


IF (OBJECT_ID(N'dbo.flipped_sentences', N'U') IS NULL)

BEGIN

CREATE TABLE [dbo].[flipped_sentences](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[sentence] [text] NOT NULL,
	[created] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_flipped_sentences] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]


CREATE NONCLUSTERED INDEX [created_desc] ON [dbo].[flipped_sentences]
(
	[created] DESC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

ALTER TABLE [dbo].[flipped_sentences] ADD  CONSTRAINT [DF_flipped_sentences_created]  DEFAULT (getdate()) FOR [created]

END
GO
