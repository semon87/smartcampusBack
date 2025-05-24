using Microsoft.AspNetCore.Mvc;
using Dev.Data;
using Dev.Dtos;
using Dev.Models;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;


namespace Dev.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly SmartCampusContext _context;

        public StudentController(SmartCampusContext context)
        {
            _context = context;
        }

        // POST: api/Student/register
        [HttpPost("register")]
        public async Task<IActionResult> RegisterStudent([FromBody] StudentRegistrationDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (_context.Users.Any(s => s.Email == dto.Email))
                return Conflict("Email already exists.");

            if (_context.Users.Any(s => s.StudentId == dto.StudentId))
                return Conflict("Student ID already exists.");

            var student = new User
            {
                UserName = dto.UserName,
                Department = dto.Department,
                Address = dto.Address,
                StudentId = dto.StudentId,
                Email = dto.Email,
                Password = dto.Password // store plain for now, but hashing recommended
            };

            _context.Users.Add(student);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Student registered successfully." });
        }
        // GET: api/Student/{studentId}/orders
        [HttpGet("{studentId}/orders")]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrderHistory(int studentId)
        {
            var orders = await _context.Orders
                .Where(o => o.StudentId == studentId)
                .Include(o => o.Vendor)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.MenuItem)
                .ToListAsync();

            if (orders == null || orders.Count == 0)
                return NotFound("No orders found for this student.");

            return Ok(orders);
        }

        // GET: api/Student/order/{orderId}/status
        [HttpGet("order/{orderId}/status")]
        public async Task<ActionResult<string>> GetOrderStatus(int orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);

            if (order == null)
                return NotFound("Order not found.");

            return Ok(order.OrderStatus.ToString());
        }
    }
}
