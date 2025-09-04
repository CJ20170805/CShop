using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CShop.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace CShop.Infrastructure.Identity
{
    public class AppUser: IdentityUser<Guid>
    {
        public UserProfile Profile { get; set; } = new UserProfile();
        public ICollection<UserAddress> Addresses { get; set; } = new List<UserAddress>();
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
