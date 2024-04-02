CREATE TABLE [PostsAPISchema].[Posts] (
    [PostId]      INT            IDENTITY (1, 1) NOT NULL,
    [UserId]      INT            NULL,
    [PostTitle]   NVARCHAR (255) NULL,
    [PostContent] NVARCHAR (MAX) NULL,
    [PostCreated] DATETIME2 (7)  CONSTRAINT [DEFAULT_Posts_PostCreated] DEFAULT GETDATE() NULL,
    [PostUpdated] DATETIME2 (7)  CONSTRAINT [DEFAULT_Posts_PostUpdated] DEFAULT GETDATE() NULL
);


GO
CREATE CLUSTERED INDEX [cix_Posts_UserId_PostId]
    ON [PostsAPISchema].[Posts]([UserId] ASC, [PostId] ASC);

GO

  CREATE TRIGGER UpdatePostUpdated 
  ON PostsAPISchema.Posts
  AFTER UPDATE
  AS
  BEGIN
    UPDATE PostsAPISchema.Posts
    SET PostUpdated = GETDATE()
    FROM PostsAPISchema.Posts AS postsT
    INNER JOIN inserted ON postsT.PostId = inserted.PostId
 END;