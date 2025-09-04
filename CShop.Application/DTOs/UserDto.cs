using CShop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CShop.Application.DTOs
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PlainPassword { get; set; } = null!;
        public IEnumerable<string> Roles { get; set; } = new List<string>();

        public UserProfileDto Profile { get; set; } = null!;

    }
}
