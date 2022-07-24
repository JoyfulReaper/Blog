CREATE TABLE [dbo].[Post]
(
	[PostId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [AuthorId] NVARCHAR(128) NULL, 
    [Title] NVARCHAR(200) NOT NULL, 
    [Content] NVARCHAR(MAX) NOT NULL, 
    [Ready] BIT NULL DEFAULT 0,
	[Slug] VARCHAR(500),
	[PostImage] VARBINARY(MAX),
	[PostImageContentType] VARCHAR(100),
    [DateCreated] DATETIME2 NOT NULL DEFAULT GETUTCDATE(), 
    [DateUpdated] DATETIME2 NULL, 
)
