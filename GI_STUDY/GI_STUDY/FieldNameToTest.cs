using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GI_STUDY
{
    class FieldNameToTest
    {
        static public Dictionary<string, int> index = new Dictionary<string, int>();
        static int getIndex(string lookUpString)
        {
            return index[lookUpString];
        }
        public string fieldname, positiveValue, info;
        public int fieldindex;
        public FieldNameToTest(string fieldname, string positiveValue, string info)
        {
            this.fieldname = fieldname;
            this.fieldindex = getIndex(fieldname);
            this.positiveValue = positiveValue;
            this.info = info;
        }
    }
}
