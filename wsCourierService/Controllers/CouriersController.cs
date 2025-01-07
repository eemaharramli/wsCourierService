using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using wsCourierService.Data;
using wsCourierService.Models;

namespace wsCourierService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CouriersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public CouriersController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Courier courier)
        {
            if (await _context.Couriers.AnyAsync(c => c.Email == courier.Email))
            {
                return BadRequest("A courier with this email already exists.");
            }

            var hashingKey = Encoding.UTF8.GetBytes(_configuration["HashingKey"]!);
            using var hmac = new HMACSHA256(hashingKey);
            courier.PasswordHash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(courier.PasswordHash)));

            _context.Couriers.Add(courier);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Courier registered successfully." });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var courier = await _context.Couriers.FirstOrDefaultAsync(c => c.Email == request.Email);
            if(courier is null)
            {
                return Unauthorized("Invalid email or password.");
            }

            var hashingKey = Encoding.UTF8.GetBytes(_configuration["HashingKey"]!);
            using var hmac = new HMACSHA256(hashingKey);
            var computedHash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(request.Password)));

            if (computedHash != courier.PasswordHash)
            {
                return Unauthorized("Invalid email or password.");
            }

            return Ok(new { message = "Login successful." });
        }

        [HttpGet("orders")]
        public IActionResult GetAssignedOrders()
        {
            return Ok(new {message = "List of assigned orders." });
        }

        [HttpPut("orders/{id}/status")]
        public IActionResult UpdateOrderStatus(int id, [FromBody] string status)
        {
            return Ok(new { message = "Order status updated.", orderId = id, status });
        }
    }
}
