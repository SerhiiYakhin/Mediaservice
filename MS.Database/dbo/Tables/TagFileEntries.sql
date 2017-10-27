CREATE TABLE [dbo].[TagFileEntries] (
    [Tag_Id]       UNIQUEIDENTIFIER NOT NULL,
    [FileEntry_Id] UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_dbo.TagFileEntries] PRIMARY KEY CLUSTERED ([Tag_Id] ASC, [FileEntry_Id] ASC),
    CONSTRAINT [FK_dbo.TagFileEntries_dbo.ObjectEntries_FileEntry_Id] FOREIGN KEY ([FileEntry_Id]) REFERENCES [dbo].[ObjectEntries] ([ObjectEntryId]) ON DELETE CASCADE,
    CONSTRAINT [FK_dbo.TagFileEntries_dbo.Tags_Tag_Id] FOREIGN KEY ([Tag_Id]) REFERENCES [dbo].[Tags] ([TagId]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_Tag_Id]
    ON [dbo].[TagFileEntries]([Tag_Id] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_FileEntry_Id]
    ON [dbo].[TagFileEntries]([FileEntry_Id] ASC);

