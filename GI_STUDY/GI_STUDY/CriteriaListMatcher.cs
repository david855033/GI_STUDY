using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GI_STUDY
{
    static class CriteriaListMather
    {
        public static bool matchAnyOfOne(this string[] rowToExam, List<Criteria> toMatch)
        {
            foreach (var currentCriteria in toMatch)
            {
                int fieldIndex = currentCriteria.fieldIndex;
                if (rowToExam[fieldIndex] == currentCriteria.shouldEqual)
                    return true;
            }
            return false;
        }
    }
}
