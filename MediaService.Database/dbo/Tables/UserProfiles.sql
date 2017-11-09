CREATE TABLE [dbo].[UserProfiles] (
    [Id]       NVARCHAR (128) NOT NULL,
    [Nickname] NVARCHAR (128) NOT NULL,
    [Avatar]   NVARCHAR (60)  NULL,
    [UserName] NVARCHAR (256) NULL,
    CONSTRAINT [PK_dbo.UserProfiles] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_UserProfiles_AspNetUsers] FOREIGN KEY ([UserName]) REFERENCES [dbo].[AspNetUsers] ([UserName]),
    CONSTRAINT [AK_Nickname] UNIQUE NONCLUSTERED ([Nickname] ASC),
    CONSTRAINT [IX_UserProfiles] UNIQUE NONCLUSTERED ([UserName] ASC)
);




GO
CREATE NONCLUSTERED INDEX [IX_Id]
    ON [dbo].[UserProfiles]([Id] ASC);

