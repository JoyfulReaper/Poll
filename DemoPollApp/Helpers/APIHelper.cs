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
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace DemoPollApp.Helpers
{
    public class APIHelper
    {
        private static readonly HttpClient client = new HttpClient();
        private static readonly string path = "api";

        static APIHelper()
        {
            client.BaseAddress = new Uri("https://localhost:44316");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/ajson"));
        }

        public async static Task<HttpStatusCode> DeletePoll(long id, string context)
        {
            HttpResponseMessage response = await client.DeleteAsync($"{path}/Polls/{id}?context={context}");
            return response.StatusCode;
        }

        public async static Task<Poll> GetPoll(string name, string context)
        {
            Poll poll = null;
            HttpResponseMessage response = await client.GetAsync($"{path}/Polls/{name}?context={context}");

            if (response.IsSuccessStatusCode)
            {
                poll = await response.Content.ReadAsAsync<Poll>();
            }
            else
            {
                throw new Exception($"Unable to retereive poll: {name}");
            }

            return poll;
        }

        public async static Task<List<Poll>> GetAllPolls(string context)
        {
            List<Poll> polls = new List<Poll>();
            HttpResponseMessage response = await client.GetAsync($"{path}/Polls?context={context}");

            if (response.IsSuccessStatusCode)
            {
                polls = await response.Content.ReadAsAsync<List<Poll>>();
            }

            return polls;
        }

        public async static Task<HttpStatusCode> CreatePoll(Poll poll)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync($"{path}/Polls", poll);
            return response.StatusCode;
        }

        public async static Task<Result> GetResult(string pollName, string context)
        {
            Result results = null;

            HttpResponseMessage response = await client.GetAsync($"{path}/Results/{pollName}?context={context}");
            if(response.IsSuccessStatusCode)
            {
                results = await response.Content.ReadAsAsync<Result>();
            }
            else
            {
                throw new Exception($"Unsuccessful getting results for {pollName}");
            }

            return results;
        }

        public async static Task<HttpStatusCode> VoteOnPoll(Vote vote)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync($"{path}/Vote", vote);
            if(!response.IsSuccessStatusCode)
            {
                throw new Exception($"Unable to save vote: {response.StatusCode} Message: {await response.Content.ReadAsStringAsync()}");
            }

            return response.StatusCode;
        }
    }
}
