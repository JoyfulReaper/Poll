using Microsoft.AspNetCore.Mvc;
using PollApi.Helpers;
using PollApi.Models;
using PollLibrary.DataAccess;
using PollLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly Mapper mapper = Mapper.Instance;

        public VoteController(IContextData contextData, IPollData pollData)
        {
            this.contextData = contextData;
            this.pollData = pollData;
        }

        // GET: api/<VoteController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<VoteController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<VoteController>
        [HttpPost]
        public async Task<ActionResult<VoteDTO>> Post([FromBody] Vote vote, string name, string context)
        {
            var poll = await pollData.GetPollByName(name);
            var ctx = await contextData.GetContext(context);

            if (ctx == null)
            {
                return Unauthorized();
            }

            if(poll == null)
            {
                return NotFound();
            }

            var success = await pollData.AddVote(poll, vote);
            {
                if(!success)
                {
                    return BadRequest();
                }
            }

            return CreatedAtAction(nameof(Get), mapper.Map<Vote, VoteDTO>(vote));
        }

        // PUT api/<VoteController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<VoteController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
