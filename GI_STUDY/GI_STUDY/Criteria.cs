using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GI_STUDY
{
    class Criteria
    {
        public int fieldIndex;
        public string shouldEqual;
        public Criteria(int fieldIndex, string shouldEqual)
        {
            this.fieldIndex = fieldIndex;
            this.shouldEqual = shouldEqual;
        }
    }


}
