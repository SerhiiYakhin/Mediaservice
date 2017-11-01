CREATE TABLE [dbo].[ObjectEntries] (
    [ObjectEntryId] UNIQUEIDENTIFIER DEFAULT (newsequentialid()) NOT NULL,
    [ParentId]      UNIQUEIDENTIFIER              NULL,
    [Name]          NVARCHAR (50)    NOT NULL,
    [Discriminator] NVARCHAR (15)    NOT NULL,
    [Size]          BIGINT           NOT NULL,
    [Created]       DATETIME2 (7)    NOT NULL,
    [Downloaded]    DATETIME2 (7)    NOT NULL,
    [Modified]      DATETIME2 (7)    NOT NULL,
    [Thumbnail]     NVARCHAR (50)    NULL,
    [NodeLevel]     SMALLINT         NOT NULL,
    CONSTRAINT [PK_dbo.ObjectEntries] PRIMARY KEY CLUSTERED ([ObjectEntryId] ASC)
);

