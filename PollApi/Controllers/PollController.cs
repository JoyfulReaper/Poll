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
        public async Task<ActionResult<IEnumerable<PollDTO>>> GetAll([FromQuery] string context)
        {
            if (!await contextData.IsValidContext(context))
            {
                return Unauthorized();
            }

            var polls = await pollData.GetPollsByContext(context);
            return polls.Select(x => mapper.Map<Poll, PollDTO>(x)).ToList();
        }


        /// <summary>
        /// Retreive a poll by name
        /// </summary>
        /// <param name="context">The poll's context</param>
        /// <param name="name">The name of the poll</param>
        /// <returns></returns>
        [HttpGet("{name}")]
        public async Task<ActionResult<PollDTO>> GetByName([FromQuery]string context, string name)
        {
            if (!await contextData.IsValidContext(context))
            {
                return Unauthorized();
            }

            var poll = await pollData.GetPollByName(name, context);
            if(poll == null)
            {
                return NotFound();
            }

            if(poll.Context.Name != context)
            {
                return Unauthorized();
            }

            return mapper.Map<Poll, PollDTO>(poll);
        }

        // GET api/<PollController>/5 (X)
        [HttpGet("{id:int}")]
        public async Task<ActionResult<PollDTO>> GetById(int id, [FromQuery]string context)
        {
            if (!await contextData.IsValidContext(context))
            {
                return Unauthorized();
            }

            var poll = await pollData.GetPollById(id, context);
            if (poll == null)
            {
                return NotFound();
            }

            if (poll.Context.Name != context)
            {
                return Unauthorized();
            }

            return mapper.Map<Poll, PollDTO>(poll);
        }

        // POST api/<PollController> (X)
        [HttpPost]
        public async Task<ActionResult<PollDTO>> Post([FromBody] PollDTO poll, [FromQuery] string context, [FromQuery] string username)
        {
            var contextDB = await contextData.GetContext(context);
            if (contextDB == null || username == null)
            {
                return Unauthorized();
            }

            var options = new List<Option>();
            foreach (var option in poll.Options)
            {
                options.Add(new Option { Name = option });
            }

            var user = await userData.GetUser(username);
            if(user == null)
            {
                user = new User()
                { UserName = username };
            }

            var newPoll = new Poll()
            {
                Name = poll.Name,
                Context = contextDB,
                Options = options,
                Votes = null,
                CreatingUser = user
            };

            await pollData.AddPoll(newPoll);

            return CreatedAtAction(nameof(GetAll), mapper.Map<Poll, PollDTO>(newPoll));
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
                return Unauthorized();
            }

            var poll = await pollData.GetPollById(id,context);
            if (poll == null)
            {
                return NotFound();
            }

            await pollData.RemovePoll(poll);

            return Accepted();
        }
    }
}