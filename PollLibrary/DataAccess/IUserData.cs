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

using PollLibrary.Models;
using System.Threading.Tasks;

namespace PollLibrary.DataAccess
{
    public interface IUserData
    {
        /// <summary>
        /// Add a user to the database.
        /// </summary>
        /// <param name="userName">The username to add</param>
        /// <returns>The user that was added to the database</returns>
        Task<User> AddUser(string userName);

        /// <summary>
        /// Check to see if the given user is in the database
        /// </summary>
        /// <param name="userName">username to check for validitity</param>
        /// <returns>true if the user exists, false otherwise</returns>
        Task<bool> IsValid(string userName);

        /// <summary>
        /// Remove the given user from the database
        /// </summary>
        /// <param name="userName">The user to remove</param>
        Task RemoveUser(string userName);

        /// <summary>
        /// Reterive a user from the database
        /// </summary>
        /// <param name="userName">the user to reterive</param>
        /// <returns>The requested user or null</returns>
        Task<User> GetUser(string userName);
    }
}