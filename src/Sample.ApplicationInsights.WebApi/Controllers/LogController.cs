using System;
using Microsoft.AspNetCore.Mvc;

namespace Sample.ApplicationInsights.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class LogController : Controller
    {
        [HttpPost("sample")]
        public IActionResult PostSampleData()
        {
            return Ok(new { Result = "Data successfully registered" });
        }

        [HttpGet("exception")]
        public IActionResult GetByName()
        {
            //Sample middlerare exception log
            throw new Exception("Unable to perform the operation.");
        }
    }
}
