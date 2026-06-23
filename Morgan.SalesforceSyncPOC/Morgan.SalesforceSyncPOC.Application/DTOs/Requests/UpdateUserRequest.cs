using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [Required]
        [StringLength(100)]
        public string FirstName { get; set; } = "";

        [Required]
        [StringLength(100)]
        public string LastName { get; set; } = "";

        [Required]
        [EmailAddress]
        [StringLength(255)]
        public string Email { get; set; } = "";

        [Required]
        [StringLength(50)]
        public string Phone { get; set; } = "";
    }
}
