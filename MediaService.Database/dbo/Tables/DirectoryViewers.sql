CREATE TABLE [dbo].[DirectoryViewers] (
    [DirectoryEntry_Id] UNIQUEIDENTIFIER NOT NULL,
    [User_Id]           NVARCHAR (128)   NOT NULL,
    [Link]              NVARCHAR (250)   NOT NULL,
    CONSTRAINT [PK_dbo.DirectoryViewers] PRIMARY KEY CLUSTERED ([DirectoryEntry_Id] ASC, [User_Id] ASC),
    CONSTRAINT [FK_dbo.DirectoryViewers_dbo.AspNetUsers_User_Id] FOREIGN KEY ([User_Id]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_dbo.DirectoryViewers_dbo.DirectoryEntries_DirectoryEntry_Id] FOREIGN KEY ([DirectoryEntry_Id]) REFERENCES [dbo].[DirectoryEntries] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_User_Id]
    ON [dbo].[DirectoryViewers]([User_Id] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_DirectoryEntry_Id]
    ON [dbo].[DirectoryViewers]([DirectoryEntry_Id] ASC);

