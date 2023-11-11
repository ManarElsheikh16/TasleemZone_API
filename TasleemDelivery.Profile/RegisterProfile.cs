using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using TasleemDelivery.DTO;
using TasleemDelivery.Models;

namespace TasleemDelivery.Profiles
{
    public class RegisterProfile:Profile
    {
        public RegisterProfile()
        {
            CreateMap<RegisterDTO, ApplicationUser>()
                .ForMember(src=>src.Id,opt=>opt.Ignore());

            CreateMap<RegisterDTO, Delivery>();
            CreateMap<RegisterDTO, Client>();
            CreateMap<RegisterDTO, Admin>();
            CreateMap<RegisterDTO, SubAdmin>();

            CreateMap<SubAdmin, RegisterDTO>()
           .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.ApplicationUser.UserName)).
           ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.ApplicationUser.PasswordHash)); // Assuming ApplicationUser has a Password property


        }
    }
}
