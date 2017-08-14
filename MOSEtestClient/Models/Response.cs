using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOSEtestClient.Models
{
    public class Response
    {
        public string code { get; set; }
        public string msg { get; set; }
        public int resultInt { get; set; }
        public object data { get; set; }
    }
}
