﻿using System;
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
            return await _context.Requests.Include(u => u.User).ToListAsync();
        }

        // GET: api/Requests/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Request>> GetRequest(int id)
        {
            //var request = await _context.Requests.FindAsync(id);

            var request = await _context.Requests
                                                .Include(u => u.User)
                                                .Include(u => u.RequestLines)
                                                .ThenInclude(u => u.Product)
                                                .SingleOrDefaultAsync(u => u.Id == id);


            if (request == null)
            {
                return NotFound();
            }

            return request;
        }

        // PUT: api/Requests/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]//this id is the id passed in from the user on the web
        public async Task<IActionResult> PutRequest(int id, Request request)//this id is the id being updated by the user
        {
            //both ids have to match, so the user doesn't update the wrong id
            if (id != request.Id)
            {
                return BadRequest();
            }

            //the changes are stored in the cache
            _context.Entry(request).State = EntityState.Modified;

            //if two users are updating at the same time, this catches the mistake and tells the caller
            //that they didn't do anything wrong, but another user is changing the system
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

            //this method checks for errors, so we don't need to return anything
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

        //the red text gets passed in after localhost:xxxxx/controller/
        [HttpPut("review/{id}")]
        public async Task<IActionResult> RequestSetToReview(int id, Request request)
        {
            request.Status = request.Total <= 50 ? "APPROVED" : "REVIEW";
            return await PutRequest(id, request);
        }

        [HttpGet("review/{Userid}")]
        public async Task<ActionResult<IEnumerable<Request>>> GetRequestsInReview(int Userid, Request request)
        {
            return await _context.Requests.Include(u => u.User).Where(r => r.Status == "REVIEW" && r.Id != Userid).ToListAsync();
          
            
        }
        
        [HttpPut("rejected/{id}")]
        public async Task<IActionResult> SetToRejected(int id, Request request)
        {
            request.Status = "REJECTED";
            return await PutRequest(id, request);
              
        }

        [HttpPut("approved/{id}")]
        public async Task<IActionResult> RequestSetToApproved(int id, Request request)      
        {
            request.Status = "APPROVED";
            return await PutRequest(id, request);
        }


    }
}
