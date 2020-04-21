CREATE TABLE [dbo].[Message] (
    [Id]       INT            IDENTITY (1, 1) NOT NULL,
    [InviteId] INT            NOT NULL,
    [Body]     VARCHAR (1000) NOT NULL,
    [DateSent] DATETIME       NOT NULL,
    [Sender]   INT            NOT NULL,
    [Read]     BIT            NOT NULL,
    CONSTRAINT [PK_Message] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Message_Invite] FOREIGN KEY ([InviteId]) REFERENCES [dbo].[Invite] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE
);

