using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BugTrackerApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BugTrackerApi.Controllers
{
    [Route("api/Bug")]
    [ApiController]
    public class BugController : ControllerBase
    {
        private readonly BugTrackerContext _context;

        public BugController(BugTrackerContext context)
        {
            _context = context;

            _context.Bugs.Include(b => b.AssignedToUser);

            if (_context.Bugs.Count() == 0)
            {
                // Create test data here. TODO: Wire up to db later.
                _context.Users.Add(new User { Id = 1, Name = "James Radley" });
                _context.Users.Add(new User { Id = 2, Name = "Jane Doe" });
                _context.Users.Add(new User { Id = 3, Name = "Joe Bloggs" });
                _context.SaveChanges();
            }

            if (_context.Bugs.Count() == 0)
            {
                // Create test data here. TODO: Wire up to db later.
                _context.Bugs.Add(new Bug { Id = 1, Title = "A bug", Description = "This is a bug", DateOpened = new DateTime(2018, 03, 01), IsActive = true, AssignedToUserId = 1 });
                _context.Bugs.Add(new Bug { Id = 2, Title = "Another bug", Description = "Here is another bug", DateOpened = new DateTime(2018, 05, 14), IsActive = true, AssignedToUserId = 1 });
                _context.Bugs.Add(new Bug { Id = 3, Title = "Yet Another bug", Description = "Here is yet another bug", DateOpened = new DateTime(2018, 06, 01), IsActive = true, AssignedToUserId = 2 });
                _context.Bugs.Add(new Bug { Id = 4, Title = "And Finally Another bug", Description = "Here is finally another bug", DateOpened = new DateTime(2018, 08, 17), IsActive = true, AssignedToUserId = 3 });
                _context.SaveChanges();
            }
        }

        // https://localhost:5001/api/bug   
        [HttpGet]
        public ActionResult<IEnumerable<Bug>> GetAll()
        {
            var bugs = _context.Bugs.Where(b => b.IsActive).Include(b => b.AssignedToUser).ToList();
            return bugs;
        }

        // https://localhost:5001/api/bug/1
        [HttpGet("{id}", Name = "GetBug")]
        public ActionResult<Bug> Get(int id)
        {
            var bug = _context.Bugs.Include(b => b.AssignedToUser).FirstOrDefault(b => b.Id == id && b.IsActive);

            if (bug == null)
            {
                return NotFound();
            }

            return bug;
        }

        [HttpPost]
        public IActionResult Create(Bug item)
        {
            _context.Bugs.Add(item);
            _context.SaveChanges();

            return CreatedAtRoute("GetBug", new { id = item.Id }, item);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public IActionResult Update(int id, Bug item)
        {
            var bug = _context.Bugs.Include(b => b.AssignedToUser).FirstOrDefault(b => b.Id == id && b.IsActive);
           
            if (bug == null)
            {
                return NotFound();
            }

            bug.Title = item.Title;
            bug.Description = item.Description;
            bug.DateOpened = item.DateOpened;
            bug.AssignedToUserId = item.AssignedToUserId;
            bug.IsActive = item.IsActive;

            _context.Bugs.Update(bug);
            _context.SaveChanges();

            return NoContent();
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var item = _context.Bugs.Find(id);
            if (item == null || item.IsActive == false)
            {
                return NotFound();
            }

            item.IsActive = false;
            _context.Bugs.Update(item);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
