using Microsoft.EntityFrameworkCore;
using PollLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PollLibrary.DataAccess
{
    public class ContextData : IContextData
    {
        private readonly PollContext dbContext;

        public ContextData(PollContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Context> GetContext(string name)
        {
            return await dbContext.Context.SingleOrDefaultAsync(x => x.Name == name);
        }
    }
}
