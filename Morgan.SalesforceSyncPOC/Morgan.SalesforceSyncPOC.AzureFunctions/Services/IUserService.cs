using Morgan.SalesforceSyncPOC.AzureFunctions.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Morgan.SalesforceSyncPOC.AzureFunctions.Services
{
    /// <summary>
    /// Provides access to user data for Salesforce synchronization.
    /// </summary>
    public interface IUserService
    {
        Task<UserDto?> GetByIdAsync(
            int userId,
            CancellationToken cancellationToken);
    }
}
