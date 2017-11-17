CREATE TABLE [dbo].[FileEntries] (
    [Id]         UNIQUEIDENTIFIER DEFAULT (newsequentialid()) NOT NULL,
    [Owner_Id]   NVARCHAR (128)   NULL,
    [Name]       NVARCHAR (128)   NOT NULL,
    [Created]    DATETIME2 (7)    NOT NULL,
    [Downloaded] DATETIME2 (7)    NOT NULL,
    [Modified]   DATETIME2 (7)    NOT NULL,
    [Thumbnail]  NVARCHAR (250)   NULL,
    [Parent_Id]  UNIQUEIDENTIFIER NULL,
    [Size]       INT              NOT NULL,

    CONSTRAINT [PK_dbo.FileEntries] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.FileEntries_dbo.AspNetUsers_Owner_Id] FOREIGN KEY ([Owner_Id]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_dbo.FileEntries_dbo.DirectoryEntries_Parent_Id] FOREIGN KEY ([Parent_Id]) REFERENCES [dbo].[DirectoryEntries] ([Id]),
	CONSTRAINT [CK_FileEntries_Parent_Id] CHECK ([Id]<>[Parent_Id])
);

GO
CREATE NONCLUSTERED INDEX [IX_Parent_Id]
    ON [dbo].[FileEntries]([Parent_Id] ASC);

GO
CREATE NONCLUSTERED INDEX [IX_Owner_Id]
    ON [dbo].[FileEntries]([Owner_Id] ASC);

