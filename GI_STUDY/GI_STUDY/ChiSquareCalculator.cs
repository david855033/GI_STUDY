using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GI_STUDY
{
    class ChiSquareCalculator
    {
        const double CHI_CRITICAL = 3.841;
        List<FieldNameToTest> CalculateFields;
        List<RowMatchPool> matchPools;
        group primaryGroup, matchedGroup;
        public ChiSquareCalculator(List<FieldNameToTest> CalculateFields, List<RowMatchPool> matchPools)
        {
            this.CalculateFields = CalculateFields;
            this.matchPools = matchPools;
        }

        public void countPosAndNegInBothGroup()
        {
            primaryGroup = new group();
            matchedGroup = new group();
            foreach (RowMatchPool matchPool in matchPools)
            {
                foreach (string[] primaryRow in matchPool.primaryRows)
                {
                    bool IsPostive = true;
                    foreach (var fieldset in CalculateFields)
                    {
                        if (primaryRow[fieldset.fieldindex] != fieldset.positiveValue)
                        {
                            IsPostive = false;
                            break;
                        }
                    }
                    if (IsPostive)
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
                    bool IsPostive = true;
                    foreach (var fieldset in CalculateFields)
                    {
                        if (matchedRow[fieldset.fieldindex] != fieldset.positiveValue)
                        {
                            IsPostive = false;
                            break;
                        }
                    }
                    if (IsPostive)
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
        public string printResult(ref int primaryMore, ref int matchMore, string name)
        {
            StringBuilder result = new StringBuilder();
            group sum = new group() { Postive = primaryGroup.Postive + matchedGroup.Postive, Negative = primaryGroup.Negative + matchedGroup.Negative };
            result.AppendLine("Prm_Grp" + getGroupResult(primaryGroup));
            result.AppendLine("Mch_Grp" + getGroupResult(matchedGroup));
            result.AppendLine("total" + getGroupResult(sum));

            double oddsRatio = Math.Round((double)primaryGroup.Postive * matchedGroup.Negative / primaryGroup.Negative / matchedGroup.Postive, 2);
            double LNofOR = Math.Log(oddsRatio);
            double SEofLN = Math.Pow(1 / (double)primaryGroup.Postive + 1 / (double)primaryGroup.Negative +
                1 / (double)matchedGroup.Postive + 1 / (double)matchedGroup.Negative, 0.5);
            double UL = LNofOR + 1.96 * SEofLN;
            double LL = LNofOR - 1.96 * SEofLN;
            double CI_Upper = Math.Round(Math.Exp(UL), 2);
            double CI_Lower = Math.Round(Math.Exp(LL), 2);
            result.AppendLine($"[Odds Ratio] = {oddsRatio}, [95%CI] = {CI_Lower}~{CI_Upper}");

            double relativeRisk =
                Math.Round(
                    (double)primaryGroup.Postive / ((double)primaryGroup.Negative + primaryGroup.Postive)
                        /
                    ((double)matchedGroup.Postive / ((double)matchedGroup.Postive + matchedGroup.Negative)),
                    2);
            double LNofRR = Math.Log(relativeRisk);
            double SEofLNofRR = Math.Pow(
                (double)primaryGroup.Negative / primaryGroup.Postive / ((double)primaryGroup.Postive + primaryGroup.Negative)
                + (double)matchedGroup.Negative / matchedGroup.Postive / ((double)matchedGroup.Postive + matchedGroup.Negative)
                , 0.5);
            double UL_RR = LNofRR + 1.96 * SEofLNofRR;
            double LL_RR = LNofRR - 1.96 * SEofLNofRR;
            double CI_Upper_RR = Math.Round(Math.Exp(UL_RR), 2);
            double CI_Lower_RR = Math.Round(Math.Exp(LL_RR), 2);
            result.AppendLine($"[Relative Risk] = {relativeRisk}, [95%CI] = {CI_Lower_RR}~{CI_Upper_RR}");

            double primaryGroupPositiveExpect = ((double)primaryGroup.Postive + primaryGroup.Negative) * sum.Postive / (sum.Postive + sum.Negative);
            double primaryGroupNegativeExpect = ((double)primaryGroup.Postive + primaryGroup.Negative) * sum.Negative / (sum.Postive + sum.Negative);
            double matchGroupPositiveExpect = ((double)matchedGroup.Postive + matchedGroup.Negative) * sum.Postive / (sum.Postive + sum.Negative);
            double matchGroupNegativeExpect = ((double)matchedGroup.Postive + matchedGroup.Negative) * sum.Negative / (sum.Postive + sum.Negative);
            double ChiValue = Math.Pow(primaryGroupPositiveExpect - primaryGroup.Postive, 2) / primaryGroupPositiveExpect
                + Math.Pow(primaryGroupPositiveExpect - primaryGroup.Postive, 2) / primaryGroupPositiveExpect
                + Math.Pow(primaryGroupPositiveExpect - primaryGroup.Postive, 2) / primaryGroupPositiveExpect
                + Math.Pow(primaryGroupPositiveExpect - primaryGroup.Postive, 2) / primaryGroupPositiveExpect;

            bool significant = ChiValue > CHI_CRITICAL;
            OddsRatioTable.addOddRatio(name, oddsRatio, CI_Lower, CI_Upper, relativeRisk, CI_Lower_RR, CI_Upper_RR, primaryGroup.Postive, primaryGroup.Negative, matchedGroup.Postive, matchedGroup.Negative, significant);
            result.AppendLine($"[Chi Value] = {Math.Round(ChiValue, 3)}, Significant = {(significant ? "*" : " none ")} ");

            if (ChiValue > CHI_CRITICAL)
            {
                if ((double)primaryGroup.Postive / (primaryGroup.Postive + primaryGroup.Negative) >
                    (double)matchedGroup.Postive / (matchedGroup.Postive + matchedGroup.Negative))
                {
                    primaryMore++;
                }
                else
                {
                    matchMore++;
                }
            }
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
