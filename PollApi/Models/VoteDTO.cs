using PollLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PollApi.Models
{
    public class VoteDTO
    {
        public User User { get; set; }
        public Option Option { get; set; }
    }
}
