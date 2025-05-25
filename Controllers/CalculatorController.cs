using Microsoft.AspNetCore.Mvc;
using System.Xml.Serialization;

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
    [Consumes("application/json")]
    [Produces("application/json")]
    public async Task<IActionResult> PostJson([FromBody] OperationRequest request)
    {
        Console.WriteLine("json request at api: " + System.Text.Json.JsonSerializer.Serialize(request));
        OperationResponse response = new OperationResponse(request);
        //Console.WriteLine("request: " + request);
        Console.WriteLine("json response: " + System.Text.Json.JsonSerializer.Serialize(response));
        IActionResult result = Ok(response);
        return result;
    }
    
    [HttpPost("debug")]
    [Consumes("application/json")]
    [Produces("application/json")]
    public async Task<IActionResult> DebugPost()
    {
        using var reader = new StreamReader(Request.Body);
        var rawBody = await reader.ReadToEndAsync();
        Console.WriteLine("debug request: " + rawBody);

        return Ok(new { received = rawBody });
    }

    public string SerializeToXml<T>(T obj)
    {
        var xmlSerializer = new XmlSerializer(typeof(T));
        using (var stringWriter = new StringWriter())
        {
            xmlSerializer.Serialize(stringWriter, obj);
            return stringWriter.ToString();
        }
    }

    [HttpPost]
    [Consumes("application/xml")]
    [Produces("application/xml")]
    public async Task<IActionResult> PostXml([FromBody] OperationRequest request)
    {
        Console.WriteLine("xml request id at api: " + request.request_id);
        OperationResponse response = new OperationResponse(request);
        Console.WriteLine("xml response at api: " + SerializeToXml(response));
        IActionResult result = Ok(response);
        return result;
    }
}
