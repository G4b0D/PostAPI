using DotnetAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DotnetAPI.Util;
using System.Numerics;
using Microsoft.AspNetCore.Authorization;
using DotnetAPI.Dtos;
namespace DotnetAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly PostsService _postsService;
        public PostsController(IConfiguration configuration)
        {
            _postsService = new PostsService(configuration);
        }

        [HttpGet]
        public async Task<IActionResult> GetPosts()
        {
            IEnumerable<Post> posts = await _postsService.GetPosts();
            if (posts.Any())
            {
                return Ok(posts);
            }
            return NotFound("Can't fetch posts");
        }

        [HttpGet("/{postId}")]
        public async Task<IActionResult> GetPost(string postId)
        {
            Post post = await _postsService.GetPostWithId(postId);
            if (post == null) { NotFound("No record found"); }
            return Ok(post);
        }

        [HttpGet("GetPostsFromUser/{userId}")]
        public async Task<IActionResult> GetPostsFromUser(string userId)
        {
            IEnumerable<Post> post = await _postsService.GetPostFromUser(userId);
            if(post.Any())
            {
                return Ok(post);
            }
            return NotFound("Couldn't fetch any post from user");
        }

        [HttpGet("MyPosts")]
        public async Task<IActionResult> GetMyPosts()
        {
            string? userId = User.FindFirst("userId")?.Value;
            IEnumerable<Post> post = await _postsService.GetPostFromUser(userId ?? string.Empty);
            if (post.Any())
            {
                return Ok(post);
            }
            return NotFound("No posts available");
        }

        [HttpPost("CreatePost")]
        public async Task<IActionResult> CreatePost(PostToAdd post) 
        { 
            if (post.PostContent == null) { return BadRequest("Post can't be blank."); }
            string? userId = User.FindFirst("userId")?.Value;
            bool res = await _postsService.CreatePost(post,userId??string.Empty);
            if (res)
            {
                return Ok();
            }
            return BadRequest("Couldn't create post.");
        }

        [HttpDelete("DeletePost/{postId}")]
        public async Task<IActionResult> DeletePost(string postId)
        {
            if(_postsService.GetPostWithId(postId) == null)
            {
                return NotFound("Can't delete post. Not found");
            }
            bool res = await _postsService.DeletePost(postId);
            if (res)
            {
                return Ok();
            }
            return BadRequest("Couldn't delete post.");
        }

        [HttpPatch("Update")]
        public async Task<IActionResult> UpadatePost(PostToEdit post)
        {
            if ( await _postsService.UpdatePost(post))
            {
                return Ok();
            };
            return BadRequest();
        }
    }
}
