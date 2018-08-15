USE [WORDSMITH]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[flipped_sentences](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[sentence] [text] NOT NULL,
	[created] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_flipped_sentences] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


CREATE NONCLUSTERED INDEX [created_desc] ON [dbo].[flipped_sentences]
(
	[created] DESC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

ALTER TABLE [dbo].[flipped_sentences] ADD  CONSTRAINT [DF_flipped_sentences_created]  DEFAULT (getdate()) FOR [created]
GO


