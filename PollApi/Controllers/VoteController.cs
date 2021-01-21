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
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PollApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VoteController : ControllerBase
    {
        private readonly IContextData contextData;
        private readonly IPollData pollData;
        private readonly IVoteData voteData;
        private readonly IUserData userData;
        private readonly Mapper mapper = Mapper.Instance;

        public VoteController(IContextData contextData, 
            IPollData pollData,
            IVoteData voteData,
            IUserData userData)
        {
            this.contextData = contextData;
            this.pollData = pollData;
            this.voteData = voteData;
            this.userData = userData;
        }

        // GET: api/<VoteController>
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET api/<VoteController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<VoteDTO>> GetById(int id, [FromQuery]string context)
        {
            if (!await contextData.IsValidContext(context))
            {
                return Unauthorized();
            }

            var vote = await voteData.GetById(id);
            if (vote == null)
            {
                return NotFound();
            }

            if (vote.Poll.Context.Name != context)
            {
                return Unauthorized();
            }

            return mapper.Map<Vote, VoteDTO>(vote);
        }

        // POST api/<VoteController>
        [HttpPost]
        public async Task<ActionResult<VoteDTO>> Post([FromBody] VoteDTO vote, [FromQuery]string userName, string context)
        {
            var poll = await pollData.GetPollByName(vote.PollName);
            var ctx = await contextData.GetContext(context);

            if (poll == null)
            {
                return NotFound();
            }

            if (ctx == null || poll.Context.Name != ctx.Name || userName == null)
            {
                return Unauthorized();
            }

            var newVote = new Vote()
            {
                User = new User() { UserName = userName },
                Option = new Option() { Name = vote.Option },
                Poll = poll
            };

            var success = await pollData.AddVote(poll, newVote);
            {
                if(success == null)
                {
                    return BadRequest();
                }
            }

            return CreatedAtAction(nameof(GetById), new { id = newVote.Id }, mapper.Map<Vote, VoteDTO>(newVote));
        }

        // PUT api/<VoteController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        // DELETE api/<VoteController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
