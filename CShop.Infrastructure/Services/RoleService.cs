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
    public class RoleService : IRoleService
    {
       // private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        // public RoleService(AppDbContext context) => _context = context;
        public RoleService(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IEnumerable<RoleDto>> GetAllAsync()
        {
            return await _roleManager.Roles
                .Select(
                    r => new RoleDto
                    {
                        Id = r.Id,
                        Name = r.Name!,
                    })
                .ToListAsync();

        }

        public async Task<RoleDto?> GetByIdAsync(Guid id)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            if (role == null) { return null; }

            return new RoleDto { Id = role.Id, Name = role.Name! };
        }

        public async Task<RoleDto> CreateAsync(RoleDto dto)
        {
            var role = new AppRole 
            { 
                Id = dto.Id, 
                Name = dto.Name,
                NormalizedName = dto.Name!.ToUpper()
            };

            var result = await _roleManager.CreateAsync(role);

            if (!result.Succeeded)
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));

            return new RoleDto
            {
                Id = dto.Id,
                Name = dto.Name
            };
        }

        public async Task<RoleDto?> UpdateAsync(RoleDto dto)
        {
            var role = await _roleManager.FindByIdAsync(dto.Id.ToString());
            if (role == null) { return null; }

            role.Name = dto.Name;
            role.NormalizedName = dto.Name!.ToUpper();


            var result = await _roleManager.UpdateAsync(role);

            if (!result.Succeeded)
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));

            return new RoleDto { Id = dto.Id, Name = dto.Name };
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            if (role == null) return false;

            var result = await _roleManager.DeleteAsync(role);

            return result.Succeeded;
        }

        //public async Task<bool> AssignRoleToUserAsync(Guid userId, Guid roleId)
        //{
        //    var user = await _userManager.FindByIdAsync(userId.ToString());
        //    var role = await _roleManager.FindByIdAsync(roleId.ToString());

        //    if (user == null || role == null) return false;

        //    if (string.IsNullOrWhiteSpace(role?.Name)) return false;

        //    var isInRole = await _userManager.IsInRoleAsync(user, role.Name);
        //    if (!isInRole)
        //    {
        //        var result = await _userManager.AddToRoleAsync(user, role.Name);
        //        return result.Succeeded;
        //    }

        //    return true;

        //}

        //public async Task<bool> RemoveRoleFromUserAsync(Guid userId, Guid roleId)
        //{
        //    var user = await _context.Users
        //   .Include(u => u.Roles)
        //   .FirstOrDefaultAsync(u => u.Id == userId);
        //    if (user == null) return false;

        //    var role = user.Roles.FirstOrDefault(r => r.Id == roleId);
        //    if (role == null) return false;

        //    user.Roles.Remove(role);
        //    await _context.SaveChangesAsync();

        //    return true;
        //}
    }
}
