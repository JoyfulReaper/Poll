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


using DemoPollApp.Models;
using System;
using System.Threading.Tasks;
using System.Linq;

namespace DemoPollApp.Helpers
{
    public class VoteHelper
    {
        public static async Task VoteOnPollOnConsole(string context, string username)
        {
            Console.Write("Poll to vote on: ");
            var pollName = Console.ReadLine();

            Poll poll = await APIHelper.GetPoll(pollName, context);

            Console.WriteLine();
            Console.Write("The question is: ");
            ConsoleHelper.ColorWriteLine(ConsoleColor.Green, poll.Question);

            Console.WriteLine("Your options are:");
            foreach(var o in poll.Options)
            {
                ConsoleHelper.ColorWriteLine(ConsoleColor.Green, o);
            }

            var validOption = true;
            Vote vote = new Vote();
            vote.UserName = username;
            vote.Context = context;

            do {
                validOption = true;
                Console.WriteLine();
                Console.Write("Select an option: ");
                var option = Console.ReadLine();

                var valid = poll.Options.Any(x => string.Equals(option, x, StringComparison.OrdinalIgnoreCase));

                if (!valid)
                {
                    ConsoleHelper.ColorWriteLine(ConsoleColor.Red, "Please select a valid option");
                    validOption = false;
                }
                vote.Option = option;
            } while (!validOption);
            vote.PollName = poll.Name;

            Console.WriteLine();
            ConsoleHelper.ColorWriteLine(ConsoleColor.Yellow, $"Status code {await APIHelper.VoteOnPoll(vote)}");
            Console.WriteLine();
        }
    }
}
