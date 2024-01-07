using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomHelper.Service.Interfaces
{
    public interface IPasswordResetAttemptService
    {
        Task AddAttemptAsync(string userId);
        Task<bool> HasAttempts(string userId);
        Task RefreshAttempts(string userId);
    }
}
