using Morgan.SalesforceSyncPOC.Core.DataModels;
using Morgan.SalesforceSyncPOC.Core.Events;
using Morgan.SalesforceSyncPOC.Infrastrcture.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Morgan.SalesforceSyncPOC.Infrastrcture.Repositories
{
    /// <summary>
    /// Repository for user entities.
    /// </summary>
    public class UserRepository : BaseRepository<User>
    {
        public UserRepository(AppDbContext dbContext) : base(dbContext)
        {

        }
    }
}
