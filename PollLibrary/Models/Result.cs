using System;
using System.Collections.Generic;
using System.Text;

namespace PollLibrary.Models
{
    public class Result
    {
        public string PollName{ get; set; }
        public Dictionary<string, int> Results { get; set; } = new Dictionary<string, int>();
    }
}
