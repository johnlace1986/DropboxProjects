CREATE TABLE [dbo].[FileTypeExtension] (
    [FileTypeId] SMALLINT     NOT NULL,
    [Extension]  VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_FileTypeExtension] PRIMARY KEY CLUSTERED ([FileTypeId] ASC, [Extension] ASC),
    CONSTRAINT [FK_FileTypeExtension_FileType] FOREIGN KEY ([FileTypeId]) REFERENCES [dbo].[FileType] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE
);

