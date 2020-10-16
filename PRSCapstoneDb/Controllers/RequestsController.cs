using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PRSCapstoneDb.Data;
using PRSCapstoneDb.Models;

namespace PRSCapstoneDb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestsController : ControllerBase
    {
        private readonly Context _context;

        public RequestsController(Context context)
        {
            _context = context;
        }

        // GET: api/Requests
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Request>>> GetRequest()
        {
            return await _context.Requests.ToListAsync();
        }

        // GET: api/Requests/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Request>> GetRequest(int id)
        {
            var request = await _context.Requests.FindAsync(id);

            if (request == null)
            {
                return NotFound();
            }

            return request;
        }

        // PUT: api/Requests/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRequest(int id, Request request)
        {
            if (id != request.Id)
            {
                return BadRequest();
            }

            _context.Entry(request).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RequestExists(id))
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

        // POST: api/Requests
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Request>> PostRequest(Request request)
        {
            _context.Requests.Add(request);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRequest", new { id = request.Id }, request);
        }

        // DELETE: api/Requests/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Request>> DeleteRequest(int id)
        {
            var request = await _context.Requests.FindAsync(id);
            if (request == null)
            {
                return NotFound();
            }

            _context.Requests.Remove(request);
            await _context.SaveChangesAsync();

            return request;
        }

        private bool RequestExists(int id)
        {
            return _context.Requests.Any(e => e.Id == id);
        }

        [HttpPut("{total}")]
        public async Task<ActionResult> RecalculateRequestTotal(int Id)
        {
            var request = _context.Requests.Find(Id);
            var reqTotal = (from rl in _context.RequestLines.ToList()
                            join pr in _context.Products.ToList()
                            on rl.ProductId equals pr.Id
                            where rl.RequestId == Id
                            select new
                            {
                                LineTotal = rl.Quantity * pr.Price
                            }).Sum(t => t.LineTotal);
            request.Total = reqTotal;
            return await _context.SaveChangesAsync();
           
        }

        [HttpGet("{toreview}")]
        public async Task<ActionResult> RequestSetToReview(int id, Request request)
        {
            request.Status = request.Total <= 50 ? "APPROVED" : "REVIEW";
            return (ActionResult) await PutRequest(id, request);
        }

        [HttpGet("{review}")]
        public async Task<ActionResult> GetRequestsInReview(Request request)
        {
            return await _context.Requests.Where(r => r.Status == "REVIEW").ToList();
            
        }

        [HttpPut("{rejected}")]
        public async Task<IActionResult> SetToRejected(int id, Request request)
        {
            request.Status = "REJECTED";
            return await PutRequest(id, request);
              
        }

        [HttpPut("{approved}")]
        public async Task<IActionResult> RequestSetToApproved(int id, Request request)      
        {
            request.Status = "APPROVED";
            return await PutRequest(id, request);
        }
    }
}
