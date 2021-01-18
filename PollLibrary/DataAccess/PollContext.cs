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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PollLibrary.DataAccess
{
    public partial class PollContext : DbContext
    {
        public PollContext(DbContextOptions options) : base(options)
        {

        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    //TODO Remove after testing
        //    optionsBuilder.UseSqlServer("Server=;Database=Poll;Trusted_Connection=True;");
        //}

        public DbSet<User> Users { get; set; }
        public DbSet<Option> Options { get; set; }
        public DbSet<Vote> Votes { get; set; }
        public DbSet<Poll> Polls { get; set; }
        public DbSet<Context> Context { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Poll>()
                .HasMany(o => o.Options)
                .WithOne(p => p.Poll)
                .HasForeignKey(f => f.PollId);

            modelBuilder.Entity<Poll>()
                .HasMany(v => v.Votes)
                .WithOne(p => p.Poll)
                .HasForeignKey(f => f.PollId);

            modelBuilder.Entity<Poll>()
                .HasOne(c => c.Context)
                .WithMany()
                .HasForeignKey(f => f.ContextId);

            modelBuilder.Entity<Option>()
                .HasOne(p => p.Poll)
                .WithMany(o => Options)
                .HasForeignKey(f => f.PollId);

            modelBuilder.Entity<Vote>()
                .HasOne(u => u.User)
                .WithMany()
                .HasForeignKey(f => f.UserId);

            modelBuilder.Entity<Vote>()
                .HasOne(p => p.Poll)
                .WithMany(v => v.Votes)
                .HasForeignKey(f => f.PollId);

            modelBuilder.Entity<Vote>()
                .HasOne(o => o.Option)
                .WithMany()
                .HasForeignKey(f => f.OptionId);


            //modelBuilder.Entity<Vote>()
            //    .HasOne(p => p.Poll)
            //    .WithMany(v => v.Votes)
            //    .HasForeignKey(f => f.PollId)
            //    .OnDelete(DeleteBehavior.Cascade);

            //modelBuilder.Entity<Option>()
            //    .HasOne(p => p.Poll)
            //    .WithMany(o => o.Options)
            //    .HasForeignKey(f => f.PollId)
            //    .OnDelete(DeleteBehavior.Cascade);

            //modelBuilder.Entity<Vote>()
            //    .HasOne(x => x.User)
            //    .WithMany();

            //modelBuilder.Entity<Vote>()
            //    .HasOne(x => x.Option)
            //    .WithMany();
        }
    }
}
