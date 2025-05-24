using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Dev.Data;
using Dev.Dtos;
using Dev.Models;
using System.Threading.Tasks;
using System.Linq;

namespace Dev.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VendorController : ControllerBase
    {
        private readonly SmartCampusContext _context;

        public VendorController(SmartCampusContext context)
        {
            _context = context;
        }

        // POST: api/Vendor/register
        [HttpPost("register")]
        public async Task<IActionResult> RegisterVendor([FromBody] VendorRegistrationDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Check if email already used in Users table (to avoid duplicates)
            if (await _context.Users.AnyAsync(u => u.Email == dto.Email) ||
                await _context.Vendors.AnyAsync(v => v.Email == dto.Email))
            {
                return Conflict("Email already registered.");
            }

            // Create User record for vendor login
            var user = new User
            {
                UserName = dto.VendorName,
                Email = dto.Email,
                Password = dto.Password,  // Plain text, hash in production
                Department = "Vendor",    // Optional placeholder
                StudentId = System.Guid.NewGuid().ToString()  // Dummy value
            };
            _context.Users.Add(user); // Assuming Students table holds all users
            await _context.SaveChangesAsync();

            // Create Vendor linked to the user
            var vendor = new Vendor
            {
                VendorName = dto.VendorName,
                Email = dto.Email,
                Password = dto.Password,  // Same password here, for vendor auth
                IsOpen = dto.IsOpen,
                Schedule = dto.Schedule,
                distribute_item = dto.DistributeItem,
                UserId = user.UserId
            };
            _context.Vendors.Add(vendor);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Vendor registered successfully.", vendorId = vendor.VendorId });
        }
        // GET: api/Vendor
        [HttpGet]
        public async Task<IActionResult> GetAllVendors()
        {
            var vendors = await _context.Vendors
                .Include(v => v.User) // Optional: include linked User info
                .ToListAsync();

            return Ok(vendors);
        }
        // GET: api/MenuItem/vendor/{vendorId}
        [HttpGet("vendor_allmenue/{vendorId}")]
        public async Task<IActionResult> GetMenuItemsByVendor(int vendorId)
        {
            var menuItems = await _context.MenuItems
                .Where(m => m.VendorId == vendorId && m.Availability)
                .ToListAsync();

            if (menuItems == null || !menuItems.Any())
                return NotFound("No menu items found for this vendor.");

            return Ok(menuItems);
        }

		// Add Menu Item
		[HttpPost("menu_add/")]
		public async Task<ActionResult<MenuItem>> CreateMenuItem(int vendorId, [FromBody] MenuItemCreateDto dto)
		{
			var menuItem = new MenuItem
			{
				VendorId = vendorId,
				ItemName = dto.ItemName,
				Description = dto.Description,
				Price = dto.Price,
				Availability = dto.Availability
			};

			_context.MenuItems.Add(menuItem);
			await _context.SaveChangesAsync();

			return CreatedAtAction(nameof(GetMenuItem), new { vendorId = vendorId, id = menuItem.MenuItemId }, menuItem);
		}

		// Get Menu Item by Id
		[HttpGet("get_menue/{id}")]
		public async Task<ActionResult<MenuItem>> GetMenuItem(int vendorId, int id)
		{
			var menuItem = await _context.MenuItems.FindAsync(id);

			if (menuItem == null || menuItem.VendorId != vendorId)
				return NotFound();

			return menuItem;
		}

		// Edit Menu Item
		[HttpPut("edit_menue/{id}")]
		public async Task<IActionResult> UpdateMenuItem(int vendorId, int id, [FromBody] MenuItemCreateDto dto)
		{
			var menuItem = await _context.MenuItems.FindAsync(id);

			if (menuItem == null || menuItem.VendorId != vendorId)
				return NotFound();

			menuItem.ItemName = dto.ItemName;
			menuItem.Description = dto.Description;
			menuItem.Price = dto.Price;
			menuItem.Availability = dto.Availability;

			await _context.SaveChangesAsync();

			return NoContent();
		}

		// Delete Menu Item
		[HttpDelete("del_menue/{id}")]
		public async Task<IActionResult> DeleteMenuItem(int vendorId, int id)
		{
			var menuItem = await _context.MenuItems.FindAsync(id);

			if (menuItem == null || menuItem.VendorId != vendorId)
				return NotFound();

			_context.MenuItems.Remove(menuItem);
			await _context.SaveChangesAsync();

			return NoContent();
		}
	}
}
