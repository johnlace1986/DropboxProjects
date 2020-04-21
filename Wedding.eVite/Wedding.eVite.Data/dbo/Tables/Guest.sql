CREATE TABLE [dbo].[Guest] (
    [Id]                  INT          IDENTITY (1, 1) NOT NULL,
    [InviteId]            INT          NOT NULL,
    [Forename]            VARCHAR (50) NOT NULL,
    [Surname]             VARCHAR (50) NOT NULL,
    [IsAttending]         BIT          NULL,
    [DateOfRsvp]          DATETIME     NULL,
    [IsChild]             BIT          CONSTRAINT [DF_Guest_IsChild] DEFAULT ((0)) NOT NULL,
    [IsVegetarian]        BIT          CONSTRAINT [DF_Guest_IsVegetarian] DEFAULT ((0)) NOT NULL,
    [CanBringPlusOne]     BIT          CONSTRAINT [DF_Guest_IncludePlusOne] DEFAULT ((0)) NOT NULL,
    [IsBringingPlusOne]   BIT          CONSTRAINT [DF__Guest__IsBringin__14270015] DEFAULT ((0)) NOT NULL,
    [PlusOneForename]     VARCHAR (50) CONSTRAINT [DF__Guest__PlusOneFo__151B244E] DEFAULT ('') NOT NULL,
    [PlusOneSurname]      VARCHAR (50) CONSTRAINT [DF__Guest__PlusOneSu__160F4887] DEFAULT ('') NOT NULL,
    [PlusOneIsVegetarian] BIT          CONSTRAINT [DF_Guest_PlusOneIsVegetarian] DEFAULT ((0)) NOT NULL,
    [TableId]             INT          NULL,
    [RoomId]              INT          NULL,
    [Notes] VARCHAR(500) NOT NULL DEFAULT (''), 
    CONSTRAINT [PK_Guest] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Guest_Invite] FOREIGN KEY ([InviteId]) REFERENCES [dbo].[Invite] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_Guest_Room] FOREIGN KEY ([RoomId]) REFERENCES [dbo].[Room] ([Id]) ON DELETE SET NULL ON UPDATE SET NULL,
    CONSTRAINT [FK_Guest_Table] FOREIGN KEY ([TableId]) REFERENCES [dbo].[Table] ([Id]) ON DELETE SET NULL ON UPDATE SET NULL
);

























