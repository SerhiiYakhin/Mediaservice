CREATE TABLE [dbo].[ObjectEntryAspNetUsers] (
    [ObjectEntry_Id]     UNIQUEIDENTIFIER NOT NULL,
    [AspNetUser_Id] NVARCHAR (128)   NOT NULL,
    CONSTRAINT [PK_dbo.ObjectEntryAspNetUsers] PRIMARY KEY CLUSTERED ([ObjectEntry_Id] ASC, [AspNetUser_Id] ASC),
    CONSTRAINT [FK_dbo.ObjectEntryAspNetUsers_dbo.AspNetUsers_AspNetUser_Id] FOREIGN KEY ([AspNetUser_Id]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_dbo.ObjectEntryAspNetUsers_dbo.ObjectEntries_ObjectEntry_Id] FOREIGN KEY ([ObjectEntry_Id]) REFERENCES [dbo].[ObjectEntries] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_ObjectEntry_Id]
    ON [dbo].[ObjectEntryAspNetUsers]([ObjectEntry_Id] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_AspNetUser_Id]
    ON [dbo].[ObjectEntryAspNetUsers]([AspNetUser_Id] ASC);

