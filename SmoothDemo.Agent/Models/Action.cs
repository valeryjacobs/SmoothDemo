using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmoothDemo.Agent.Models
{ 
    public class Action
    {
        public string Type { get; set; }
        public object Content { get; set; }
        public bool AutoContinue { get; set; }
        public string Description { get; set; }
        public bool Current { get; set; }
    }
}
