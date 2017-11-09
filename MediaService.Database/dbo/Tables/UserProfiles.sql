CREATE TABLE [dbo].[UserProfiles] (
    [Id]     UNIQUEIDENTIFIER DEFAULT (newsequentialid()) NOT NULL,
    [Avatar] NVARCHAR (60)    NULL,
    CONSTRAINT [PK_dbo.UserProfiles] PRIMARY KEY CLUSTERED ([Id] ASC)
);

