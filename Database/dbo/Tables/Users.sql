CREATE TABLE [dbo].[Users] (
    [Id]             INT            IDENTITY (1, 1) NOT NULL,
    [Email]          NVARCHAR (250) NOT NULL,
    [EmailNormalize] NVARCHAR (250) NOT NULL,
    [PasswordHsh]    NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED ([Id] ASC)
);

