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
using PollApi.Models;
using PollLibrary.DataAccess;
using PollLibrary.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PollApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResultsController : ControllerBase
    {
        private readonly IPollData pollData;
        private readonly IContextData contextData;

        public ResultsController(IPollData pollData, IContextData contextData)
        {
            this.pollData = pollData;
            this.contextData = contextData;
        }

        // GET api/<ResultsController>/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Result>> GetById(int id, [FromQuery]string context)
        {
            if(!await contextData.IsValidContext(context))
            {
                return Unauthorized(new ErrorResponse { ErrorMessage = "Context is not valid" });
            }

            Poll poll = await pollData.GetPollById(id, context);
            if(poll == null)
            {
                return NotFound(new ErrorResponse { ErrorMessage = $"Unable to find poll with id {id}." });
            }

            if(poll.Context.Name != context)
            {
                return Unauthorized(new ErrorResponse { ErrorMessage = $"Context is not valid for id {id}" });
            }

            Result res = await pollData.GetResults(poll);

            return res;
        }

        [HttpGet("{name}")]
        public async Task<ActionResult<Result>> GetByName(string name, [FromQuery]string context)
        {
            if (!await contextData.IsValidContext(context))
            {
                return Unauthorized(new ErrorResponse { ErrorMessage = "Context is not valid" });
            }

            Poll poll = await pollData.GetPollByName(name, context);
            if (poll == null)
            {
                return NotFound(new ErrorResponse { ErrorMessage = $"Unable to find poll {name}." });
            }

            if (poll.Context.Name != context)
            {
                return Unauthorized(new ErrorResponse { ErrorMessage = $"Context is not valid for poll {name}" });
            }

            Result res = await pollData.GetResults(poll);

            return res;
        }
    }
}
