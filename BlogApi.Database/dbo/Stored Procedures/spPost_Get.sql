CREATE PROCEDURE [dbo].[spPost_Get]
	@PostId INT
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT
		[PostId]
		,[AuthorId]
		,[Title]
		,[Abstract]
		,[Content]
		,[Ready]
		,[Slug]
		,[PostImage]
		,[PostImageContentType]
		,[DateCreated]
		,[DateUpdated]
	FROM
		[dbo].[Post]
	WHERE
		[PostId] = @PostId
END