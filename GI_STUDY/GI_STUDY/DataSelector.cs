using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GI_STUDY
{
    static class DataSelector
    {
        public static DataSet select(this DataSet originDataSet, List<Criteria> includeCriteria, List<Criteria> excludeCriteria)
        {
            DataSet destineDataSet = new DataSet();
            destineDataSet.index = originDataSet.index;
            foreach (var row in originDataSet.dataRow)
            {
                bool isMatched = false;
                if (includeCriteria != null)
                {
                    foreach (var c in includeCriteria)
                    {
                        if (row[c.fieldIndex] == c.shouldEqual)
                        {
                            isMatched = true;
                            continue;
                        }
                    }
                }
                else
                {
                    isMatched = true;
                }
                if (isMatched)
                {
                    if (excludeCriteria != null)
                    {
                        foreach (var c in excludeCriteria)
                        {
                            if (row[c.fieldIndex] == c.shouldEqual)
                            {
                                isMatched = false;
                                continue;
                            }
                        }
                    }
                }
                if (isMatched)
                {
                    destineDataSet.addDataRow(row);
                }
            }
            return destineDataSet;
        }
    }
}
