using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOSEtestClient.Models
{
    public class AnswerModel
    {
        public Guid id { get; set; }
        public string optionType { get; set; }
        public string answer { get; set; }
    }
}
