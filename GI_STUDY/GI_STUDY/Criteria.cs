using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GI_STUDY
{
    class Criteria
    {
        static public Dictionary<string, int> index = new Dictionary<string, int>();
        static int getIndex(string lookUpString)
        {
            return index[lookUpString];
        }
        public int fieldIndex;
        public string shouldEqual;
        public Criteria(string field, string shouldEqual)
        {
            this.fieldIndex = getIndex(field);
            this.shouldEqual = shouldEqual;
        }
    }


}
