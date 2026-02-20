using Microsoft.AspNetCore.Mvc;

namespace TaskManagerApi.Controller;

[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new { message = "API is working on mac"});
    }

}