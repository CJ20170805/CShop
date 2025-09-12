using CShop.Application.DTOs;
using CShop.Application.Interfaces;
using CShop.Domain.Entities;
using CShop.Infrastructure.Data;
using CShop.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace CShop.Infrastructure.Services
{
    public class UserService: IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IMapper _mapper;

        public UserService(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, IMapper mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
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
                var dto = _mapper.Map<UserDto>(user);
                dto.Roles = await _userManager.GetRolesAsync(user);
                result.Add(dto);
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

            var dto = _mapper.Map<UserDto>(user);
            dto.Roles = await _userManager.GetRolesAsync(user);
            return dto; 
        }

        public async Task<UserDto> CreateAsync(UserDto userDto)
        {
            var user = _mapper.Map<AppUser>(userDto);

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

            var dto = _mapper.Map<UserDto>(user);
            dto.Roles = await _userManager.GetRolesAsync(user);
            return dto;
        }

        public async Task<UserDto?> UpdateAsync(UserDto userDto)
        {
            var user = await _userManager.Users
              .Include(u => u.Profile)
              .Include(u => u.Addresses)
              .FirstOrDefaultAsync(u => u.Id == userDto.Id);

            if (user == null) return null;

            // update values
            _mapper.Map(userDto, user);

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

            var dto = _mapper.Map<UserDto>(user);
            dto.Roles = await _userManager.GetRolesAsync(user);
            return dto;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null) return false;

            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded;
        }

    }
}
