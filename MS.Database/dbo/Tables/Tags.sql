CREATE TABLE [dbo].[Tags] (
    [TagId] UNIQUEIDENTIFIER DEFAULT (newsequentialid()) NOT NULL,
    [Name]  NVARCHAR (200)   NOT NULL,
    CONSTRAINT [PK_dbo.Tags] PRIMARY KEY CLUSTERED ([TagId] ASC)
);

