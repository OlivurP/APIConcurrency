using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APIConcurrency.Data;
using RazorConcurrency.Models;

namespace APIConcurrency.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        public Order Order { get; set; }
        public static List<Seat> SeatList { get; set; }

        private readonly APIConcurrencyContext _context;

        public OrdersController(APIConcurrencyContext context)
        {
            _context = context;
        }

        // GET: api/Orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrder()
        {
          if (_context.Order == null)
          {
              return NotFound();
          }
            return await _context.Order.ToListAsync();
        }

        // GET: api/Orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
          if (_context.Order == null)
          {
              return NotFound();
          }
            var order = await _context.Order.FindAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            return order;
        }

        //GET: Create
        [HttpGet("Create/{id}")]
        public async Task<IActionResult> Create(int id)
        {
            Order = new Order();
            SeatList = _context.Seat.ToList();
            _context.Order.Add(Order);
            Order.Movie = await _context.Movie.FindAsync(id);
            Order.Movie.Seats = _context.Seat.ToList();
            return Ok(Order); // Return a 200 OK response with the order data
        }

        //POST: Create
        [HttpPost("Create/{seat}")]
        public async Task<ActionResult> Create(string seat)
        {
            Order order = new Order();
            if (ModelState.IsValid)
            {
                try
                {
                    foreach (Seat s in _context.Seat.ToList())
                    {
                        if (seat == s.Name)
                        {
                            order.Seat = s;
                            order.Seat.Taken = true;
                            break;
                        }
                    }
                    foreach (Seat s in SeatList)
                    {
                        if (s.Name == order.Seat.Name)
                        {
                            _context.Entry(order.Seat).Property(x => x.RowVersion).OriginalValue = s.RowVersion;
                        }
                    }
                    _context.Update(order.Seat);
                    _context.Entry(order.Seat).Property(x => x.RowVersion).IsModified = false;
                    _context.Update(order);
                    _context.Entry(order).Property(x => x.RowVersion).OriginalValue = order.RowVersion;
                    _context.Entry(order).Property(x => x.RowVersion).IsModified = false;
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.Id))
                    {
                        return NotFound(); // Return a 404 Not Found response
                    }
                    else
                    {
                        return BadRequest(); // Return a 400 Bad Request response
                    }
                }
                return Ok(order); // Return a 200 OK response with the order data
            }
            return BadRequest(ModelState); // Return a 400 Bad Request response with the model state errors
        }


        // PUT: api/Orders/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(int id, Order order)
        {
            if (id != order.Id)
            {
                return BadRequest();
            }

            _context.Entry(order).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        private bool OrderExists(int id)
        {
            return (_context.Order?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
