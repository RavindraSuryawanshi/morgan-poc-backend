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
    /// Repository for outbox messages.
    /// </summary>
    public class OutboxMessageRepository : BaseRepository<OutboxMessage>
    {
        public OutboxMessageRepository(AppDbContext dbContext) : base(dbContext)
        {

        }
    }
}
