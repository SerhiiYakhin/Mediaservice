CREATE TABLE [dbo].[TagFileEntries] (
    [Tag_Id]       UNIQUEIDENTIFIER NOT NULL,
    [FileEntry_Id] UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_dbo.TagFileEntries] PRIMARY KEY CLUSTERED ([Tag_Id] ASC, [FileEntry_Id] ASC),
    CONSTRAINT [FK_dbo.TagFileEntries_dbo.FileEntries_FileEntry_Id] FOREIGN KEY ([FileEntry_Id]) REFERENCES [dbo].[FileEntries] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_dbo.TagFileEntries_dbo.Tags_Tag_Id] FOREIGN KEY ([Tag_Id]) REFERENCES [dbo].[Tags] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE
);

GO
CREATE NONCLUSTERED INDEX [IX_Tag_Id]
    ON [dbo].[TagFileEntries]([Tag_Id] ASC);

GO
CREATE NONCLUSTERED INDEX [IX_FileEntry_Id]
    ON [dbo].[TagFileEntries]([FileEntry_Id] ASC);