using global::Morgan.SalesforceSyncPOC.AzureFunctions.Models;
using Microsoft.EntityFrameworkCore;
using Morgan.SalesforceSyncPOC.Infrastrcture.Data;

namespace Morgan.SalesforceSyncPOC.AzureFunctions.Services
{
    /// <summary>
    /// Provides access to user data for Salesforce synchronization.
    /// </summary>
    public sealed class UserService : IUserService
    {
        private readonly AppDbContext _dbContext;

        public UserService(
            AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<UserDto?> GetByIdAsync(
            int userId,
            CancellationToken cancellationToken)
        {
            return await _dbContext.Users
                .Where(x => x.Id == userId)
                .Select(x => new UserDto
                {
                    Id = x.Id,
                    ExternalId = x.ExternalId,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Email = x.Email,
                    Phone = x.Phone
                })
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}