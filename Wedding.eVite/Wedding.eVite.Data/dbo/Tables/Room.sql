CREATE TABLE [dbo].[Room] (
    [Id]   INT          IDENTITY (1, 1) NOT NULL,
    [Name] VARCHAR (50) NOT NULL,
    [Beds] INT          NOT NULL,
    CONSTRAINT [PK_Room] PRIMARY KEY CLUSTERED ([Id] ASC)
);

