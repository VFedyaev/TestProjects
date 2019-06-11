using AutoMapper;
using Projects.BLL.DTO;
using Projects.DAL.Entities;

namespace Projects.BLL.MappingProfiles
{
    public class BLLMappingProfile : Profile
    {
        public BLLMappingProfile()
        {
            CreateMap<ExecutorCompany, ExecutorCompanyDTO>(MemberList.None).ReverseMap();
            CreateMap<Position, PositionDTO>(MemberList.None).ReverseMap();
            CreateMap<Employee, EmployeeDTO>(MemberList.None).ReverseMap();
            CreateMap<Customer, CustomerDTO>(MemberList.None).ReverseMap();
            CreateMap<Project, ProjectDTO>(MemberList.None).ReverseMap();
            CreateMap<ProjectEmployee, ProjectEmployeeDTO>(MemberList.None).ReverseMap();
        }
    }
}
