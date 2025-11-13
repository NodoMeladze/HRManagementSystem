using AutoMapper;
using HRManagement.Web.Models.API;

namespace HRManagement.Web.Mappers
{
    public class MvcMappingProfile : Profile
    {
        public MvcMappingProfile()
        {
            CreateMap<EmployeeViewModel, object>()
                .ConvertUsing(src => new
                {
                    id = src.Id,
                    personalNumber = src.PersonalNumber,
                    firstName = src.FirstName,
                    lastName = src.LastName,
                    gender = src.Gender,
                    dateOfBirth = src.DateOfBirth.ToString("yyyy-MM-dd"),
                    email = string.IsNullOrWhiteSpace(src.Email) ? null : src.Email,
                    positionId = src.PositionId,
                    status = src.Status,
                    releaseDate = src.ReleaseDate.HasValue
                        ? src.ReleaseDate.Value.ToString("yyyy-MM-dd")
                        : null
                });

            CreateMap<RegisterViewModel, object>()
                .ConvertUsing(src => new
                {
                    personalNumber = src.PersonalNumber,
                    firstName = src.FirstName,
                    lastName = src.LastName,
                    gender = src.Gender,
                    dateOfBirth = src.DateOfBirth.ToString("yyyy-MM-dd"),
                    email = src.Email,
                    password = src.Password,
                    confirmPassword = src.ConfirmPassword
                });

            CreateMap<LoginViewModel, object>()
                .ConvertUsing(src => new
                {
                    username = src.Username,
                    password = src.Password
                });

            CreateMap<PositionViewModel, object>()
                .ConvertUsing(src => new
                {
                    name = src.Name,
                    parentId = src.ParentId
                });
        }
    }
}