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

// TODO add additional options for reterive polls, such as most recent, or 
// 5 at a time.

using PollLibrary.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PollLibrary.DataAccess
{
    public interface IPollData
    {
        /// <summary>
        /// Add a bew poll to the database
        /// </summary>
        /// <param name="poll">The Poll to add</param>
        /// <returns>The poll that has been added to the database</returns>
        Task<Poll> AddPoll(Poll poll);

        /// <summary>
        /// Vote on a poll option
        /// </summary>
        /// <param name="poll">The Poll to vote on</param>
        /// <param name="vote">The vote to apply to the poll</param>
        /// <returns>The vote that was added to the database</returns>
        Task<Vote> AddVote(Poll poll, Vote vote);

        /// <summary>
        /// Reterive a list of all polls from the database
        /// </summary>
        /// <returns>A list of all polls</returns>
        Task<List<Poll>> GetAllPolls();

        /// <summary>
        /// Reterive a Poll by it's database row id
        /// </summary>
        /// <param name="id">The Poll's ID</param>
        /// <param name="context">The context what the poll belongs to</param>
        /// <returns></returns>
        Task<Poll> GetPollById(long id, string context);

        /// <summary>
        /// Reterive a Poll by name and context
        /// </summary>
        /// <param name="name">The name of the poll</param>
        /// <param name="context">The context that the poll belongs to</param>
        /// <returns></returns>
        Task<Poll> GetPollByName(string name, string context);

        /// <summary>
        /// Reterive a list of all polls in a given context
        /// </summary>
        /// <param name="context">Desired context</param>
        /// <returns>All polls in the given context</returns>
        Task<List<Poll>> GetPollsByContext(string context);

        /// <summary>
        /// Delete a Poll from the database
        /// </summary>
        /// <param name="poll">The Poll to delete</param>
        Task RemovePoll(Poll poll);

        /// <summary>
        /// Get the results of the given poll
        /// </summary>
        /// <param name="poll">The poll for which to get the results</param>
        /// <returns>The results of the given Poll</returns>
        Task<Result> GetResults(Poll poll);
    }
}