using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOSEtestClient.Models
{
    public class SubmitModel
    {
        public string username { get; set; }
        public string dateCreated { get; set; }
        public string submisionDate { get; set; }
        public AnswerModel[] answers { get; set; }
        public int userId { get; set; }
    }
}
