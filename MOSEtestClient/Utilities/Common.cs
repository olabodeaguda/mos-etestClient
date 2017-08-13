using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOSEtestClient.Utilities
{
    public class Common
    {
        public static List<int> GeneratCount(int size)
        {
            List<int> lst = new List<int>();

            for (int i = 1; i <= size; i++)
            {
                lst.Add(i);
            }

            return lst;
        }
    }
}
