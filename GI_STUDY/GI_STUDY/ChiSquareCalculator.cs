﻿using System;
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

        public static string printTitle()
        {
            return "\t(+)\t%\t(-)\t%\tTotal";
        }
        public string printResult()
        {
            StringBuilder result = new StringBuilder();
            group sum = new group() { Postive = primaryGroup.Postive + matchedGroup.Postive, Negative = primaryGroup.Negative + matchedGroup.Negative };
            result.AppendLine("Prm_Grp" + getGroupResult(primaryGroup));
            result.AppendLine("Mch_Grp" + getGroupResult(matchedGroup));
            result.AppendLine("total" + getGroupResult(sum));

            const double ChiCritical = 3.841;
            double primaryGroupPositiveExpect = ((double)primaryGroup.Postive + primaryGroup.Negative) * sum.Postive / (sum.Postive + sum.Negative);
            double primaryGroupNegativeExpect = ((double)primaryGroup.Postive + primaryGroup.Negative) * sum.Negative / (sum.Postive + sum.Negative);
            double matchGroupPositiveExpect = ((double)matchedGroup.Postive + matchedGroup.Negative) * sum.Postive / (sum.Postive + sum.Negative);
            double matchGroupNegativeExpect = ((double)matchedGroup.Postive + matchedGroup.Negative) * sum.Negative / (sum.Postive + sum.Negative);
            double ChiValue = Math.Pow(primaryGroupPositiveExpect - primaryGroup.Postive, 2) / primaryGroupPositiveExpect
                + Math.Pow(primaryGroupPositiveExpect - primaryGroup.Postive, 2) / primaryGroupPositiveExpect
                + Math.Pow(primaryGroupPositiveExpect - primaryGroup.Postive, 2) / primaryGroupPositiveExpect
                + Math.Pow(primaryGroupPositiveExpect - primaryGroup.Postive, 2) / primaryGroupPositiveExpect;

            result.AppendLine($"Chi Value = {Math.Round(ChiValue,3)}, Critical Chi = {Math.Round(ChiCritical,3)}, Significant = {ChiValue > ChiCritical}");

            return result.ToString(); ;
        }
        string getGroupResult(group g)
        {
            return $"\t{g.Postive}\t{g.PosPer}\t{g.Negative}\t{g.NegPer}\t{g.Total}";
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
