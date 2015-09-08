using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextFinder
{
    class Result
    {
        public int Line { get; set; }
        public String TextStart { get; set; }
        public String Pattern { get; set;  }
        public String TextEnd { get; set; }
        public int Index { get; set; }
        public int Length { get; set; }
    }
}
