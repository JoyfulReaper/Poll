using Microsoft.AspNetCore.Mvc;
using PollApi.Models;
using PollLibrary.DataAccess;
using PollLibrary.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PollApi.Controllers
{
    [Route("api/Polls")]
    [ApiController]
    public class PollController : ControllerBase
    {
        private readonly IPollData pollData;
        private readonly IUserData userData;

        public PollController(IPollData pollData, IUserData userData)
        {
            this.pollData = pollData;
            this.userData = userData;
        }

        // GET: api/<PollController>
        [HttpGet]
        public async Task<IEnumerable<Poll>> Get()
        {
            var test = await pollData.GetAllPolls();
            return test;
        }

        // GET api/<PollController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<PollController>
        [HttpPost]
        public async Task Post([FromBody] Poll poll)
        {
            await pollData.AddPoll(poll);
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

        private static OptionDTO OptionToDTO(Option option) =>
            new OptionDTO()
            {
                OptionValue = option.OptionValue
            };

        private static ContextDTO ContextToDTO(Context context) =>
            new ContextDTO()
            {
                Name = context.Name
            };

        private static VoteDTO VoteToDTO(Vote vote) =>
            new VoteDTO
            {
                User = vote.User
            };

        private static PollDTO PollToDTO(Poll poll) =>
            new PollDTO()
            {

            };

    }
}