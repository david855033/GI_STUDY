using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GI_STUDY
{
    class DataSelector
    {
        List<Criteria> includeCriteria;
        List<Criteria> excludeCriteria;
        DataSet originDataSet;
        public DataSelector(DataSet originDataSet)
        {
            includeCriteria = new List<Criteria>();
            excludeCriteria = new List<Criteria>();
            this.originDataSet = originDataSet;
        }
        public void addIncludeCriteria(Criteria toAdd)
        {
            includeCriteria.Add(toAdd);
        }
        public void addExcludeCriteria(Criteria toAdd)
        {
            excludeCriteria.Add(toAdd);
        }
        public DataSet select()
        {
            DataSet destineDataSet = new DataSet();
            destineDataSet.index = originDataSet.index;
            foreach (var row in originDataSet.dataRow)
            {
                bool isMatched = true;
                foreach (var c in includeCriteria)
                {
                    if (row[c.fieldIndex] != c.shouldEqual)
                    {
                        isMatched = false;
                        continue;
                    }
                }
                if (isMatched)
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
                if (isMatched)
                {
                    destineDataSet.addDataRow(row);
                }
            }
            return destineDataSet;
        }
    }
}
