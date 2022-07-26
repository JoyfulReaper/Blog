CREATE PROCEDURE [dbo].[spPost_GetAll]
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT
		[PostId],
		[AuthorId],
		[Title],
		[Abstract],
		[Content],
		[Ready], 
		[Slug],
		[PostImage],
		[PostImageContentType],
		[DateCreated], 
		[DateUpdated]
	FROM
		dbo.[Post];
END