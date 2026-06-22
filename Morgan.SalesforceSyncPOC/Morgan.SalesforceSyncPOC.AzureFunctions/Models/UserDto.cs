using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Morgan.SalesforceSyncPOC.AzureFunctions.Models
{
    /// <summary>
    /// User data transferred between services.
    /// </summary>
    public sealed class UserDto
    {
        public int Id { get; set; }

        public Guid ExternalId { get; set; }

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string? Phone { get; set; }
    }
}
