using AuthenApp.UserModel;
using AuthenApp.ViewDTO;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenApp.Profiles
{
    public class SignUpProfile : Profile
    {
        public SignUpProfile()
        {
            CreateMap<SignUpDTO, SignUp>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(s => s.UserName))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(s => s.Id))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(s => s.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(s => s.LastName))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(s => s.Password))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(s => s.Email))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(s => s.PhoneNumber));
            CreateMap<LoginModel, SignUp>()
                 .ForMember(dest => dest.UserName, opt => opt.MapFrom(s => s.Username))
                   .ForMember(dest => dest.Password, opt => opt.MapFrom(s => s.Password));
        }
    }
}
