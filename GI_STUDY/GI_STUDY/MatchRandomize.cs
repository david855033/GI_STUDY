using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GI_STUDY
{
    static class SampleRandomizely
    {
        public static void sampleTheMatchList(List<RowMatch> originMatches, int pairCount)
        {
            foreach (RowMatch rowMatch in originMatches)
            {
                List<string[]> originMatchedRows = new List<string[]>(rowMatch.matchedRows);
                List<string[]> newMatchedRows = new List<string[]>();

                while (newMatchedRows.Count < pairCount && originMatchedRows.Count > 0)
                {
                    moveOneRandomly(originMatchedRows, newMatchedRows);
                }
                rowMatch.matchedRows = newMatchedRows;
            }
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
