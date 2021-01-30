/*
MIT License

Copyright(c) 2021 Kyle Givler
https://github.com/JoyfulReaper

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

using Microsoft.AspNetCore.Mvc;
using PollApi.Helpers;
using PollApi.Models;
using PollLibrary.DataAccess;
using PollLibrary.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PollApi.Controllers
{
    [Route("api/Polls")]
    [ApiController]
    public class PollController : ControllerBase
    {
        private readonly IPollData pollData;
        private readonly IUserData userData;
        private readonly IContextData contextData;
        private readonly Mapper mapper = Mapper.Instance;

        public PollController(IPollData pollData, IUserData userData, IContextData contextData)
        {
            this.pollData = pollData;
            this.userData = userData;
            this.contextData = contextData;
        }

        // GET: api/<PollController>
        /// <summary>
        /// Get all polls for the given context
        /// </summary>
        /// <param name="context">The poll's context</param>
        /// <returns>All polls in the given contexy</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PollResponseDTO>>> GetAll([FromQuery] string context)
        {
            if (!await contextData.IsValidContext(context))
            {
                return Unauthorized(new ErrorResponse { ErrorMessage = "Context is not valid" });
            }

            var polls = await pollData.GetPollsByContext(context);
            return polls.Select(x => mapper.Map<Poll, PollResponseDTO>(x)).ToList();
        }


        /// <summary>
        /// Retreive a poll by name
        /// </summary>
        /// <param name="context">The poll's context</param>
        /// <param name="name">The name of the poll</param>
        /// <returns></returns>
        [HttpGet("{name}")]
        public async Task<ActionResult<PollResponseDTO>> GetByName([FromQuery]string context, string name)
        {
            if (!await contextData.IsValidContext(context))
            {
                return Unauthorized(new ErrorResponse { ErrorMessage = "Context is not valid" });
            }

            var poll = await pollData.GetPollByName(name, context);
            if(poll == null)
            {
                return NotFound(new ErrorResponse { ErrorMessage = $"Unable to find poll {name}." });
            }

            if(poll.Context.Name != context)
            {
                return Unauthorized(new ErrorResponse { ErrorMessage = $"Context is not valid for {name}" });
            }

            return mapper.Map<Poll, PollResponseDTO>(poll);
        }

        // GET api/<PollController>/5 (X)
        [HttpGet("{id:int}")]
        public async Task<ActionResult<PollResponseDTO>> GetById(int id, [FromQuery]string context)
        {
            if (!await contextData.IsValidContext(context))
            {
                return Unauthorized(new ErrorResponse { ErrorMessage = "Context is not valid" });
            }

            var poll = await pollData.GetPollById(id, context);
            if (poll == null)
            {
                return NotFound(new ErrorResponse { ErrorMessage = $"Unable to find poll with id {id}." });
            }

            if (poll.Context.Name != context)
            {
                return Unauthorized(new ErrorResponse { ErrorMessage = $"Context is not valid for id {id}" });
            }

            return mapper.Map<Poll, PollResponseDTO>(poll);
        }

        // POST api/<PollController> (X)
        [HttpPost]
        public async Task<ActionResult<PollDTO>> Post([FromBody] PollDTO poll)
        {
            if(poll == null || string.IsNullOrEmpty(poll.Name) || string.IsNullOrEmpty(poll.Question) || poll.Options == null)
            {
                BadRequest("Poll failed validation");
            }

            var contextDB = await contextData.GetContext(poll.Context);
            if (contextDB == null || string.IsNullOrEmpty(poll.UserName))
            {
                return Unauthorized(new ErrorResponse { ErrorMessage = $"{(contextDB == null ? "Context" : "Username")} is not valid" });
            }

            var options = new List<Option>();
            foreach (var option in poll.Options)
            {
                options.Add(new Option { Name = option });
            }

            var user = await userData.GetUser(poll.UserName, poll.Context);
            if(user == null)
            {
                user = new User()
                {
                    UserName = poll.UserName,
                    Context = contextDB
                };
            }

            var newPoll = new Poll()
            {
                Name = poll.Name,
                Question = poll.Question,
                Context = contextDB,
                Options = options,
                Votes = null,
                CreatingUser = user
            };

            await pollData.AddPoll(newPoll);

            return CreatedAtAction(nameof(GetAll), mapper.Map<Poll, PollResponseDTO>(newPoll));
        }

        // PUT api/<PollController>/5   (Not used at the moment)
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        // DELETE api/<PollController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, string context)
        {
            if (!await contextData.IsValidContext(context))
            {
                return Unauthorized(new ErrorResponse { ErrorMessage = "Context is not valid" });
            }

            var poll = await pollData.GetPollById(id,context);
            if (poll == null)
            {
                return NotFound(new ErrorResponse { ErrorMessage = $"Unable to find poll with id {id}." });
            }

            await pollData.RemovePoll(poll);

            return Ok(new { Message = $"Successfully deleted {id}" });
        }
    }
}