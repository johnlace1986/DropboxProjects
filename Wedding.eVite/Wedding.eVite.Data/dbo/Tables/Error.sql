CREATE TABLE [dbo].[Error] (
    [Id]           INT            IDENTITY (1, 1) NOT NULL,
    [Name]         VARCHAR (500)  NOT NULL,
    [DateThrown]   DATETIME       NOT NULL,
    [OuterErrorId] INT            NULL,
    [Message]      VARCHAR (1000) NOT NULL,
    [StackTrace]   VARCHAR (4000) NOT NULL,
    CONSTRAINT [PK_Error] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Error_Error] FOREIGN KEY ([OuterErrorId]) REFERENCES [dbo].[Error] ([Id])
);





