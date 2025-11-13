using HRManagement.Domain.Entities;

namespace HRManagement.Domain.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<bool> EmailExistsAsync(string email);
        Task<bool> PersonalNumberExistsAsync(string personalNumber);
    }
}
