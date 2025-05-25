using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace juniorcalcmiles_serve.Controllers;

[ApiController]
[Route("[controller]")]
public class CalculatorController : ControllerBase
{
    [HttpGet()]
    public string Get()
    {
        return "Welcome to the Calculator API. This is the index page. The instructions are (...)";
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] OperationRequest request)
    {
        //Console.WriteLine("request: " + request.request_id);
        IActionResult response = Ok(new OperationResponse(request));
        return response;
    }
    
    [HttpPost("debug")]
    public async Task<IActionResult> DebugPost()
    {
        using var reader = new StreamReader(Request.Body);
        var rawBody = await reader.ReadToEndAsync();
        Console.WriteLine("RAW BODY: " + rawBody);

        return Ok(new { received = rawBody });
    }
}
