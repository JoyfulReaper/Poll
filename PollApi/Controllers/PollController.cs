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
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PollDTO>>> Get()
        {
            var polls = await pollData.GetAllPolls();

            return polls.Select(x => mapper.Map<Poll, PollDTO>(x)).ToList();
        }

        // GET api/<PollController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PollDTO>> Get(int id)
        {
            var poll = await pollData.GetPollById(id);

            if(poll == null)
            {
                return NotFound();
            }

            return mapper.Map<Poll, PollDTO>(poll);
        }

        // POST api/<PollController>
        [HttpPost]
        public async Task<ActionResult<PollDTO>> Post([FromBody] PollDTO poll)
        {
            var context = await contextData.GetContext(poll.Context.Name);

            if (context == null)
            {
                return Unauthorized();
            }

            var options = new List<Option>();
            foreach (var option in poll.Options)
            {
                options.Add(new Option { Name = option.Name });
            }

            //var votes = new List<Vote>();
            //if (poll.Votes != null)
            //{
            //    foreach (var v in poll.Votes)
            //    {
            //        Vote vote = new Vote()
            //        {
            //            User = new User { UserName = v.User.UserName },
            //            Option = new Option { Name = v.Option.Name }
            //        };

            //        votes.Add(vote);
            //    }
            //}
            //else
            //{
            //    votes = null;
            //}

            var newPoll = new Poll()
            {
                Name = poll.Name,
                Context = context,
                Options = options,
                Votes = null
            };

            await pollData.AddPoll(newPoll);

            return CreatedAtAction(nameof(Get), mapper.Map< Poll, PollDTO>(newPoll));
        }

        // PUT api/<PollController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<PollController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}