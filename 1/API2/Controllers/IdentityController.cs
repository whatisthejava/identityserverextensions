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
            var list2 = new List<dynamic>
            {
                new { Type = "employer", Value = "SL" },
                new { Type = "jobtitle", Value= "Technical Architect" }
            };
            return new JsonResult(from c in list2 select new { c.Type, c.Value } );
        }
    }
}
