using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BugTrackerApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace BugTrackerApi.Controllers
{
    [Route("api/User")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly BugTrackerContext _context;

        public UserController(BugTrackerContext context)
        {
            _context = context;

            if (_context.Users.Count() == 0)
            {
                // Create test data here. TODO: Wire up to db later.
                _context.Users.Add(new User { Id = 1, Name = "James Radley" });
                _context.Users.Add(new User { Id = 2, Name = "Jane Doe" });
                _context.Users.Add(new User { Id = 3, Name = "Joe Bloggs" });
                _context.SaveChanges();
            }
        }

        // https://localhost:5001/api/user 
        [HttpGet]
        public ActionResult<IEnumerable<User>> GetAll()
        {
            return _context.Users.ToList();
        }

        // https://localhost:5001/api/user/1
        [HttpGet("{id}", Name = "GetUser")]
        public ActionResult<User> Get(int id)
        {
            var item = _context.Users.Find(id);
            if (item == null)
            {
                return NotFound();
            }

            return item;
        }


        [HttpPost]
        public IActionResult Create(User item)
        {
            _context.Users.Add(item);
            _context.SaveChanges();

            return CreatedAtRoute("GetUser", new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, User item)
        {
            var user = _context.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            user.Name = item.Name;
            
            _context.Users.Update(user);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
