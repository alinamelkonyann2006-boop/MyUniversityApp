using Microsoft.AspNetCore.Mvc;

namespace MyUniversityApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExternalApiController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public ExternalApiController(IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient();
        }

        [HttpGet]
        public async Task<IActionResult> GetData()
        {
            var response = await _httpClient.GetAsync(
                "https://jsonplaceholder.typicode.com/todos/1");

            var json = await response.Content.ReadAsStringAsync();

            return Ok(json);
        }
    }
}