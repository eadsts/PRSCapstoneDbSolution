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
    public class RequestLinesController : ControllerBase
    {
        private readonly Context _context;

        public RequestLinesController(Context context)
        {
            _context = context;
        }
        //don't need this because we are calling it from the web
        // [HttpPut("{total}")]
        //this don't not have to be async it can be private
        private async Task<ActionResult> RecalculateRequestTotal(int Id)
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
            await _context.SaveChangesAsync();
            return Ok();
           

        }

        // GET: api/RequestLines
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RequestLine>>> GetRequestLine()
        {
            return await _context.RequestLines.ToListAsync();
        }

        // GET: api/RequestLines/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RequestLine>> GetRequestLine(int id)
        {
            var requestLine = await _context.RequestLines.FindAsync(id);

            if (requestLine == null)
            {
                return NotFound();
            }

            return requestLine;
        }

        // PUT: api/RequestLines/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRequestLine(int id, RequestLine requestLine)
        {
            if (id != requestLine.Id)
            {
                return BadRequest();
            }

            _context.Entry(requestLine).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RequestLineExists(id))
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

        // POST: api/RequestLines
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<RequestLine>> PostRequestLine(RequestLine requestLine)
        {
            _context.RequestLines.Add(requestLine);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRequestLine", new { id = requestLine.Id }, requestLine);
        }

        // DELETE: api/RequestLines/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<RequestLine>> DeleteRequestLine(int id)
        {
            var requestLine = await _context.RequestLines.FindAsync(id);
            if (requestLine == null)
            {
                return NotFound();
            }

            _context.RequestLines.Remove(requestLine);
            await _context.SaveChangesAsync();

            return requestLine;
        }

        private bool RequestLineExists(int id)
        {
            return _context.RequestLines.Any(e => e.Id == id);
        }
    }
}
