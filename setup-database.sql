CREATE DATABASE [WORDSMITH]
GO

USE [WORDSMITH]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[flipped_sentences](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[sentence] [text] NOT NULL,
	[created] [datetime2] NOT NULL,
 CONSTRAINT [PK_flipped_sentences] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[flipped_sentences] ADD  CONSTRAINT [DF_flipped_sentences_created]  DEFAULT (getdate()) FOR [created]
GO


