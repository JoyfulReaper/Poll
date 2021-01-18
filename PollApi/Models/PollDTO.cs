using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PollApi.Models
{
    public class PollDTO
    {
        public string Name { get; set; }
        public List<OptionDTO> Options { get; set; }
        public List<VoteDTO> Votes { get; set; }
        public ContextDTO Context { get; set; }
    }
}
