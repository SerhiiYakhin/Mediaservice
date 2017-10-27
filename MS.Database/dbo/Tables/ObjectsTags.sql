CREATE TABLE [dbo].[ObjectsTags]
(
	[ObjectId] INT NOT NULL , 
    [TagId] INT NOT NULL
	CONSTRAINT [PK_ObjectsTags] PRIMARY KEY ([ObjectId], [TagId]), 
    CONSTRAINT [FK_ObjectsTags_Objects] FOREIGN KEY ([ObjectId]) REFERENCES [Objects]([ObjectId]), 
    CONSTRAINT [FK_ObjectsTags_Tags] FOREIGN KEY ([TagId]) REFERENCES [Tags]([TagId])
)
