CREATE TABLE [dbo].[Objects]
(
	[ObjectId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [ParentId] INT NOT NULL, 
    [Name] NVARCHAR(50) NOT NULL, 
    [Discriminator] NVARCHAR(20) NOT NULL, 
    [Size] BIGINT NOT NULL, 
    [NodeLevel] SMALLINT NOT NULL, 
    [Created] DATETIME2 NOT NULL, 
    [Downloaded] DATETIME2 NOT NULL, 
    [Modified] DATETIME2 NOT NULL, 
    [Thumbnail] NCHAR(50) NULL
)