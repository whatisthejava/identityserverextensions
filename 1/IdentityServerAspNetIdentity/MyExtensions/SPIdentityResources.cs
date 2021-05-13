using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.MyExtensions
{
    public static class SPIdentityResources
    {
        public static IdentityResource Rugby()
        {
            var claims = new List<string>()
            {
                "position",
                "club",
                "country"
            };
            return new IdentityResource("rugby", claims);
        }

        public static IdentityResource Bmi()
        {
            var claims = new List<string>()
            {
                "height",
                "weight",
            };
            return new IdentityResource("bmi", claims);
        }
    }
}
