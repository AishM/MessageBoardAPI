using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectAPI;
using ProjectAPI.Models;

namespace ProjectAPI.Controllers
{
    //[Authorize(Roles = UserRoles.User)]
    [Authorize(Roles = UserRoles.Admin + "," + UserRoles.User)]


    [Route("api/[controller]")]
    [ApiController]
    public class MessageBoardController : ControllerBase
    {
        private readonly MessageContext _context;

        public MessageBoardController(MessageContext context)
        {
            _context = context;
        }

        // GET: api/MessageBoards
        [Authorize(Roles = UserRoles.Admin)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MessageBoard>>> GetMessageBoard()
        {
            return await _context.MessageBoard.ToListAsync();
        }

        // GET: api/MessageBoards/5
       // [Authorize(Roles = UserRoles.Admin + "," + UserRoles.User)]
        [HttpGet("{id}")]      
        public async Task<ActionResult<MessageBoard>> GetMessageBoard(long id)
        {
            var messageBoard = await _context.MessageBoard.FindAsync(id);

            if (messageBoard == null)
            {
                return NotFound();
            }

            return messageBoard;
        }

        // PUT: api/MessageBoards/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[Authorize(Roles = UserRoles.Admin + "," + UserRoles.User)]
        [HttpPut("{id}")] 
        public async Task<IActionResult> PutMessageBoard(long id, MessageBoard messageBoard)
        {
            if (id != messageBoard.Id)
            {
                return BadRequest();
            }

            _context.Entry(messageBoard).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MessageBoardExists(id))
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

        // POST: api/MessageBoards
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[Authorize(Roles = UserRoles.User)]
        [HttpPost]
        public async Task<ActionResult<MessageBoard>> PostMessageBoard(MessageBoard messageBoard)
        {
            _context.MessageBoard.Add(messageBoard);
           
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMessageBoard", new { id = messageBoard.Id }, messageBoard);
        }

        // DELETE: api/MessageBoards/5
        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.User)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMessageBoard(long id)
        {
            var messageBoard = await _context.MessageBoard.FindAsync(id);
            if (messageBoard == null)
            {
                return NotFound();
            }

            _context.MessageBoard.Remove(messageBoard);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MessageBoardExists(long id)
        {
            return _context.MessageBoard.Any(e => e.Id == id);
        }
    }
}
