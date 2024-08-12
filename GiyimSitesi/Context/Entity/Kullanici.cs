using Microsoft.AspNetCore.Identity;

namespace GiyimSitesi.Context.Entity
{
    public class Kullanici : IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }
    }
}
