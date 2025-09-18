using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceConsumption.Models.Entity;
using System.Data.SqlTypes;
using System.Text.Json;

namespace ServiceConsumption.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CunsumeServiceController : ControllerBase
    {
        private readonly IConfiguration _config;
        public CunsumeServiceController(IConfiguration congig)
        {
            _config = congig;
        }

        [HttpGet("Post")]
        public async Task<IActionResult> GetExternolApiData()
        {
            string BaseUrl = _config["ApiUrl:BaseUrl"];
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync($"{BaseUrl}/posts");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                return Ok(data);
            }
            else
            {
                return StatusCode((int)response.StatusCode, "Error fetching data from external API");
            }
        }

        [HttpPost("Post")]
        public async Task<IActionResult> PostApiData([FromBody] PostApiEntity Entity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            using (HttpClient client = new HttpClient())
            {
                string BaseUrl = _config["ApiUrl:BaseUrl"];

                var Payload = new
                {
                    title = Entity.Title,
                    body = Entity.Body,
                    userId = Entity.UserID
                };

                var content = new StringContent(
                    JsonSerializer.Serialize(Payload),
                    System.Text.Encoding.UTF8,
                    "application/json"
                );

                using (HttpResponseMessage response = await client.PostAsync($"{BaseUrl}/posts", content))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var responseData = await response.Content.ReadAsStringAsync();
                        return Ok(responseData);
                    }
                    else
                    {
                        return StatusCode((int)response.StatusCode, "Error posting data to external API");
                    }
                }
            }
        }

        [HttpGet("Post/{ID}")]
        public async Task<IActionResult> GetPostDatabyID(int ID)
        {
            string BaseUrl = _config["ApiUrl:BaseUrl"];

            using (HttpClient client = new HttpClient())
            {
                var Url = $"{BaseUrl}/posts/{ID}";
                HttpResponseMessage response = await client.GetAsync(Url);

                if (response.IsSuccessStatusCode)
                {
                    string responseData = await response.Content.ReadAsStringAsync();
                    return Ok(responseData);
                }
                else
                {
                    return StatusCode((int)response.StatusCode, "Error fetching data from external API");
                }
            }
        }

        [HttpGet("PostWithImage")]
        public async Task<IActionResult> GetAllPostWithImage()
        {
            try
            {
                string BaseUrl = _config["ApiUrl:BaseUrl"];
                using (HttpClient client = new HttpClient())
                {
                    var ImgApiUrl = $"{BaseUrl}/photos";
                    HttpResponseMessage Response = await client.GetAsync(ImgApiUrl);
                    if (Response.IsSuccessStatusCode)
                    {
                        string responseData = await Response.Content.ReadAsStringAsync();
                        return Ok(responseData);
                    }
                    else
                    {
                        return StatusCode(500, new { success = false, message = "there is a error in extrnal api!" });
                    }
                }
            }
            catch(Exception Ex)
            {
                return BadRequest($"Error to call external service https://jsonplaceholder.typicode.com/photos \n {Ex.Message}");
            }
        }
    }
}
