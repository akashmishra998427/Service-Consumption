using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceConsumption.Models.Entity;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;

namespace ServiceConsumption.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CunsumeApiByFakeApiController : ControllerBase
    {
        private readonly IConfiguration _config;
        public CunsumeApiByFakeApiController(IConfiguration congig)
        {
            _config = congig;
        }

        [HttpGet("Product")]
        public async Task<IActionResult> GetProducts()
        {
            string BaseUrl = _config["ApiUrl:FakeApiUrl"];
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync($"{BaseUrl}/products");
                if ((response.IsSuccessStatusCode))
                {
                    var data = await response.Content.ReadAsStringAsync();
                    return Ok(data);
                }
                else
                {
                    return StatusCode(500, new {success = false, message = "there is a error in extrnal api!" });
                }
            }
        }

        [HttpPost("Product")]
        public async Task<IActionResult> AddProduct([FromBody] AddProductsEntity Entity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            string BaseUrl = _config["ApiUrl:FakeApiUrl"];

            using (var client = new HttpClient())
            {
                var payload = new
                {
                    id = Entity.ID,
                    title = Entity.Title,
                    price = Entity.Price,
                    description = Entity.Description,
                    category = Entity.category,
                    image = Entity.Image
                };
                var cvontent = new StringContent( JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

                using (HttpResponseMessage respone = await client.PostAsync($"{BaseUrl}/products", cvontent))
                {
                    if (respone.IsSuccessStatusCode)
                    {
                        var responseData = await respone.Content.ReadAsStringAsync();
                        return Ok(responseData);
                    }
                    else
                    {
                        return StatusCode((int)respone.StatusCode, new { success = false, message = "There is a error in external api!" });
                    }
                }
            }
        }

        [HttpGet("Product{ID}")]
        public async Task<IActionResult> GetProduct(int ID)
        {
            if(ID == 0 || ID == null)
            {
                return BadRequest("Invalid Id Please Pass a Valid ID");
            }
            string BaseUrl = _config["ApiUrl:FakeApiUrl"];

            using(var client = new HttpClient())
            {
                using(HttpResponseMessage Response = await client.GetAsync($"{BaseUrl}/products/{ID}"))
                {
                    if(Response.IsSuccessStatusCode)
                    {
                        var responseMessage = await Response.Content.ReadAsStringAsync();
                        return Ok(responseMessage);
                    }
                    else
                    {
                        return StatusCode((int)Response.StatusCode, new { success = false, message = "there is an error in external api!" });
                    }
                }
            }
        }

        [HttpPut("Product{ID}")]
        public async Task<IActionResult> UpdateProduct([FromBody] AddProductsEntity Entity , int ID)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }
            string BaseUrl = _config["ApiUrl:FakeApiUrl"];

            var payload = new
            {
                id = Entity.ID,
                title = Entity.Title,
                price = Entity.Price,
                description = Entity.Description,
                category = Entity.category,
                image = Entity.Image
            };
            var cvontent = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

            using (var client = new HttpClient())
            {
                using (HttpResponseMessage Response = await client.PutAsync($"{BaseUrl}/products/{ID}", cvontent))
                {
                    if (Response.IsSuccessStatusCode)
                    {
                        var responseMessage = await Response.Content.ReadAsStringAsync();
                        return Ok(responseMessage);
                    }
                    else
                    {
                        return StatusCode((int)Response.StatusCode, new { success = false, message = "there is an error in external api!" });
                    }
                }
            }
        }

        [HttpDelete("Products{ID}")]
        public async Task<IActionResult> DeleteProduct(int ID)
        {
            if (ID == null)
            {
                return BadRequest();
            }
            string BaseUrl = _config["ApiUrl:FakeApiUrl"];
            using(var client = new HttpClient())
            {
                using(HttpResponseMessage Response = await client.DeleteAsync($"{BaseUrl}/products/{ID}"))
                {
                    if(Response.IsSuccessStatusCode)
                    {
                        var ResponseMessage = await Response.Content.ReadAsStringAsync();
                        return Ok(ResponseMessage);
                    }
                    else
                    {
                        return StatusCode((int)Response.StatusCode, new { success = false, message = "There's a error in external service api!" });
                    }
                }
            }
        }
    }
}
