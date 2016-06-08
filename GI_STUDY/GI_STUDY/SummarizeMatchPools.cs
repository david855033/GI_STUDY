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
            string title = "";
            foreach (string s in dataMatcher.matchKey) { title += s + "\t"; }
            title += "primary\tmatch";
            result.AppendLine(title);
            for(int i = 0; i<rowMatchPools.Count; i++)
            {
                string line = "";
                foreach (var s in dataMatcher.groupContentList[i])
                {
                    line += s + "\t";
                }
                line += rowMatchPools[i].primaryRows.Count() + "\t" + rowMatchPools[i].matchedRows.Count();
                result.AppendLine(line);
            }
            return result.ToString();
        }
    }
}
