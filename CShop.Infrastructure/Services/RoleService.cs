using CShop.Application.DTOs;
using CShop.Application.Interfaces;
using CShop.Infrastructure.Data;
using CShop.Domain.Entities;
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
        private readonly AppDbContext _context;
        public RoleService(AppDbContext context) => _context = context;

        public async Task<IEnumerable<RoleDto>> GetAllAsync()
        {
            return await _context.Roles
                .Select(
                    r => new RoleDto
                    {
                        Id = r.Id,
                        Name = r.Name,
                    })
                .ToListAsync();

        }

        public async Task<RoleDto?> GetByIdAsync(Guid id)
        {
            var role = await _context.Roles.FindAsync(id);
            if (role == null) { return null; }

            return new RoleDto { Id = role.Id, Name = role.Name };
        }

        public async Task<RoleDto> CreateAsync(RoleDto dto)
        {
            var role = new Role { Id = dto.Id, Name = dto.Name };

            _context.Roles.Add(role);
            await _context.SaveChangesAsync();

            return new RoleDto
            {
                Id = dto.Id,
                Name = dto.Name
            };
        }

        public async Task<RoleDto?> UpdateAsync(RoleDto dto)
        {
            var role = await _context.Roles.FindAsync(dto.Id);
            if (role == null) { return null; }

            role.Name = dto.Name;

            await _context.SaveChangesAsync();

            return new RoleDto { Id = dto.Id, Name = dto.Name };
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var role = await _context.Roles.FindAsync(id);
            if (role == null) return false;

            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> AssignRoleToUserAsync(Guid userId, Guid roleId)
        {
            var user = await _context.Users
                .Include(u => u.Roles)
                .FirstOrDefaultAsync(u => u.Id == userId);

            var role = await _context.Roles.FindAsync(roleId);

            if (user == null || role == null) { return false; }

            if (!user.Roles.Any(r => r.Id == roleId))
            {
                user.Roles.Add(role);
                await _context.SaveChangesAsync();
            }
            return true;
        }

        public async Task<bool> RemoveRoleFromUserAsync(Guid userId, Guid roleId)
        {
            var user = await _context.Users
           .Include(u => u.Roles)
           .FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null) return false;

            var role = user.Roles.FirstOrDefault(r => r.Id == roleId);
            if (role == null) return false;

            user.Roles.Remove(role);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
