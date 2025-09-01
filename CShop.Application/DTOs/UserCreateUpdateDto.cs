using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CShop.Application.DTOs
{
    public class UserCreateUpdateDto
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public IEnumerable<string> Roles { get; set; } = new List<string>();

        public UserProfileDto Profile { get; set; } = null!;
    }
}
