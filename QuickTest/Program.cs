using Microsoft.EntityFrameworkCore;
using PollLibrary.DataAccess;
using PollLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //MainAsync().GetAwaiter().GetResult();
        }

        private static async Task MainAsync()
        {
        //    DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder();
        //    optionsBuilder.UseSqlServer("Server=RyzenShine;Database=Poll;Trusted_Connection=True;");

        //    using (var context = new PollContext(optionsBuilder.Options))
        //    {
        //        PollData data = new PollData(context);

        //        await LibAdd(data);
        //        //await LibRemove(data);

        //        //var test = await data.GetAllPolls();
        //        //var test2 = await data.GetPollById(1);

        //        Console.WriteLine("Hello World!");
        //    }
        //}

        //private static async Task LibAdd(PollData data)
        //{
        //    Poll newPoll = new Poll()
        //    {
        //        Name = "Another Test Poll2",
        //        Options = GetDemoOptions(),
        //        Votes = GetDemoVotes(),
        //        Context = new Context { Name = "Testing" }
        //    };

        //    await data.AddPoll(newPoll);
        //}

        //private static async Task LibRemove(PollData data)
        //{
        //    Poll poll = new Poll()
        //    {
        //        Id = 1,
        //        Name = "Test Poll",
        //        Options = GetDemoOptions(),
        //        Votes = GetDemoVotes(),
        //        Context = new Context { Name = "Testing" }
        //    };

        //    await data.RemovePoll(poll);
        //}

        //private static List<Vote> GetDemoVotes()
        //{
        //    List<Vote> votes = new List<Vote>();
        //    votes.Add(new Vote
        //    {
        //        User = new User { UserName = "TestUser"},
        //        Option = new Option { Name = "Option 1" }
        //    });

        //    return votes;
        //}

        //private static List<Option> GetDemoOptions()
        //{
        //    List<Option> options = new List<Option>();
        //    options.Add(new Option
        //    {
        //        Name = "Option 1"
        //    });
        //    options.Add(new Option
        //    {
        //        Name = "Option 2"
        //    });

        //    return options;
        //}

        //private static void AddToDbTest(DbContextOptionsBuilder optionsBuilder)
        //{
        //    using (var context = new PollContext(optionsBuilder.Options))
        //    {
        //        Poll poll = new Poll
        //        {
        //            Name = "test",
        //            Options = GetDemoOptions(),
        //            Votes = GetDemoVotes(),
        //            Context = new Context { Name = "TESTING" }
        //        };

        //        context.Add(poll);
        //        context.SaveChanges();

        //        Poll pollfromDb = context.Polls.Include(a => a.Options).Include(a => a.Votes).Where(x => x.Id == 1).FirstOrDefault();

        //        Console.WriteLine(pollfromDb.Name);
        //    }
        }
    }
}
