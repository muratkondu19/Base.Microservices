using AutoMapper;
using FreeCourse.Services.Catolog.Dtos;
using FreeCourse.Services.Catolog.Models;

namespace FreeCourse.Services.Catolog.Mapping {
    public class GeneralMapping : Profile {
        public GeneralMapping() {
            CreateMap<Course, CourseDto>().ReverseMap();
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<Feature, FeatureDto>().ReverseMap();

            CreateMap<Course, CourseCreateDto>().ReverseMap();
            CreateMap<Course, CourseUpdateDto>().ReverseMap();
        }
    }
}
