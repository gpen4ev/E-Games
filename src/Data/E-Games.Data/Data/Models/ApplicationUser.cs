using Microsoft.AspNetCore.Identity;

namespace E_Games.Data.Data.Models
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string? AddressDelivery { get; set; }
    }
}
