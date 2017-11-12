CREATE TABLE [dbo].[ObjectEntries] (
    [Id]         UNIQUEIDENTIFIER DEFAULT (newsequentialid()) NOT NULL,
    [Name]       NVARCHAR (128)   NOT NULL,
    [Created]    DATETIME2 (7)    NOT NULL,
    [Downloaded] DATETIME2 (7)    NOT NULL,
    [Modified]   DATETIME2 (7)    NOT NULL,
    [Thumbnail]  NVARCHAR (250)   NULL,
    [Parent_Id]  UNIQUEIDENTIFIER NULL,
    [Owner_Id]   NVARCHAR (128)   NULL,
    CONSTRAINT [PK_dbo.ObjectEntries] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [CK_ObjectEntries] CHECK ([Id]<>[Parent_Id]),
    CONSTRAINT [FK_dbo.ObjectEntries_dbo.AspNetUsers_Owner_Id] FOREIGN KEY ([Owner_Id]) REFERENCES [dbo].[AspNetUsers] ([Id]),
    CONSTRAINT [FK_dbo.ObjectEntries_dbo.DirectoryEntries_Parent_Id] FOREIGN KEY ([Parent_Id]) REFERENCES [dbo].[DirectoryEntries] ([Id])
);

GO
CREATE NONCLUSTERED INDEX [IX_Parent_Id]
    ON [dbo].[ObjectEntries]([Parent_Id] ASC);

GO
CREATE NONCLUSTERED INDEX [IX_Owner_Id]
    ON [dbo].[ObjectEntries]([Owner_Id] ASC);

GO
-- =============================================
-- Author:		<Serhii Yakhin>
-- Create date: <12.11.17>
-- Description:	<To prevent the addition or creation of folders with an nesting level greater than 10>
-- =============================================
CREATE TRIGGER [dbo].[Trigger_DirectoryEntry]
    ON [dbo].[ObjectEntries]
    FOR INSERT, UPDATE
    AS
    BEGIN
        SET NoCount ON
    END