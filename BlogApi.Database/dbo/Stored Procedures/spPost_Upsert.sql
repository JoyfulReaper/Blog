CREATE PROCEDURE [dbo].[spPost_Upsert]
	@PostId INT,
	@AuthorId NVARCHAR(128),
	@Title NVARCHAR(200),
	@Abstract NVARCHAR(200),
	@Content NVARCHAR(MAX),
	@Ready BIT,
	@Slug VARCHAR(500),
	@PostImage VARBINARY(MAX),
	@PostImageContentType VARCHAR(100)
AS
BEGIN
	BEGIN TRANSACTION;
 
	UPDATE dbo.Post WITH (UPDLOCK, SERIALIZABLE) 
	SET
		AuthorId = @AuthorId,
		Title = @Title,
		Abstract = @Abstract,
		Content = @Content,
		Ready = @Ready,
		Slug = @Slug,
		PostImage = @PostImage,
		PostImageContentType = @PostImageContentType,
		DateUpdated = GETUTCDATE()
	WHERE 
		PostId = @PostId;
 
	IF @@ROWCOUNT = 0
	BEGIN
	  INSERT dbo.Post
		(AuthorId,
		Title,
		Abstract,
		Content,
		Ready)
	  VALUES
		(@AuthorId,
		@Title,
		@Abstract,
		@Content,
		@Ready);
		
		SET @PostId = SCOPE_IDENTITY();
	END
 
	COMMIT TRANSACTION;

	SELECT @PostId;
END