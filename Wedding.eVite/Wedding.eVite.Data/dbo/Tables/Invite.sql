CREATE TABLE [dbo].[Invite] (
    [Id]                  INT           IDENTITY (1, 1) NOT NULL,
    [EmailAddress]        VARCHAR (50)  NOT NULL,
    [Password]            VARCHAR (256) CONSTRAINT [DF_Invite_Password] DEFAULT ('') NOT NULL,
    [HasChangedPassword]  BIT           CONSTRAINT [DF_Invite_HasChangedPassword] DEFAULT ((0)) NOT NULL,
    [IsAdmin]             BIT           CONSTRAINT [DF_Invite_IsAdmin] DEFAULT ((0)) NOT NULL,
    [IncludesCeremony]    BIT           CONSTRAINT [DF_Invite_IncludedCeremony] DEFAULT ((1)) NOT NULL,
    [ReserveSandholeRoom] BIT           CONSTRAINT [DF_Invite_ReserveSandholeRoom] DEFAULT ((0)) NOT NULL,
    [EmailMessages]       BIT           CONSTRAINT [DF_Invite_EmailMessages] DEFAULT ((1)) NOT NULL,
    [NotifyGiftWebsite]   BIT           NOT NULL,
    CONSTRAINT [PK_Invite] PRIMARY KEY CLUSTERED ([Id] ASC)
);















