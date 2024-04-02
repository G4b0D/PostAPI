CREATE TABLE [PostsAPISchema].[Auth](
	[Email] [nvarchar](100) NULL,
	[PasswordHash] [varbinary](max) NULL,
	[PasswordSalt] [varbinary](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]