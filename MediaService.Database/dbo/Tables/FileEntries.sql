CREATE TABLE [dbo].[FileEntries] (
    [Id]   UNIQUEIDENTIFIER NOT NULL,
    [Size] INT              NOT NULL,
    CONSTRAINT [PK_dbo.FileEntries] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.FileEntries_dbo.ObjectEntries_Id] FOREIGN KEY ([Id]) REFERENCES [dbo].[ObjectEntries] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE
);

GO
CREATE NONCLUSTERED INDEX [IX_Id]
    ON [dbo].[FileEntries]([Id] ASC);

GO
-- =============================================
-- Author:		<Serhii Yakhin>
-- Create date: <12.11.17>
-- Description:	<For delition object entry of this file>
-- =============================================
Create TRIGGER [dbo].[Trigger_FileEntries_Delete]
   ON  [dbo].[FileEntries]
   AFTER DELETE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for trigger here
	DELETE [dbo].[ObjectEntries] 
	WHERE Id IN (SELECT Id FROM deleted);
END
GO
