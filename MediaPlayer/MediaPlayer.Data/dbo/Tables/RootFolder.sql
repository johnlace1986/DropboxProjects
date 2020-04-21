CREATE TABLE [dbo].[RootFolder] (
    [Priority]      SMALLINT      NOT NULL,
    [MediaItemType] SMALLINT      NOT NULL,
    [Path]          VARCHAR (255) NOT NULL,
    CONSTRAINT [PK_RootFolder_1] PRIMARY KEY CLUSTERED ([Priority] ASC, [MediaItemType] ASC)
);

