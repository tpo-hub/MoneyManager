CREATE TABLE [dbo].[CountsType] (
    [Id]        INT           IDENTITY (1, 1) NOT NULL,
    [Name]      NVARCHAR (50) NOT NULL,
    [UserId]    INT           NOT NULL,
    [OrderType] INT           NOT NULL,
    CONSTRAINT [PK_CountsType] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_CountsType_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([Id])
);

