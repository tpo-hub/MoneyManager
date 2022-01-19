CREATE TABLE [dbo].[Counts] (
    [Id]          INT             IDENTITY (1, 1) NOT NULL,
    [Name]        NVARCHAR (50)   NOT NULL,
    [CountTypeId] INT             NOT NULL,
    [Balance]     DECIMAL (18, 2) NOT NULL,
    [Description] NCHAR (1000)    NULL,
    [Condition]   BIT             NOT NULL,
    CONSTRAINT [PK_Counts] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Counts_CountsType] FOREIGN KEY ([CountTypeId]) REFERENCES [dbo].[CountsType] ([Id])
);

