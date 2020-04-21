CREATE TABLE [dbo].[MediaItem] (
    [Id]           BIGINT        NOT NULL,
    [Type]         SMALLINT      NOT NULL,
    [Name]         VARCHAR (255) NOT NULL,
    [Genre]        VARCHAR (50)  NOT NULL,
    [IsHidden]     BIT           NOT NULL,
    [DateCreated]  DATETIME      NOT NULL,
    [DateModified] DATETIME      NOT NULL,
    [UserName]     VARCHAR (255) NOT NULL,
    CONSTRAINT [PK_MediaItem_1] PRIMARY KEY CLUSTERED ([Id] ASC, [Type] ASC)
);

