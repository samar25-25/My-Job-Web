using AutoMapper;
using MyJobWeb.Data;
using MyJobWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyJobWeb.App_Start
{
    public static class AutoMapperConfig
    {
        public static IMapper Mapper { get; private set; }
        public static void Init()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Category, CategoryModel>().
                ForMember(dst => dst.Id, src => src.MapFrom(e => e.Id))
                .ForMember(dst => dst.ParentId, src => src.MapFrom(e => e.Category1.Parent_Id))
                .ForMember(dst => dst.ParentName, src => src.MapFrom(e => e.Category1.Name)).ReverseMap();

                cfg.CreateMap<JobDetail, JobdetailsModel>().
              ForMember(dst => dst.Job_Id, src => src.MapFrom(e => e.Id))
              .ForMember(dst => dst.PuplisherName, src => src.MapFrom(e => e.AspNetUser.FullName)).
              ForMember(dst => dst.UserId, src => src.MapFrom(e => e.AspNetUser.Id))
             .ForMember(dst => dst.Category_Name, src => src.MapFrom(e => e.Category.Name)).ReverseMap();



            });
            Mapper = config.CreateMapper();
        }
    }
}