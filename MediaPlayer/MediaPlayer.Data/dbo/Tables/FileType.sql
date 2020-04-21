CREATE TABLE [dbo].[FileType] (
    [Id]            SMALLINT     IDENTITY (1, 1) NOT NULL,
    [Name]          VARCHAR (50) NOT NULL,
    [MediaItemType] SMALLINT     NOT NULL,
    CONSTRAINT [PK_FileType] PRIMARY KEY CLUSTERED ([Id] ASC)
);

