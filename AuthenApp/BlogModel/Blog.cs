using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenApp.BlogModel
{
    public class Blog
    {
        [Key]
        public Guid BlogId { get; set; }
        [ForeignKey("AspNetUsers")]
        public string OwnerId { get; set; }
        public string Post { get; set; }
        public Category Category { get; set; }
      

    }
}
