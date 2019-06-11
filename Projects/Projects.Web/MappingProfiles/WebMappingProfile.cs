using AutoMapper;
using Projects.BLL.DTO;
using Projects.Web.Models.Entities;

namespace Projects.Web.MappingProfiles
{
    public class WebMappingProfile : Profile
    {
        public WebMappingProfile()
        {
            CreateMap<ExecutorCompanyDTO, ExecutorCompanyVM>(MemberList.None).ReverseMap();
            CreateMap<PositionDTO, PositionVM>(MemberList.None).ReverseMap();
            CreateMap<EmployeeDTO, EmployeeVM>(MemberList.None).ReverseMap();
            CreateMap<CustomerDTO, CustomerVM>(MemberList.None).ReverseMap();
            CreateMap<ProjectDTO, ProjectVM>(MemberList.None).ReverseMap();
        }
    }
}