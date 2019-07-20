using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace NA.WebApi.Bases.Policies
{
    public class LinkPoliceHandler : AuthorizationHandler<LinkPolice>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, LinkPolice requirement)
        {            
            var filter = ((AuthorizationFilterContext)context.Resource);
            var controller = filter.RouteData.Values["controller"].ToString().ToLower();
            var action = filter.RouteData.Values["action"].ToString().ToLower();
            Claim clmGenaric = new Claim("link",$"api.{controller}.{action}");

            var isAuth = false;
            foreach (var item in context.User.Claims)
            {
                var value = item.Value.Split(',');
                if (item.Type == clmGenaric.Type && value.Contains(clmGenaric.Value))
                {
                    isAuth = true;
                    break;
                }
            }
            if (isAuth)
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }
            return Task.CompletedTask;
        }
    }
}
