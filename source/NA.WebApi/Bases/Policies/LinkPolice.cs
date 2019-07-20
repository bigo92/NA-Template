using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace NA.WebApi.Bases.Policies
{
    public class LinkPolice : IAuthorizationRequirement
    {
        public LinkPolice()
        {

        }
    }
}
