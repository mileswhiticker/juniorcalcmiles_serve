using Microsoft.AspNetCore.Mvc;

namespace juniorcalcmiles_serve.Controllers
{
    [ApiController]
    [Route("/")]
    public class RootController : ControllerBase
    {
        [HttpGet()]
        public string Get()
        {
            return "Welcome to my .Net 8 webapi sandbox. This is the index page. Head to /weatherforecast for a sample REST API. Head to /calculator for the calculator operations API.";
        }
    }
}
