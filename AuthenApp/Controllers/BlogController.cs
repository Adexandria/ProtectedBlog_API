using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthenApp.BlogModel;
using AuthenApp.BlogServices;
using AuthenApp.BlogView;
using AuthenApp.UserModel;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AuthenApp.Controllers
{
    [ApiController]
    [Route("api/blog/{username}")]
    [Authorize]
    public class BlogController : ControllerBase
    {
        readonly IBlog blog;
        readonly IMapper mapper;
        readonly UserManager<SignUp> users;
        public BlogController(IBlog blog, IMapper mapper, UserManager<SignUp> users)
        {
            this.blog = blog;
            this.mapper = mapper;
            this.users = users;

        }
        [HttpGet("blogs")]
        public ActionResult<IEnumerable<BlogsDTO>> GetallBlogs(string username)
        {
            var query = blog.GetAllBlogs;
            var blogs = mapper.Map<IEnumerable<BlogViewDTO>>(query);
            
            foreach(var x in blogs) 
            {
                var user = users.FindByIdAsync(x.Id).Result;
                x.Username = user.UserName;
            }
            var post = mapper.Map<IEnumerable<BlogsDTO>>(blogs);
            return Ok(post);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BlogDTO>>> GetUserBlogs(string username)
         {
            var user = await users.FindByNameAsync(username);
            var blogs = blog.GetUserBlogs(user.Id);
            if(blogs == null) 
            {
                return NotFound();
            }
            var post = mapper.Map<IEnumerable<BlogDTO>>(blogs);
            return Ok(post);
        }
        [HttpPut]
        public async Task<ActionResult<BlogDTO>> AddBlog(BlogCreateDTO newblog,string username) 
        {
            var user = await users.FindByNameAsync(username);
            var post = mapper.Map<Blog>(newblog);
            post.OwnerId = user.Id;
            var addedpost = await blog.UpdateBlog(post, post.BlogId);
            var addedblog = mapper.Map<BlogDTO>(addedpost);
            await blog.SaveChanges();
            return Ok(addedblog);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBlogPost(Guid id) 
        {
            var userblog = await blog.GetBlogById(id);
            if(userblog != null) 
            {
                await blog.Delete(id);
                return NoContent();
            }
            return NotFound();
        }
    }
}
