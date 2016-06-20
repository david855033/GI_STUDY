using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GI_STUDY
{
    static class SummarizeMatchPools
    {
        public static string printCount(List<RowMatchPool> rowMatchPools, DataMatcher dataMatcher)
        {
            StringBuilder result = new StringBuilder();
            result.AppendLine("\tFemale\t\tMale\t\tTotal\t\t");
            result.AppendLine("AgeGrp\tPrimary\tMatch\tPrimary\tMatch\tPrimary\tMatch");


            for (int i = 0; i < rowMatchPools.Count / 2; i++)
            {
                string line = (i + 1).ToString();
                int primaryFemaleCount = rowMatchPools[i * 2].primaryRows.Count;
                int matchFemaleCount = rowMatchPools[i * 2].matchedRows.Count;
                int primaryMaleCount = rowMatchPools[i * 2 + 1].primaryRows.Count;
                int matchMaleCount = rowMatchPools[i * 2 + 1].matchedRows.Count;
                int primaryTotalCount = primaryFemaleCount + primaryMaleCount;
                int matchTotalCount = matchFemaleCount + matchMaleCount;

                line += "\t" + primaryFemaleCount;
                line += "\t" + matchFemaleCount;
                line += "\t" + primaryMaleCount;
                line += "\t" + matchMaleCount;
                line += "\t" + primaryTotalCount;
                line += "\t" + matchTotalCount;
                result.AppendLine(line);
            }
            return result.ToString();
        }
    }
}
