using CouchbaseDemo.Model;
using CouchbaseDemo.Services;
using Microsoft.AspNetCore.Mvc;

namespace CouchbaseDemo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CacheController : Controller
    {
        private readonly ICouchbaseClusterProvider _couchbaseClusterProvider;

        public CacheController(ICouchbaseClusterProvider couchbaseClusterProvider)
        {
            _couchbaseClusterProvider = couchbaseClusterProvider;
        }

        [HttpPost]
        public async Task<IActionResult> AddItem([FromBody] OrderModel item)
        {
            await _couchbaseClusterProvider.SetAsync($"Order.id.{item.Id}", item, TimeSpan.FromMinutes(5));
            return Ok(new { Message = "Item added successfully." });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetItem(string id)
        {
            var result = await _couchbaseClusterProvider.GetAsync<OrderModel>($"Order.id.{id}");
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateItem([FromBody] OrderModel item)
        {
            await _couchbaseClusterProvider.UpdateAsync($"Order.id.{item.Id}", item, TimeSpan.FromMinutes(5));
            return Ok(new { Message = "Item updated successfully." });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(string id)
        {
            await _couchbaseClusterProvider.DeleteAsync($"Order.id.{id}");
            return Ok(new { Message = "Item deleted successfully." });
        }
    }
}
