﻿/*
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
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace DemoPollApp.Helpers
{
    public static class PollHelper
    {
        public async static Task ListPollsOnConsole(string context)
        {
            var polls = await  APIHelper.GetAllPolls(context);

            Console.WriteLine();
            foreach(var p in polls)
            {
                ConsoleHelper.ColorWrite(ConsoleColor.Green, $"Poll Name: ");
                Console.WriteLine($"{p.Name}");

                ConsoleHelper.ColorWrite(ConsoleColor.Green, $"Poll Question: ");
                Console.WriteLine($"{p.Question}");

                ConsoleHelper.ColorWrite(ConsoleColor.Green, $"Options: ");
                StringBuilder sb = new StringBuilder();
                foreach (var o in p.Options)
                {
                    sb.Append($"{o}, ");
                }
                sb.Remove(sb.Length - 2, 2);
                Console.WriteLine(sb);
                Console.WriteLine();
            }
        }

        public async static Task CreatePollFromConsoleInput(string context, string userName)
        {
            Poll poll = new Poll();

            Console.WriteLine();
            Console.Write("Poll Name: ");
            poll.Name = Console.ReadLine();

            Console.Write("Poll Question: ");
            poll.Question = Console.ReadLine();

            var moreOptions = true;
            while(moreOptions)
            {
                Console.Write("Poll Option: ");
                poll.Options.Add(Console.ReadLine());

                Console.WriteLine();
                Console.Write("Another Option (y/N): ");
                var another = Console.ReadLine();

                moreOptions = another.ToUpper().StartsWith('Y');
            }

            Console.Write("Create poll? (y/N): ");
            var create = Console.ReadLine().ToUpper().StartsWith('Y');

            Console.WriteLine();
            if (create)
            {
                ConsoleHelper.ColorWriteLine(ConsoleColor.Yellow, $"Response Status Code: {await APIHelper.CreatePoll(poll, context, userName)}");
            }
            else
            {
                ConsoleHelper.ColorWriteLine(ConsoleColor.Red, "Not creating poll.");
            }

            Console.WriteLine();
        }
    }
}
