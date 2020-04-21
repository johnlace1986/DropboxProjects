CREATE TABLE [dbo].[RootFolderTag] (
    [RootFolderPriority] SMALLINT     NOT NULL,
    [MediaItemType]      SMALLINT     NOT NULL,
    [Tag]                VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_RootFolderTag] PRIMARY KEY CLUSTERED ([RootFolderPriority] ASC, [MediaItemType] ASC, [Tag] ASC),
    CONSTRAINT [FK_RootFolderTag_RootFolderTag] FOREIGN KEY ([RootFolderPriority], [MediaItemType]) REFERENCES [dbo].[RootFolder] ([Priority], [MediaItemType]) ON DELETE CASCADE ON UPDATE CASCADE
);

