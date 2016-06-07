using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GI_STUDY
{
    class DataMatcher
    {
        List<string> matchKey;
        DataSet primaryData;
        DataSet matchData;
        List<RowMatch> matchedRows = new List<RowMatch>();
        public DataMatcher()
        {
            matchKey = new List<string>();
        }

        public void setPrimaryData(DataSet primaryData)
        {
            this.primaryData = primaryData;
        }

        public void setMatchData(DataSet matchData)
        {
            this.matchData = matchData;
        }

        public void addMatchKey(string toAdd)
        {
            matchKey.Add(toAdd);
        }

        public List<RowMatch> doMatch()
        {
            foreach (string[] primaryRow in primaryData.dataRow)
            {
                List<string> matchContentPrimary = getMatchContent(matchKey, primaryRow);
                RowMatch matchToAdd = new RowMatch() { primaryRow = primaryRow };
                foreach (string[] matchRow in matchData.dataRow)
                {
                    List<string> matchContentInThisRow = getMatchContent(matchKey, matchRow);
                    if (isTwoMatchContentEqual(matchContentPrimary, matchContentInThisRow))
                    {
                        matchToAdd.matchedRows.Add(matchRow);
                    }
                }
                matchedRows.Add(matchToAdd);
            }
            return matchedRows;
        }

        private List<string> getMatchContent(List<string> keys, string[] row)
        {
            List<string> matchContent = new List<string>();
            foreach (string key in keys)
            {
                int index = primaryData.getIndex(key);
                matchContent.Add(row[index]);
            }
            return matchContent;
        }

        private bool isTwoMatchContentEqual(List<string> content1, List<string> content2)
        {
            for (int i = 0; i < content1.Count; i++)
            {
                if (content1[i] != content2[i])
                {
                    return false;
                }
            }
            return true;
        }
    }
}
