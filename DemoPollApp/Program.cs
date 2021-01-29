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

/*
 * This program is intended to be a demo of using a WebAPI, not an example of
 * a proper production application.
 */

using DemoPollApp.Helpers;
using System;
using System.Threading.Tasks;

namespace DemoPollApp
{
    public enum Option { Quit, ChangeUser, ListPolls, CreatePoll, VoteOnPoll, PollResults, DeletePoll, Invalid };

    class Program
    {
        // Configuration Options
        private static string context = "testing"; // Set the poll context 

        private static string userName = "testUser";

        static async Task Main(string[] args)
        {
            while (true)
            {
                ConsoleHelper.ColorWrite(ConsoleColor.Cyan, "Poll API Demo Application");
                ConsoleHelper.ColorWriteLine(ConsoleColor.Blue, " - Copyright 2021 Kyle Givler - https://github.com/JoyfulReaper");
                Console.Write("Current username: ");
                ConsoleHelper.ColorWriteLine(ConsoleColor.Yellow, userName);
                Console.WriteLine();
                Console.WriteLine("0.) Quit");
                Console.WriteLine("1.) Change username");
                Console.WriteLine("2.) List Polls");
                Console.WriteLine("3.) Create Poll");
                Console.WriteLine("4.) Vote on Poll");
                Console.WriteLine("5.) Poll Results");
                Console.WriteLine("6.) Delete Poll");

                Console.WriteLine();
                Console.Write("Option: ");

                var optionValid = int.TryParse(Console.ReadLine(), out int option );
                {
                    if(!optionValid)
                    {
                        Console.WriteLine();
                        ConsoleHelper.ColorWriteLine(ConsoleColor.Red,"Option is not valid!");
                        Console.WriteLine();
                        
                        continue;
                    }
                }

                if(!await PerformSelection((Option)option))
                {
                    Console.WriteLine();
                    ConsoleHelper.ColorWriteLine(ConsoleColor.Red, "Please select a valid option!");
                    Console.WriteLine();
                }
            }
        }

        private async static Task<bool> PerformSelection(Option option)
        {
            try
            {
                switch (option)
                {
                    case Option.Quit:
                        {
                            System.Environment.Exit(0);
                            break;
                        }
                    case Option.ChangeUser:
                        {
                            ChangeUserName();
                            break;
                        }
                    case Option.CreatePoll:
                        {
                            await PollHelper.CreatePollOnConsole(context, userName);
                            break;
                        }
                    case Option.ListPolls:
                        {
                            await PollHelper.ListPollsOnConsole(context);
                            break;
                        }
                    case Option.PollResults:
                        {
                            await ResultHelper.GetResultsOnConsole(context);
                            break;
                        }
                    case Option.VoteOnPoll:
                        {
                            await VoteHelper.VoteOnPollOnConsole(context, userName);
                            break;
                        }
                    case Option.DeletePoll:
                        {
                            await PollHelper.DeletePollOnConsole(context);
                            break;
                        }
                    default:
                        {
                            return false;
                        }

                }
            }
            catch(Exception e)
            {
                Console.WriteLine();
                ConsoleHelper.ColorWriteLine(ConsoleColor.Red, "An Exception Occured: ");
                ConsoleHelper.ColorWriteLine(ConsoleColor.Yellow, e.Message);
                Console.WriteLine();
            }

            return true;
        }

        private static void ChangeUserName()
        {
            Console.WriteLine("Enter new username: ");
            userName = Console.ReadLine();

            Console.WriteLine();
            Console.WriteLine();
        }

    }
}
