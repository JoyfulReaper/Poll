using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PollApi.Models
{
    public class PollDTO
    {
        public string Name { get; set; }
        public ICollection<OptionDTO> Options { get; set; }
        public ICollection<VoteDTO> Votes { get; set; }
        public ContextDTO Context { get; set; }
    }
}
