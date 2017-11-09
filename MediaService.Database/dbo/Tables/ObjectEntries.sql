CREATE TABLE [dbo].[ObjectEntries] (
    [Id]            UNIQUEIDENTIFIER DEFAULT (newsequentialid()) NOT NULL,
    [ParentId]      UNIQUEIDENTIFIER NULL,
    [Name]          NVARCHAR (50)    NOT NULL,
    [Size]          BIGINT           NOT NULL,
    [Created]       DATETIME2 (7)    NOT NULL,
    [Downloaded]    DATETIME2 (7)    NOT NULL,
    [Modified]      DATETIME2 (7)    NOT NULL,
    [Thumbnail]     NVARCHAR (250)    NULL,
    [NodeLevel]     SMALLINT         NULL,
    [Discriminator] NVARCHAR (128)   NOT NULL,
    CONSTRAINT [PK_dbo.ObjectEntries] PRIMARY KEY CLUSTERED ([Id] ASC)
);

