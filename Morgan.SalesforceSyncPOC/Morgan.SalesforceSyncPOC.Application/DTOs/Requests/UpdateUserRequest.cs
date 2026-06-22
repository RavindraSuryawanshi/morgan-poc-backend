using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Morgan.SalesforceSyncPOC.Application.DTOs.Requests
{
    /// <summary>
    /// Request model for updating a user.
    /// </summary>
    public class UpdateUserRequest
    {
        public string FirstName { get; set; } = "";

        public string LastName { get; set; } = "";

        public string Email { get; set; } = "";

        public string Phone { get; set; } = "";
    }
}
