using NA.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace NA.Domain.Models
{
    public class UsserServiceModel
    {
        public class RegisterAccountModel : ApplicationUser
        {
           public virtual string password { get; set; }

            public virtual string roleType { get; set; }
        }
    }
}
