using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Route("identity")]
    [Authorize]
    public class IdentityController : Controller
    {
        [HttpGet]
        [ProducesResponseType(typeof(JsonResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status403Forbidden)]

        public IActionResult GetExtraClaims()
        {
            var list = new List<dynamic>();
            var nameClaim = User.Claims.ToList().Find(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
            if (nameClaim.Value == "177facc7-0b30-42af-bddd-b6af65e8c804")
            {
                list = CarsonClaims();
            }
            else if (nameClaim.Value == "cf5ce848-1616-4b2c-85b3-9e9f979a4da9")
            {
                list = AliceClaims();
            }
            else if (nameClaim.Value == "2632fa7c-672f-40b9-b715-79e1f78ab642")
            {
                list = BobClaims();
            }
            else
            {
                list = new List<dynamic>();
            }
            return new JsonResult(from c in list select new { c.Type, c.Value });
        }

        public List<dynamic> AliceClaims()
        {
            var list2 = new List<dynamic>
            {
                new { Type = "emp", Value = "Alice" },
                new { Type = "employer", Value = "SL Inc" },
                new { Type = "jobtitle", Value= "Architect" },
                new { Type = "project", Value= "Common" },

            };
            return list2;
        }

        public List<dynamic> BobClaims()
        {
            var list2 = new List<dynamic>
            {
                new { Type = "emp", Value = "Bob" },
                new { Type = "employer", Value = "Gov Inc" },
                new { Type = "jobtitle", Value= "Analyst" },
                new { Type = "project", Value= "Payments" },

            };
            return list2;
        }

        public List<dynamic> CarsonClaims()
        {
            var list2 = new List<dynamic>
            {
                new { Type = "emp", Value = "Carson" },
                new { Type = "employer", Value = "SL inc" },
                new { Type = "jobtitle", Value= "Developer" },
                new { Type = "project", Value= "Identity" },

            };
            return list2;
        }
    }
}
