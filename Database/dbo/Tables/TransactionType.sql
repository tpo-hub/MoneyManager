CREATE TABLE [dbo].[TransactionType] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [Description] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_TransactionType] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_TransactionType_TransactionType] FOREIGN KEY ([Id]) REFERENCES [dbo].[TransactionType] ([Id])
);

