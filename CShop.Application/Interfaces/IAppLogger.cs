using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CShop.Application.Interfaces
{
    public interface IAppLogger<T>
    {
        void LogDebug(string message, params object[] args);
        void LogTrace(string message, params object[] args);
        void LogInformation(string message, params object[] args);
        void LogWarning(string message, params object[] args);
        void LogError(Exception ex, string message, params object[] args);
    }
}
