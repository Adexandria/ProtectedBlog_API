using AuthenApp.BlogModel;
using AuthenApp.BlogView;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenApp.Profiles
{
    public class BlogProfile :Profile
    {
        public BlogProfile()
        {
            CreateMap<BlogViewDTO, BlogsDTO>()
            .ForMember(dest => dest.Post, opt => opt.MapFrom(s => s.Post))
              .ForMember(dest => dest.Category, opt => opt.MapFrom(s => s.Category.ToString()));
            CreateMap<Blog, BlogViewDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(s => s.OwnerId))
            .ForMember(dest => dest.Post, opt => opt.MapFrom(s => s.Post))
              .ForMember(dest => dest.Category, opt => opt.MapFrom(s => s.Category.ToString()));
            CreateMap<Blog, BlogDTO>()
            .ForMember(dest => dest.Post, opt => opt.MapFrom(s => s.Post))
              .ForMember(dest => dest.Category, opt => opt.MapFrom(s => s.Category.ToString()));
            CreateMap<BlogCreateDTO, Blog>()
            .ForMember(dest => dest.Post, opt => opt.MapFrom(s => s.Post))
              .ForMember(dest => dest.Category, opt => opt.MapFrom(s => s.Category.ToString()));
        }
    }
}
