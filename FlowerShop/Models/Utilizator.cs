using Microsoft.AspNetCore.Identity;

namespace Boutique.Models
{
    public class Utilizator : IdentityUser
    {
        public string FullName { get; set; }
    }
}
