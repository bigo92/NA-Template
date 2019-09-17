using NA.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace NA.Domain.Models
{
    public class UsserServiceModel
    {
        public class Register_UsserServiceModel
        {
            public virtual string email { get; set; }
            public virtual string password { get; set; }

            public virtual string roleType { get; set; }
        }
    }
}
