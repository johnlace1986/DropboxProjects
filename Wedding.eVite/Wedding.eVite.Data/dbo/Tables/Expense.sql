CREATE TABLE [dbo].[Expense] (
    [Id]   INT             IDENTITY (1, 1) NOT NULL,
    [Name] VARCHAR (100)   NOT NULL,
    [Cost] DECIMAL (18, 2) NOT NULL,
    [Paid] DECIMAL (18, 2) NOT NULL,
    CONSTRAINT [PK_Expense] PRIMARY KEY CLUSTERED ([Id] ASC)
);

