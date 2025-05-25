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
        Console.WriteLine("request at api: " + System.Text.Json.JsonSerializer.Serialize(request));
        OperationResponse response = new OperationResponse(request);
        //Console.WriteLine("request: " + request);
        Console.WriteLine("response: " + System.Text.Json.JsonSerializer.Serialize(response));
        IActionResult result = Ok(response);
        return result;
    }
    
    [HttpPost("debug")]
    public async Task<IActionResult> DebugPost()
    {
        using var reader = new StreamReader(Request.Body);
        var rawBody = await reader.ReadToEndAsync();
        Console.WriteLine("debug request: " + rawBody);

        return Ok(new { received = rawBody });
    }
}
