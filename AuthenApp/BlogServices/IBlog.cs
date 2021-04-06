using AuthenApp.BlogModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenApp.BlogServices
{
    public interface IBlog
    {
        public IEnumerable<Blog> GetAllBlogs { get; }
        public IEnumerable<Blog> GetUserBlogs(string id);
        public Task<Blog> GetBlogById(Guid id);
        public Task<Blog> UpdateBlog(Blog updatedpost,Guid id);
        public Task<int> Delete(Guid id);

        public Task<int> SaveChanges();

    }
}
