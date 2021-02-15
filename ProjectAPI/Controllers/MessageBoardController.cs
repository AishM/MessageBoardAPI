using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectAPI;
using ProjectAPI.Models;



namespace ProjectAPI.Controllers
{
    //[Authorize(Roles = UserRoles.User)]
    [Authorize(Roles = UserRoles.Admin + "," + UserRoles.User + "," + UserRoles.Moderator)]

   

    [Route("api/[controller]")]
    [ApiController]
    public class MessageBoardController : ControllerBase
    {
        private readonly MessageContext _context;
        //private readonly UserManager<ApplicationUser> userManager;

        public MessageBoardController(MessageContext context)
        {
            _context = context;
        }

        // GET: api/MessageBoards
        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.Moderator)]
        [HttpGet]
        [Route("ViewMessages")]
        public async Task<ActionResult<IEnumerable<MessageBoard>>> GetMessageBoard()
        {
            return await _context.MessageBoard.ToListAsync();
        }

        // GET: api/MessageBoards/5
        
        [HttpGet]
        [Route("ViewMessages/{id}")]
        public async Task<ActionResult<MessageBoard>> GetMessageBoard(long id)
        {
            var messageBoard = await _context.MessageBoard.FindAsync(id);
            System.Security.Claims.ClaimsPrincipal currentUserRole = this.User;

            if (messageBoard == null)
            {
                return NotFound();
            }
            
            return messageBoard;
        }

        // PUT: api/MessageBoards/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[Authorize(Roles = UserRoles.Admin + "," + UserRoles.User)]
        //[HttpPut("{id}")]
        [HttpPut]
        [Route("UpdateMessages/{id}")]
        public async Task<IActionResult> PutMessageBoard(long id, MessageBoard messageBoard)
        {
            System.Security.Claims.ClaimsPrincipal currentUser = this.User;
            bool IsModerator = currentUser.IsInRole("Moderator");

           // var currmsg = await _context.Messages.FindAsync(id);
           
            if (id != messageBoard.Id)
            {
                return BadRequest();
            }


            //  if (messageBoard.flagged != currmsg.flagged && !IsModerator)
            if (messageBoard.Flagged == true && !IsModerator)
            {
                return StatusCode(StatusCodes.Status203NonAuthoritative, new Response { Status = "Error", Message = " Flag modification not allowed" });

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

            return Ok(new Response { Status = "Success", Message = "Record Updated Successfully" }); 
        }

        // POST: api/MessageBoards
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[Authorize(Roles = UserRoles.User)]
        [HttpPost]
        [Route("CreateMessages")]
        public async Task<ActionResult<MessageBoard>> PostMessageBoard(MessageBoard messageBoard)
        {

            // System.Security.Claims.ClaimsPrincipal currentUserRole = this.User.;

            var currentUserRole = User.FindFirstValue(ClaimTypes.Role);
            _context.MessageBoard.Add(messageBoard);

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMessageBoard", new { id = messageBoard.Id, flagged = false, createdby = currentUserRole },messageBoard);
        }

        // DELETE: api/MessageBoards/5s
        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.User)]
        [HttpDelete]
        [Route("DeleteMessages/{id}")]
        public async Task<IActionResult> DeleteMessageBoard(long id)
        {
            var messageBoard = await _context.MessageBoard.FindAsync(id);
            if (messageBoard == null)
            {
                return NotFound();
            }

            _context.MessageBoard.Remove(messageBoard);
            await _context.SaveChangesAsync();

           
            return Ok(new Response { Status = "Success", Message = "Record Deleted Successfully" }); ;
        }

        private bool MessageBoardExists(long id)
        {
            return _context.MessageBoard.Any(e => e.Id == id);
        }


        
       
       
        /* [Authorize(Roles = UserRoles.Moderator)]
        [HttpPut]
        [Route("flagMessages/{id}")]
        
        public async Task<IActionResult> FlagMessageBoard(long id, MessageBoard messageBoard)
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
        */

    }
}
