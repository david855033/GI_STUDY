using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GI_STUDY
{
    class ChiSquareCalculator
    {
        int fieldIndex;
        string postiveShouldequalTo;
        List<RowMatchPool> matchPools;
        group primaryGroup, matchedGroup;
        public ChiSquareCalculator(int fieldIndex, string postiveShouldequalTo, List<RowMatchPool> matchPools)
        {
            this.fieldIndex = fieldIndex;
            this.matchPools = matchPools;
            this.postiveShouldequalTo = postiveShouldequalTo;
        }

        public void calculate()
        {
            primaryGroup = new group();
            matchedGroup = new group();
            foreach (RowMatchPool matchPool in matchPools)
            {
                foreach (string[] primaryRow in matchPool.primaryRows)
                {
                    if (primaryRow[fieldIndex] == postiveShouldequalTo)
                    {
                        primaryGroup.Postive++;
                    }
                    else
                    {
                        primaryGroup.Negative++;
                    }
                }
                foreach (string[] matchedRow in matchPool.matchedRows)
                {
                    if (matchedRow[fieldIndex] == postiveShouldequalTo)
                    {
                        matchedGroup.Postive++;
                    }
                    else
                    {
                        matchedGroup.Negative++;
                    }
                }
            }
        }

        public string printResult()
        {
            StringBuilder result = new StringBuilder();
            group sum = new group() { Postive = primaryGroup.Postive + matchedGroup.Postive, Negative = primaryGroup.Negative + matchedGroup.Negative };
            result.AppendLine("\t(+)\t%\t(-)\t%\tTotal");
            result.AppendLine(getGroupResult(primaryGroup));
            result.AppendLine(getGroupResult(matchedGroup));
            result.AppendLine(getGroupResult(sum));
            return result.ToString(); ;
        }
        string getGroupResult(group g)
        {
            return $"Primary\t{g.Postive}\t{g.PosPer}\t{g.Negative}\t{g.NegPer}\t{g.Total}";
        }


        class group
        {
            public int Postive = 0;
            public int Negative = 0;
            public string PosPer
            {
                get { return Math.Round((double)Postive / Total * 100, 2) + "%"; }
            }
            public string NegPer
            {
                get { return Math.Round((double)Negative / Total * 100, 2) + "%"; }
            }
            public int Total
            {
                get { return Postive + Negative; }
            }
        }
    }
}
