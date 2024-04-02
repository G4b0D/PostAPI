using DotnetAPI.Models;

namespace DotnetAPI
{
    public class Queries
    {
        #region User
        public static readonly string GetAllUsersQuery = @"SELECT [UserId]
                                                                 ,[FirstName]
                                                                 ,[LastName]
                                                                 ,[Email]
                                                                 ,[Gender]
                                                                 ,[Active]
                                                           FROM [DotNetCourseDatabase].[PostsAPISchema].[Users]";
        public static readonly string CreateUserQuery = @"INSERT INTO PostsAPISchema.Users
                                                            (
                                                                [FirstName],
                                                                [LastName],
                                                                [Email],
                                                                [Gender],
                                                                [Active]
                                                            ) VALUES(@FirstName,@LastName,@Email,@Gender,1)";

        public static readonly string GetSingleUserQuery = @"SELECT TOP (1) [UserId]
                                                    ,[FirstName]
                                                    ,[LastName]
                                                    ,[Email]
                                                    ,[Gender]
                                                    ,[Active]
                                                FROM [PostsAPISchema].[Users]
                                                WHERE UserId = @UserId";

        public static readonly string UpdateUserQuery = @"Update PostsAPISchema.Users
                                                 SET [Email] = @Email
                                                    ,[Gender] = @Gender
                                                    ,[Active] = @Active
                                                 WHERE UserId = @UserId";

        public static readonly string DeleteUserQuery = @"DELETE FROM PostsAPISchema.Users 
                                                          WHERE UserId = @UserId";
        #endregion

        #region UserAuth
        public static readonly string GetUserIdQuery = @"SELECT [UserId]
                                           FROM [PostsAPISchema].[Users]
                                           WHERE Email = @Email";

        public static readonly string ValidateUserIdQuery = @"SELECT [UserId]
                                                FROM [PostsAPISchema].[Users]
                                                WHERE UserId = @UserId";

        public static readonly string UserExistsQuery = @"SELECT Email
                                                 FROM PostsAPISchema.Auth 
                                                 WHERE Email = @Email";

        public static readonly string RegisterAuthUserQuery = @"INSERT INTO [PostsAPISchema].[Auth]([Email],[PasswordHash],[PasswordSalt]) 
                                                                VALUES (@Email,@Passwordhash,@PasswordSalt)";

        public static readonly string GetSaltandHashQuery = @"SELECT [PasswordHash],[PasswordSalt] 
                                                              FROM PostsAPISchema.Auth WHERE Email =@Email";
        #endregion

        #region Posts
        public static readonly string GetAllPosts = @"SELECT [PostId]
                                                            ,[UserId]
                                                            ,[PostTitle]
                                                            ,[PostContent]
                                                            ,[PostCreated]
                                                            ,[PostUpdated]
                                                      FROM PostsAPISchema.Posts";

        public static readonly string GetPostById = @"SELECT [PostId]
                                                            ,[UserId]
                                                            ,[PostTitle]
                                                            ,[PostContent]
                                                            ,[PostCreated]
                                                            ,[PostUpdated]
                                                      FROM PostsAPISchema.Posts
                                                      WHERE [PostId] = @PostId";

        public static readonly string GetPostsByUserId = @"SELECT [PostId]
                                                                 ,[UserId]
                                                                 ,[PostTitle]
                                                                 ,[PostContent]
                                                                 ,[PostCreated]
                                                                 ,[PostUpdated]
                                                           FROM PostsAPISchema.Posts
                                                           WHERE [UserId] = @UserId";

        public static readonly string CreatePost = @"INSERT INTO [PostsAPISchema].[Posts]
                                                     (
		                                                [UserId]
		                                                ,[PostTitle]
		                                                ,[PostContent]
                                                      )
                                                      VALUES(@UserId,@PostTitle,@PostContent)";

        public static readonly string DeletePost = @"DELETE FROM [PostsAPISchema].[Posts]
                                                     WHERE [PostId] = @PostId";

        public static readonly string UpdatePost = @"UPDATE [PostsAPISchema].[Posts] 
                                                     SET
		 
		                                                [PostTitle] = @PostTitle
		                                                ,[PostContent] = @PostContent
		
                                                     WHERE [PostId] = @PostId";
        #endregion
    }

}
