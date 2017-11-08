CREATE TABLE [dbo].[ObjectEntryApplicationUsers] (
    [ObjectEntry_Id]     UNIQUEIDENTIFIER NOT NULL,
    [ApplicationUser_Id] NVARCHAR (128)   NOT NULL,
    CONSTRAINT [PK_dbo.ObjectEntryApplicationUsers] PRIMARY KEY CLUSTERED ([ObjectEntry_Id] ASC, [ApplicationUser_Id] ASC),
    CONSTRAINT [FK_dbo.ObjectEntryApplicationUsers_dbo.AspNetUsers_ApplicationUser_Id] FOREIGN KEY ([ApplicationUser_Id]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_dbo.ObjectEntryApplicationUsers_dbo.ObjectEntries_ObjectEntry_Id] FOREIGN KEY ([ObjectEntry_Id]) REFERENCES [dbo].[ObjectEntries] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_ObjectEntry_Id]
    ON [dbo].[ObjectEntryApplicationUsers]([ObjectEntry_Id] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ApplicationUser_Id]
    ON [dbo].[ObjectEntryApplicationUsers]([ApplicationUser_Id] ASC);

