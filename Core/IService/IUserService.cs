using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Core.Entities;
using Core.Enums;
using Core.ViewModels;

namespace Core.IService
{
    public interface IUserService : IDisposable
    {
        void SignOut(params string[] authenticationTypes);
        Task<SignInStatus> PasswordSignIn(string userName, string password, bool isPersistent,bool shouldLockout);
        Task<ApplicationIdentityResult> CreateAsync(User user, string password);
        Task<string> GenerateEmailConfirmationTokenAsync(object id);
        Task<ApplicationIdentityResult> ConfirmEmailAsync(int value, string code);
        Task SendEmailAsync(object id, string confirmYourAccount, string s);
        Task<User> FindByNameAsync(string email);
        Task<bool> IsEmailConfirmedAsync(int id);
        Task<string> GeneratePasswordResetTokenAsync(int id);
        Task<ApplicationIdentityResult> ResetPasswordAsync(int id, string code, string password);
        Task<ApplicationIdentityResult> ChangePasswordAsync(int userId, string oldPassword, string newPassword);
        Task<User> FindByIdAsync(int userId);
        Task<ApplicationIdentityResult> AddPasswordAsync(int userId, string newPassword);
        
        Task<ClaimsIdentity> CreateIdentityAsync(User user, string applicationCookie);
        void SignIn(bool isPersistent, ClaimsIdentity claimsIdentity);
        User FindById(int userId);
        PaginatedList<User> FindAll(UserSearchViewModel model);
    }
}