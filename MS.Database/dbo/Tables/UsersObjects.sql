CREATE TABLE [dbo].[UsersObjects]
(
	[UserId] NVARCHAR(128) NOT NULL, 
    [ObjectId] INT NOT NULL, 
    CONSTRAINT [PK_UsersObjects] PRIMARY KEY ([UserId], [ObjectId]), 
    CONSTRAINT [FK_UsersObjects_Objects] FOREIGN KEY ([ObjectId]) REFERENCES [Objects]([ObjectId]), 
    CONSTRAINT [FK_UsersObjects_Users] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers]([Id])
)