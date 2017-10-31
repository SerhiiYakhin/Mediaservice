CREATE TABLE [dbo].[ObjectEntryUserProfiles] (
    [ObjectEntry_Id] UNIQUEIDENTIFIER NOT NULL,
    [UserProfile_Id] NVARCHAR (128)   NOT NULL,
    CONSTRAINT [PK_dbo.ObjectEntryUserProfiles] PRIMARY KEY CLUSTERED ([ObjectEntry_Id] ASC, [UserProfile_Id] ASC),
    CONSTRAINT [FK_dbo.ObjectEntryUserProfiles_dbo.ObjectEntries_ObjectEntry_Id] FOREIGN KEY ([ObjectEntry_Id]) REFERENCES [dbo].[ObjectEntries] ([ObjectEntryId]) ON DELETE CASCADE,
    CONSTRAINT [FK_dbo.ObjectEntryUserProfiles_dbo.UserProfiles_UserProfile_Id] FOREIGN KEY ([UserProfile_Id]) REFERENCES [dbo].[UserProfiles] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_ObjectEntry_Id]
    ON [dbo].[ObjectEntryUserProfiles]([ObjectEntry_Id] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_UserProfile_Id]
    ON [dbo].[ObjectEntryUserProfiles]([UserProfile_Id] ASC);

