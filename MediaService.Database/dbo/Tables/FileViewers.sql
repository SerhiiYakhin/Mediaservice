CREATE TABLE [dbo].[FileViewers] (
    [FileEntry_Id] UNIQUEIDENTIFIER NOT NULL,
    [User_Id]      NVARCHAR (128)   NOT NULL,
    [Link]         NVARCHAR (250)   NOT NULL,
    CONSTRAINT [PK_dbo.FileViewers] PRIMARY KEY CLUSTERED ([FileEntry_Id] ASC, [User_Id] ASC),
    CONSTRAINT [FK_dbo.FileViewers_dbo.AspNetUsers_User_Id] FOREIGN KEY ([User_Id]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_dbo.FileViewers_dbo.FileEntries_FileEntry_Id] FOREIGN KEY ([FileEntry_Id]) REFERENCES [dbo].[FileEntries] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_User_Id]
    ON [dbo].[FileViewers]([User_Id] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_FileEntry_Id]
    ON [dbo].[FileViewers]([FileEntry_Id] ASC);

