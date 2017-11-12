CREATE TABLE [dbo].[ObjectViewers] (
    [ObjectEntryId] UNIQUEIDENTIFIER NOT NULL,
    [UserId]        NVARCHAR (128)   NOT NULL,
    [Link]          NVARCHAR (250)   NOT NULL,
    CONSTRAINT [PK_dbo.ObjectViewers] PRIMARY KEY CLUSTERED ([ObjectEntryId] ASC, [UserId] ASC),
    CONSTRAINT [FK_dbo.ObjectViewers_dbo.AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_dbo.ObjectViewers_dbo.ObjectEntries_ObjectEntryId] FOREIGN KEY ([ObjectEntryId]) REFERENCES [dbo].[ObjectEntries] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE
);

GO
CREATE NONCLUSTERED INDEX [IX_UserId]
    ON [dbo].[ObjectViewers]([UserId] ASC);

GO
CREATE NONCLUSTERED INDEX [IX_ObjectEntryId]
    ON [dbo].[ObjectViewers]([ObjectEntryId] ASC);

