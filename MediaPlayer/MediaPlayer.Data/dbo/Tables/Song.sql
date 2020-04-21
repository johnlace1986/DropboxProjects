CREATE TABLE [dbo].[Song] (
    [MediaItemId]    BIGINT       NOT NULL,
    [MediaItemType]  SMALLINT     NOT NULL,
    [Artist]         VARCHAR (50) NOT NULL,
    [Album]          VARCHAR (50) NOT NULL,
    [DiskNumber]     SMALLINT     NOT NULL,
    [NumberOfDisks]  SMALLINT     NOT NULL,
    [TrackNumber]    SMALLINT     NOT NULL,
    [NumberOfTracks] SMALLINT     NOT NULL,
    [Year]           SMALLINT     NOT NULL,
    [iTunesId]       SMALLINT     NOT NULL,
    CONSTRAINT [PK_Song] PRIMARY KEY CLUSTERED ([MediaItemId] ASC, [MediaItemType] ASC),
    CONSTRAINT [FK_Song_MediaItem] FOREIGN KEY ([MediaItemId], [MediaItemType]) REFERENCES [dbo].[MediaItem] ([Id], [Type]) ON DELETE CASCADE ON UPDATE CASCADE
);

