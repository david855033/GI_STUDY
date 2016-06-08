using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GI_STUDY
{
    static class SampleRandomizely
    {
        public static string sampleTheMatchList(List<RowMatchPool> RowMatchPools, int pairCount)
        {
            StringBuilder result = new StringBuilder();
            int conut = 0;
            foreach (RowMatchPool matchPool in RowMatchPools)
            {
                conut++;
                int primaryRowCount = matchPool.primaryRows.Count;
                int targetmMatchedRowCount = primaryRowCount * pairCount;
                
                List<string[]> newMatchedRows = new List<string[]>();

                while (newMatchedRows.Count < targetmMatchedRowCount && matchPool.matchedRows.Count > 0)
                {
                    moveOneRandomly(matchPool.matchedRows, newMatchedRows);
                }
                if (newMatchedRows.Count < targetmMatchedRowCount)
                {
                    result.AppendLine($"Match Group {conut} has only {newMatchedRows.Count} matched data.");
                }
                matchPool.matchedRows = newMatchedRows;
            }
            return result.ToString();
        }

        static void moveOneRandomly(List<string[]> inputRows, List<string[]> targetRows)
        {
            Random random = new Random();
            int randomIndex = random.Next(inputRows.Count());
            targetRows.Add(inputRows[randomIndex]);
            inputRows.RemoveAt(randomIndex);
        }
    }
}
