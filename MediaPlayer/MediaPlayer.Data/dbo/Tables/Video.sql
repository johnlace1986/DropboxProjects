CREATE TABLE [dbo].[Video] (
    [MediaItemId]      BIGINT       NOT NULL,
    [MediaItemType]    SMALLINT     NOT NULL,
    [Program]          VARCHAR (50) NOT NULL,
    [Series]           SMALLINT     NOT NULL,
    [Episode]          SMALLINT     NOT NULL,
    [NumberOfEpisodes] SMALLINT     NOT NULL,
    CONSTRAINT [PK_Video] PRIMARY KEY CLUSTERED ([MediaItemId] ASC, [MediaItemType] ASC),
    CONSTRAINT [FK_Video_MediaItem] FOREIGN KEY ([MediaItemId], [MediaItemType]) REFERENCES [dbo].[MediaItem] ([Id], [Type]) ON DELETE CASCADE ON UPDATE CASCADE
);

