CREATE TABLE [dbo].[Transaction] (
    [Id]                INT             IDENTITY (1, 1) NOT NULL,
    [UserId]            INT             NOT NULL,
    [DateTransaction]   DATETIME        NOT NULL,
    [Mount]             DECIMAL (18, 2) NOT NULL,
    [TransactionTypeId] INT             NOT NULL,
    [Note]              NCHAR (1000)    NULL,
    [CountId]           INT             NOT NULL,
    [CategoryId]        INT             NOT NULL,
    [Condition]         BIT             NOT NULL,
    CONSTRAINT [PK_Transaction] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Transaction_Categories] FOREIGN KEY ([CategoryId]) REFERENCES [dbo].[Categories] ([Id]),
    CONSTRAINT [FK_Transaction_Counts] FOREIGN KEY ([CountId]) REFERENCES [dbo].[Counts] ([Id]),
    CONSTRAINT [FK_Transaction_TransactionType] FOREIGN KEY ([TransactionTypeId]) REFERENCES [dbo].[TransactionType] ([Id]),
    CONSTRAINT [FK_Transaction_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([Id])
);

