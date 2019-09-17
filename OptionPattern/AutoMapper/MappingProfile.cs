using AutoMapper;
using OptionPattern.Model;

namespace OptionPattern.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<BasicOptions, BasicOptionsDto>();
        }
    }
}
