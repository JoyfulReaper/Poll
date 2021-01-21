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

namespace PollLibrary.DataAccess
{
    public partial class PollContext : DbContext
    {
        public PollContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Option> Options { get; set; }
        public DbSet<Vote> Votes { get; set; }
        public DbSet<Poll> Polls { get; set; }
        public DbSet<Context> Context { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Poll>()
                .HasMany(x => x.Options)
                .WithOne(x => x.Poll)
                .IsRequired();

            modelBuilder.Entity<Poll>()
                .HasMany(x => x.Votes)
                .WithOne(x => x.Poll)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Poll>()
                 .HasOne(x => x.Context)
                 .WithMany()
                 .IsRequired();

            modelBuilder.Entity<Poll>()
                .HasOne(x => x.CreatingUser)
                .WithMany()
                .IsRequired();

            modelBuilder.Entity<Option>()
                .HasOne(x => x.Poll)
                .WithMany(x => x.Options)
                .IsRequired();

            modelBuilder.Entity<Vote>()
                .HasOne(x => x.User)
                .WithMany()
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Vote>()
                .HasOne(x => x.Option)
                .WithMany()
                .IsRequired();

            modelBuilder.Entity<Vote>()
                .HasOne(x => x.Poll)
                .WithMany(x => x.Votes)
                .IsRequired();
        }
    }
}
