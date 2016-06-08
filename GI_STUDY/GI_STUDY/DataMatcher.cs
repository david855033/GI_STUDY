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
        List<List<string>> groupContentList;
        List<RowMatchPool> rowMatchPool ;
        DataSet primaryData, matchData;

        public DataMatcher()
        {
            matchKey = new List<string>();
            groupContentList = new List<List<string>>();
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

        public void addGroupContentList(IEnumerable<string> toAdd)
        {
            groupContentList.Add(new List<string>(toAdd));
        }

        public List<RowMatchPool> selectIntoMatchPools()
        {
            rowMatchPool = new List<RowMatchPool>();
            for (int i = 0; i < groupContentList.Count;i++)
                rowMatchPool.Add(new RowMatchPool());

            foreach (string[] primaryRow in primaryData.dataRow)
            {
                List<string> matchContentInThisRow = getMatchContent(matchKey, primaryRow);
                for (int i = 0; i < groupContentList.Count; i++)
                {
                    if (isTwoMatchContentEqual(matchContentInThisRow, groupContentList[i]))
                    {
                        rowMatchPool[i].primaryRows.Add(primaryRow);
                        continue;
                    }
                }
            }

            foreach (string[]  matchRow in matchData.dataRow)
            {
                List<string> matchContentInThisRow = getMatchContent(matchKey, matchRow);
                for (int i = 0; i < groupContentList.Count; i++)
                {
                    if (isTwoMatchContentEqual(matchContentInThisRow, groupContentList[i]))
                    {
                        rowMatchPool[i].matchedRows.Add(matchRow);
                        continue;
                    }
                }
            }
            return rowMatchPool;
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
