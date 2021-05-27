using AutoMapper;
using Events.Api.Models.UserManagement;


namespace UserManagment.Services
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<EUser, EmployeeModel>();
            CreateMap<RegisterModel, EUser>();
            CreateMap<UpdateModel, EUser>();
        }
    }
}

