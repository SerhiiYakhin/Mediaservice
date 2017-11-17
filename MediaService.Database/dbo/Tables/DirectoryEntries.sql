CREATE TABLE [dbo].[DirectoryEntries] (
    [Id]         UNIQUEIDENTIFIER DEFAULT (newsequentialid()) NOT NULL,
    [Owner_Id]   NVARCHAR (128)   NULL,
    [Name]       NVARCHAR (128)   NOT NULL,
    [Created]    DATETIME2 (7)    NOT NULL,
    [Downloaded] DATETIME2 (7)    NOT NULL,
    [Modified]   DATETIME2 (7)    NOT NULL,
    [Thumbnail]  NVARCHAR (250)   NULL,
    [Parent_Id]  UNIQUEIDENTIFIER NULL,
    [NodeLevel]  SMALLINT         NOT NULL,

    CONSTRAINT [PK_dbo.DirectoryEntries] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.DirectoryEntries_dbo.AspNetUsers_Owner_Id] FOREIGN KEY ([Owner_Id]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_dbo.DirectoryEntries_dbo.DirectoryEntries_Parent_Id] FOREIGN KEY ([Parent_Id]) REFERENCES [dbo].[DirectoryEntries] ([Id]), 
    CONSTRAINT [CK_DirectoryEntries_NodeLevel] CHECK (NodeLevel < 11),
	CONSTRAINT [CK_DirectoryEntries_Parent_Id] CHECK ([Id]<>[Parent_Id])
);

GO
CREATE NONCLUSTERED INDEX [IX_Parent_Id]
    ON [dbo].[DirectoryEntries]([Parent_Id] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Owner_Id]
    ON [dbo].[DirectoryEntries]([Owner_Id] ASC);

GO
-- =============================================
-- Author:		<Serhii Yakhin>
-- Create date: <12.11.17>
-- Description:	<For cascade delition all objects in deleted folder>
-- =============================================
CREATE TRIGGER [dbo].[Trigger_DirectoryEntries_Delete]
    ON [dbo].[DirectoryEntries]
    FOR DELETE
    AS
    BEGIN
		SET NoCount ON
		DELETE [dbo].[DirectoryEntries] 
		WHERE Parent_Id IN (SELECT Id FROM deleted);
		DELETE [dbo].[FileEntries] 
		WHERE Parent_Id IN (SELECT Id FROM deleted);
    END