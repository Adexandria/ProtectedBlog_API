using AuthenApp.BlogModel;
using AuthenApp.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO.Pipelines;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenApp.BlogServices
{
    public class Blogrepos : IBlog
    {
        readonly IdentityDb db;
        public Blogrepos(IdentityDb db)
        {
            this.db = db;
        }
        public IEnumerable<Blog> GetAllBlogs 
        {
            get 
            {
                return db.BlogsP.OrderBy(r => r.BlogId).AsNoTracking();
            }
        }

        public IEnumerable<Blog> GetUserBlogs (string id) 
        {
            if(id == null) 
            {
                throw new NullReferenceException(nameof(id));
            }
            return db.BlogsP.Where(r => r.OwnerId == id).OrderBy(r => r.BlogId).AsNoTracking();
        }

        public async Task<int> Delete(Guid id)
        {
            if (id == null)
            {
                throw new NullReferenceException(nameof(id));
            }
            var query = await GetBlogById(id);
            db.BlogsP.Remove(query);
            return  await db.SaveChangesAsync();
        }

        public async Task<Blog> GetBlogById(Guid id)
        {
            if (id == null)
            {
                throw new NullReferenceException(nameof(id));
            }
            return await db.BlogsP.Where(r => r.BlogId == id).AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task<int> SaveChanges()
        {
            return await db.SaveChangesAsync();
        }

        public async Task<Blog> UpdateBlog(Blog updatedpost,Guid id)
        {
            var query = await GetBlogById(id);
            if (query == null) 
            {
                await AddBlog(updatedpost);
            }
            else
            {
                db.Entry(query).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
                db.Entry(updatedpost).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            }
            return await GetBlogById(updatedpost.BlogId);
        }
        private async Task<Blog> AddBlog(Blog post)
        {
            if (post == null)
            {
                throw new NullReferenceException(nameof(post));
            }
            post.BlogId = Guid.NewGuid();
            await db.BlogsP.AddAsync(post);
            await db.SaveChangesAsync();
            return post;
        }
    }
}
