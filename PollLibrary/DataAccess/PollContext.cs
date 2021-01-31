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
                .HasKey(x => x.Id);

            modelBuilder.Entity<Poll>()
                .Property(x => x.Name).HasMaxLength(20)
                .IsRequired();

            modelBuilder.Entity<Poll>()
                .Property(x => x.Question).HasMaxLength(100)
                .IsRequired();

            modelBuilder.Entity<Poll>()
                .HasMany(x => x.Options)
                .WithOne(x => x.Poll)
                .IsRequired();

            modelBuilder.Entity<Poll>()
                .HasMany(x => x.Votes)
                .WithOne(x => x.Poll);

            modelBuilder.Entity<Poll>()
                 .HasOne(x => x.Context)
                 .WithMany(x => x.Polls)
                 .HasForeignKey(x => x.ContextId)
                 .IsRequired();

            modelBuilder.Entity<Poll>()
                .HasOne(x => x.CreatingUser)
                .WithMany()
                .HasForeignKey(x => x.CreatingUserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

            ////////////////////////////////////////

            modelBuilder.Entity<Option>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<Option>()
                .Property(x => x.Name).HasMaxLength(50);

            modelBuilder.Entity<Option>()
                .HasOne(x => x.Poll)
                .WithMany(x => x.Options)
                .HasForeignKey(x => x.PollId)
                .IsRequired();

            ////////////////////////////////////////

            modelBuilder.Entity<Vote>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<Vote>()
                .HasOne(x => x.User)
                .WithMany(x => x.Votes)
                .IsRequired()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Vote>()
                .HasOne(x => x.Option)
                .WithMany(x => x.Votes)
                .HasForeignKey(x => x.OptionId)
                .IsRequired();

            modelBuilder.Entity<Vote>()
                .HasOne(x => x.Poll)
                .WithMany(x => x.Votes)
                .HasForeignKey(x => x.PollId)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

            ////////////////////////////////////////

            modelBuilder.Entity<User>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<User>()
                .Property(x => x.UserName).HasMaxLength(50);

            modelBuilder.Entity<User>()
                .HasOne(x => x.Context)
                .WithMany(x => x.Users)
                .IsRequired()
                .HasForeignKey(x => x.ContextId);

            modelBuilder.Entity<User>()
                .HasMany(x => x.Votes)
                .WithOne(x => x.User);

            //////////////////////////////////////

            modelBuilder.Entity<Context>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<Context>()
                .HasMany(x => x.Polls)
                .WithOne(x => x.Context);

            modelBuilder.Entity<Context>()
                .HasMany(x => x.Users)
                .WithOne(x => x.Context);

            modelBuilder.Entity<Context>()
                .Property(x => x.Name).HasMaxLength(50);
        }
    }
}
