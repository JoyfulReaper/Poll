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

using Microsoft.EntityFrameworkCore;
using PollLibrary.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace PollLibrary.DataAccess
{
    public class PollData : IPollData
    {
        private readonly PollContext dbContext;
        private readonly IContextData contextData;

        public PollData(PollContext dbContext, IContextData contextData)
        {
            this.dbContext = dbContext;
            this.contextData = contextData;
        }

        public async Task AddPoll(Poll poll)
        {
            await dbContext.AddAsync(poll);
            await dbContext.SaveChangesAsync();
        }

        public async Task RemovePoll(Poll poll)
        {
            dbContext.Remove(poll);
            await dbContext.SaveChangesAsync();
        }

        public async Task AddVote(Poll poll, Vote vote)
        {
            poll.Votes.Add(vote);
            await dbContext.SaveChangesAsync();
        }

        public async Task<Poll> GetPollById(int id)
        {
            var res =  await dbContext.Polls
                .Include(x => x.Options)
                .Include(x => x.Votes)
                .Include(x => x.Context)
                .SingleOrDefaultAsync(x => x.Id == id);

            return res;
        }

        public async Task<List<Poll>> GetAllPolls()
        {
            var res = await dbContext.Polls
                .Include(x => x.Options)
                .Include(x => x.Votes)
                .Include(x => x.Context)
                .ToListAsync();

            return res;
        }

        public async Task<Poll> GetPollByName(string name)
        {
            return await dbContext.Polls
                .Include(x => x.Options)
                .Include(x => x.Votes)
                .SingleOrDefaultAsync(x => x.Name == name);
        }

        public async Task<List<Poll>> GetPollsByContext(string context)
        {
            var ctx = await contextData.GetContext(context);

            if(ctx == null)
            {
                throw new ArgumentException("Context is not valid", nameof(context));
            }

            return await dbContext.Polls
                .Include(x => x.Options)
                .Include(x => x.Votes)
                .Where(x => x.Context == ctx)
                .ToListAsync();
        }
    }
}
