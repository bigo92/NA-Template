using Microsoft.AspNetCore.Identity;

namespace NA.DataAccess.Models
{
    public class ApplicationRole: IdentityRole<long>
    {
        public virtual string data { get; set; }
        public virtual string data_db { get; set; }
    }
}
