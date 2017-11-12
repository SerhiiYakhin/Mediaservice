CREATE TABLE [dbo].[DirectoryEntries] (
    [Id]        UNIQUEIDENTIFIER NOT NULL,
    [NodeLevel] SMALLINT         NOT NULL,
    CONSTRAINT [PK_dbo.DirectoryEntries] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.DirectoryEntries_dbo.ObjectEntries_Id] FOREIGN KEY ([Id]) REFERENCES [dbo].[ObjectEntries] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE, 
    CONSTRAINT [CK_DirectoryEntries_NodeLevel] CHECK (NodeLevel < 11)
);

GO
CREATE NONCLUSTERED INDEX [IX_Id]
    ON [dbo].[DirectoryEntries]([Id] ASC);

GO
-- =============================================
-- Author:		<Serhii Yakhin>
-- Create date: <12.11.17>
-- Description:	<For cascade delition all objects in deleted folder>
-- =============================================
CREATE TRIGGER [dbo].[Trigger_DirectoryEntries_Delete]
   ON  [dbo].[DirectoryEntries]
   AFTER DELETE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for trigger here
	DELETE [dbo].[ObjectEntries] 
	WHERE Parent_Id IN (SELECT Id FROM deleted) OR Id IN (SELECT Id FROM deleted);
END
GO
