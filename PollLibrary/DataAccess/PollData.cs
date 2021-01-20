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
        private readonly IUserData userData;

        public PollData(PollContext dbContext, 
            IContextData contextData, 
            IUserData userData)
        {
            this.dbContext = dbContext;
            this.contextData = contextData;
            this.userData = userData;
        }

        public async Task AddPoll(Poll poll)
        {
            var hs = new HashSet<string>();
            bool allUnique = poll.Options.All(x => hs.Add(x.Name.ToUpperInvariant()));

            if(!allUnique)
            {
                throw new ArgumentException("Options are not unique");
            }

            await dbContext.AddAsync(poll);
            await dbContext.SaveChangesAsync();
        }

        public async Task RemovePoll(Poll poll)
        {
            dbContext.Remove(poll);
            await dbContext.SaveChangesAsync();
        }

        public async Task<bool> AddVote(Poll poll, Vote vote)
        {
            var userDB = await userData.GetUser(vote.User.UserName);
            var pollDB = await GetPollByName(poll.Name);

            var options = await dbContext.Options
                .Include(x => x.Poll)
                .Where(x => x.Poll.Id == pollDB.Id)
                .ToListAsync();

            var option = options.FirstOrDefault(x => string.Equals(x.Name, vote.Option.Name, StringComparison.OrdinalIgnoreCase));

            if (pollDB == null || option == null)
            {
                return false;
            }

            if(userDB == null)
            {
                userDB = await userData.AddUser(vote.User.UserName);
            }

            var alreadyVoted = await dbContext.Votes.AnyAsync(u => u.Id == userDB.Id && u.Poll.Id == pollDB.Id);
            if (alreadyVoted)
            {
                return false;
            }

            vote.Option = option;
            vote.Poll = pollDB;
            vote.User = userDB;

            poll.Votes.Add(vote);
            await dbContext.AddAsync(vote);
            await dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<Poll> GetPollById(long id)
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
