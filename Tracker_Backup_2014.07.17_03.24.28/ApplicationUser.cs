using leeksnet.AspNet.Identity.TableStorage;

namespace Tracker
{
    public class ApplicationUser : IdentityUser
    {

        public string Imei { get; set; }

    }
}
