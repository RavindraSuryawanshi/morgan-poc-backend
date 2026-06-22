using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Morgan.SalesforceSyncPOC.Core.DataModels
{
    /// <summary>
    /// Represents a user synchronized with external systems.
    /// </summary>
    public class User
    {
        public int Id { get; set; }

        /// <summary>
        /// Identifier of the user in Salesforce.
        /// </summary>
        public Guid ExternalId { get; set; }

        public string FirstName { get; set; } = "";

        public string LastName { get; set; } = "";

        public string Email { get; set; } = "";

        public string Phone { get; set; } = "";
    }
}
