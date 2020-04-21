CREATE TABLE [dbo].[MediaItemTag] (
    [MediaItemId]   BIGINT       NOT NULL,
    [MediaItemType] SMALLINT     NOT NULL,
    [Tag]           VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_MediaItemTag] PRIMARY KEY CLUSTERED ([MediaItemId] ASC, [MediaItemType] ASC, [Tag] ASC),
    CONSTRAINT [FK_MediaItemTag_MediaItem] FOREIGN KEY ([MediaItemId], [MediaItemType]) REFERENCES [dbo].[MediaItem] ([Id], [Type]) ON DELETE CASCADE ON UPDATE CASCADE
);

