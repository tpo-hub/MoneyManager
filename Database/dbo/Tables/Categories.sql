CREATE TABLE [dbo].[Categories] (
    [Id]                INT           IDENTITY (1, 1) NOT NULL,
    [Name]              NVARCHAR (50) NOT NULL,
    [TransactionTypeId] INT           NOT NULL,
    [UserId]            INT           NOT NULL,
    CONSTRAINT [PK_categories] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_categories_TransactionType] FOREIGN KEY ([TransactionTypeId]) REFERENCES [dbo].[TransactionType] ([Id]),
    CONSTRAINT [FK_categories_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([Id])
);

