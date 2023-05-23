using AwesomeDevEvents.API.Data.Persistence;
using AwesomeDevEvents.API.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AwesomeDevEvents.API.Controllers
{
    [Route("api/dev-events")]
    [ApiController]
    public class DevEventsController : ControllerBase
    {
        private readonly DevEventsDbContext _context;
        public DevEventsController(DevEventsDbContext devEventsDbContext)
        {
            _context = devEventsDbContext;
        }

        [HttpGet]
        public IActionResult GetAll() {
            var devEvents = _context.DevEvents.Where(x => !x.IsDeleted).ToList();

            return Ok(devEvents);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(Guid id) {
            var devEvent = _context.DevEvents.Include(de => de.Speakers).SingleOrDefault(x => x.Id == id);

            if(devEvent == null)
            {
                return NotFound();
            }

            return Ok(devEvent);
        }
        
        [HttpPost]
        public IActionResult Post(DevEvent devEvent) {
            _context.DevEvents.Add(devEvent);

            _context.SaveChanges();

            return CreatedAtAction(nameof(GetById), new { id = devEvent.Id }, devEvent);
        }

        [HttpPut("{id}")]
        public IActionResult Update(Guid id, DevEvent input) { 
            var devEventFound = _context.DevEvents.SingleOrDefault(x => x.Id == id);

            if(devEventFound == null)
            {
                return NotFound();
            }

            devEventFound.Update(input.Title, input.Description, input.StartDate, input.EndDate);

            _context.DevEvents.Update(devEventFound);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var devEventFound = _context.DevEvents.SingleOrDefault(x => x.Id == id);

            if (devEventFound == null)
            {
                return NotFound();
            }

            devEventFound.Delete();

            _context.SaveChanges();

            return NoContent();
        }

        [HttpPost("{id}/speakers")]
        public IActionResult PostSpeaker(Guid id, DevEventSpeaker speaker)
        {
            speaker.DevEventId = id;

            var devEventFound = _context.DevEvents.Any(x => x.Id == id);

            if (!devEventFound)
            {
                return NotFound();
            }

            _context.DevEventsSpeakers.Add(speaker);
            _context.SaveChanges();
                
            return NoContent();
        }
    }
}
