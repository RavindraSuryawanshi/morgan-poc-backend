using System.ComponentModel.DataAnnotations;

namespace Morgan.SalesforceSyncPOC.Application.DTOs.Requests
{
    /// <summary>
    /// Request model for creating a user.
    /// </summary>
    public class CreateUserRequest
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
