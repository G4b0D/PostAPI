using DotnetAPI.Data;
using DotnetAPI.Dtos;
using DotnetAPI.Models;
using Microsoft.Data.SqlClient;

namespace DotnetAPI.Util
{
    public class PostsService
    {
        private readonly DataContextDapper _dapper;

        public PostsService(IConfiguration config)
        {
            _dapper = new DataContextDapper(config);
        }

        public async Task<IEnumerable<Post>> GetPosts()
        {
            string query = Queries.GetAllPosts;

            IEnumerable<Post> posts = await _dapper.LoadData<Post>(query);

            return posts;
        }

        public async Task<Post> GetPostWithId(string postId)
        {
            string query = Queries.GetPostById;
            List<SqlParameter> parameters = [new SqlParameter("@PostId", postId)];
            Post post = await _dapper.LoadDataSingle<Post>(query, parameters);
            return post;
        }

        public async Task<IEnumerable<Post>> GetPostFromUser(string userId)
        {
            string query = Queries.GetPostsByUserId;
            List<SqlParameter> parameters = [new SqlParameter("@UserId", userId)];
            IEnumerable<Post> posts = await _dapper.LoadData<Post>(query, parameters);
            return posts;
        }

        public async Task<bool> CreatePost(PostToAdd postToAdd,string userId)
        {
            string query = Queries.CreatePost;
            List<SqlParameter> parameters =
                [
                    new SqlParameter("@UserId",userId),
                    new SqlParameter("@PostTitle",postToAdd.PostTitle),
                    new SqlParameter("@PostContent",postToAdd.PostContent)
                ];
            return await _dapper.ExecuteQuery(query, parameters);
        }

        public async Task<bool> DeletePost(string postId)
        {
            string query = Queries.DeletePost;
            List<SqlParameter> parameters = [new SqlParameter("@PostId",postId)];
            return await _dapper.ExecuteQuery(query, parameters);
        }
        
        public async Task<bool> UpdatePost(PostToEdit post)
        {
            Post originalPost = await GetPostWithId(post.PostId.ToString());
            if(originalPost == null) 
            {
                return false;
            }
         
            string query = Queries.UpdatePost;
            List<SqlParameter> parameters = 
                [ 
                    new SqlParameter("@PostTitle",post.PostTitle ?? originalPost.PostTitle),
                    new SqlParameter("@PostContent",post.PostContent ?? originalPost.PostContent),
                    new SqlParameter("@PostId",post.PostId)
                ];
            return await _dapper.ExecuteQuery(query , parameters);
        }
    }
}
