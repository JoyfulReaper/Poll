using Microsoft.AspNetCore.Mvc;
using PollApi.Models;
using PollLibrary.DataAccess;
using PollLibrary.Models;
using System.Collections.Generic;
using System.Linq;
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
        public async Task<ActionResult<IEnumerable<PollDTO>>> Get()
        {
            var polls = await pollData.GetAllPolls();

            return polls.Select(x => PollToDTO(x)).ToList();
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

            return PollToDTO(poll);
        }

        // POST api/<PollController>
        [HttpPost]
        public async Task Post([FromBody] PollDTO poll)
        {
            var options = new List<Option>();
            foreach (var option in poll.Options)
            {
                options.Add(new Option { Name = option.Name });
            }

            var votes = new List<Vote>();
            if (poll.Votes != null)
            {
                foreach (var v in poll.Votes)
                {
                    Vote vote = new Vote()
                    {
                        User = v.User,
                        Option = v.Option
                    };

                    votes.Add(vote);
                }
            }
            else
            {
                votes = null;
            }

            var newPoll = new Poll();
            newPoll.Name = poll.Name;
            newPoll.Context = new Context { Name = poll.Context.Name };
            newPoll.Options = options;
            newPoll.Votes = votes;

            await pollData.AddPoll(newPoll);
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
                Name = option.Name
            };

        private static ContextDTO ContextToDTO(Context context) =>
            new ContextDTO()
            {
                Name = context.Name
            };

        private static VoteDTO VoteToDTO(Vote vote) =>
            new VoteDTO
            {
                User = vote.User,
                Option = vote.Option
            };

        private static UserDTO UserToDTO(User user) =>
            new UserDTO
            {
                UserName = user.UserName
            };

        private static PollDTO PollToDTO(Poll poll)
        {
            List<OptionDTO> options = new List<OptionDTO>();
            List<VoteDTO> votes = new List<VoteDTO>();

            foreach (var option in poll.Options)
            {
                var temp = OptionToDTO(option);
                options.Add(temp);
            }

            foreach (var vote in poll.Votes)
            {
                var temp = VoteToDTO(vote);
                votes.Add(temp);
            }

            return new PollDTO()
            {
                Name = poll.Name,
                Options = options,
                Votes = votes
            };
        }

    }
}