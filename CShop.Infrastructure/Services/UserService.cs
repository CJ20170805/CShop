using CShop.Application.DTOs;
using CShop.Application.Interfaces;
using CShop.Domain.Entities;
using CShop.Infrastructure.Data;
using CShop.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CShop.Infrastructure.Services
{
    public class UserService: IUserService
    {
        // private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        //public UserService(AppDbContext context) => _context = context;
        public UserService(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            var users = await _userManager.Users
                .Include(u => u.Addresses)
                .Include(u => u.Profile)
                .ToListAsync();

            var result = new List<UserDto>();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                result.Add(MapToDto(user, roles));
            }

            return result;
        }

        public async Task<UserDto?> GetByIdAsync(Guid id)
        {
            var user = await _userManager.Users
                .Include(u => u.Addresses)
                .Include(u => u.Profile)
                .FirstOrDefaultAsync(u => u.Id == id);
            if (user == null) return null;

            var roles = await _userManager.GetRolesAsync(user);
            return MapToDto(user, roles); 
        }

        public async Task<UserDto> CreateAsync(UserDto userDto)
        {
            var user = new AppUser
            {
                UserName = userDto.UserName,
                Email = userDto.Email,
                Profile = new UserProfile
                {
                    FirstName = userDto.Profile.FirstName,
                    MiddleName = userDto.Profile.MiddleName,
                    LastName = userDto.Profile.LastName,
                    Phone = userDto.Profile.Phone,
                    City = userDto.Profile.City,
                    Country = userDto.Profile.Country
                },
            };

            var result = await _userManager.CreateAsync(user, userDto.PlainPassword);
            if (!result.Succeeded)
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));



            if (userDto.Roles != null && userDto.Roles.Any())
            {
                foreach (var role in userDto.Roles)
                {
                    if (await _roleManager.RoleExistsAsync(role))
                        await _userManager.AddToRoleAsync(user, role);
                }
            }

            var roles = await _userManager.GetRolesAsync(user);
            return MapToDto(user, roles);
        }

        public async Task<UserDto?> UpdateAsync(UserDto userDto)
        {
            var user = await _userManager.Users
              .Include(u => u.Profile)
              .Include(u => u.Addresses)
              .FirstOrDefaultAsync(u => u.Id == userDto.Id);

            if (user == null) return null;

            // update profile
            user.Email = userDto.Email;
            user.UserName = userDto.UserName;
            user.Profile.FirstName = userDto.Profile.FirstName;
            user.Profile.MiddleName = userDto.Profile.MiddleName;
            user.Profile.LastName = userDto.Profile.LastName;
            user.Profile.Phone = userDto.Profile.Phone;
            user.Profile.City = userDto.Profile.City;
            user.Profile.Country = userDto.Profile.Country;

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
                throw new Exception(string.Join(", ", updateResult.Errors.Select(e => e.Description)));

            // update roles
            var currentRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, currentRoles);

            if(userDto.Roles != null && userDto.Roles.Any())
            {
                foreach (var role in userDto.Roles)
                {
                    if(await _roleManager.RoleExistsAsync(role))
                        await _userManager.AddToRoleAsync(user, role);
                }
            }

            var roles = await _userManager.GetRolesAsync(user);

            return MapToDto(user, roles);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null) return false;

            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded;
        }

        private static UserDto MapToDto(AppUser user, IEnumerable<string> roles) =>
            new UserDto
            {
                Id = user.Id,
                UserName = user.UserName!,
                Email = user.Email!,
                Roles = roles,
                Profile = new UserProfileDto
                {
                    FirstName = user.Profile.FirstName,
                    MiddleName = user.Profile.MiddleName ?? "",
                    LastName = user.Profile.LastName,
                    Phone = user.Profile.Phone,
                    City = user.Profile.City,
                    Country = user.Profile.Country 
                }
            };

    }
}
