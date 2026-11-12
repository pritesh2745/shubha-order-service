using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using OrderService.Models;

namespace OrderService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private static readonly List<Order> Orders = new();
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        public OrderController(IHttpClientFactory httpClientFactory, IConfiguration config)
        {
            _httpClient = httpClientFactory.CreateClient();
            _config = config;
        }

        [HttpGet]
        public IActionResult GetOrders() => Ok(Orders);

        [HttpPost("{braceletId:int}")]
        public async Task<IActionResult> PlaceOrder(int braceletId, [FromQuery] string customerName = "Pritesh Vandra")
        {
            var catalogBaseUrl = _config["CatalogServiceUrl"] ?? "http://catalog-service:80";
            var response = await _httpClient.PutAsync($"{catalogBaseUrl}/api/catalog/{braceletId}/stock", null);

            if (!response.IsSuccessStatusCode)
            {
                return BadRequest(new { Status = "Failed", Reason = "Bracelet is out of stock or invalid ID." });
            }

            var newOrder = new Order
            {
                OrderId = Guid.NewGuid(),
                BraceletId = braceletId,
                CustomerName = customerName,
                Status = "Confirmed",
                OrderTimestampUtc = DateTime.UtcNow
            };
            Orders.Add(newOrder);

            return Ok(new { Message = "shubha.co order placed successfully!", Order = newOrder });
        }
    }
}
