using AutoMapper;
using EmployeeManagementSystem.DTOs;
using EmployeeManagementSystem.Entities;

namespace EmployeeManagementSystem.Common
{
    public class AutomapperProfile : Profile
    {
        public  AutomapperProfile()
        {
            CreateMap<EmployeeBasicDetailsEntity, EmployeeBasicDetailsDTO>();
            CreateMap<EmployeeAdditionalDetailsEntity, EmployeeAdditionalDetailsDTO>();
            CreateMap<AddressEntity, AddressDTO>();
            CreateMap<WorkInfoEntity, WorkInfoDTO>();
            CreateMap<PersonalDetailsEntity, PersonalDetailsDTO>();
            CreateMap<IdentityInfoEntity, IdentityInfoDTO>();
        }
    }
}

