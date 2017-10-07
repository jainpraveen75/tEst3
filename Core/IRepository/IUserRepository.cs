using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Core.Entities;
using Core.ViewModels;

namespace Core.IRepository
{
    public interface IUserRepository : IDisposable
    {
       

        Task<User> FindAsync(string username);
        Task<User> FindByNameAsync(string email);
        Task<bool> IsEmailConfirmedAsync(int id);
        Task<bool> IsLockedOutAsync(int id);
        Task<bool> CheckPasswordAsync(string user, string password);
        Task<ClaimsIdentity> CreateIdentityAsync(User user, string authenticationType);
        Task<ApplicationIdentityResult> AccessFailedAsync(int userId);
        User FindById(int userId);
        Task<ApplicationIdentityResult> AddPasswordAsync(int userId, string newPassword);
        Task<User> FindByIdAsync(int userId);
        Task<ApplicationIdentityResult> ChangePasswordAsync(int userId, string oldPassword, string newPassword);
        PaginatedList<User> FindAll(UserSearchViewModel model);
    }
}