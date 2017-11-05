CREATE TABLE [dbo].[UserProfiles] (
    [Id]       NVARCHAR (128) NOT NULL,
    [Nickname] NVARCHAR (128) NOT NULL,
    [Avatar]   NVARCHAR (60)  NULL,
    CONSTRAINT [PK_dbo.UserProfiles] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.UserProfiles_dbo.AspNetUsers_Id] FOREIGN KEY ([Id]) REFERENCES [dbo].[AspNetUsers] ([Id]),
    CONSTRAINT AK_Nickname UNIQUE([Nickname]) 
);


GO
CREATE NONCLUSTERED INDEX [IX_Id]
    ON [dbo].[UserProfiles]([Id] ASC);

