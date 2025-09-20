using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CShop.Application.Options
{
    public class StorageOptions
    {
        public string ConnectionString { get; set; } = string.Empty;
        public Dictionary<string, string> Containers { get; set; } = new Dictionary<string, string>();
    }
}
