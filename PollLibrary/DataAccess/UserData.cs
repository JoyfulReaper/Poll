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
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PollLibrary.DataAccess
{
    public class UserData : IUserData
    {
        private readonly PollContext dbContext;
        private readonly IContextData contextData;

        public UserData(PollContext dbContext, IContextData contextData)
        {
            this.dbContext = dbContext;
            this.contextData = contextData;
        }

        public async Task<User> AddUser(string userName, string context)
        {
            var contextDb = await contextData.GetContext(context);

            if (contextDb == null)
            {
                throw new ArgumentException("Context is not valid", nameof(context));
            }

            var user = await dbContext.AddAsync(new User() 
            { 
                UserName = userName,
                Context = contextDb
            }
            );;
            await dbContext.SaveChangesAsync();

            return user.Entity;
        }

        public async Task<User> GetUser(string userName, string context)
        {
            return await dbContext.Users
                .Where(x => x.Context.Name == context)
                .SingleOrDefaultAsync(x => x.UserName == userName);
        }

        public async Task<bool> IsValid(string userName, string context)
        {
            var user = await GetUser(userName, context);

            return user != null;
        }

        public async Task RemoveUser(string userName, string context)
        {
            var user = await GetUser(userName, context);

            if(user == null)
            {
                throw new ArgumentException("User does not exist", nameof(userName));
            }

            dbContext.Users.Remove(user);
            await dbContext.SaveChangesAsync();
        }
    }
}
