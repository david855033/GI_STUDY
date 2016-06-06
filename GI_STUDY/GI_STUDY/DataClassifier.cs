using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GI_STUDY
{
    class DataClassifier
    {
        List<Criteria> criteria;
        DataSet originDataSet;
        DataSet destineDataSet;
        public DataClassifier(DataSet originDataSet, DataSet destineDataSet)
        {
            criteria = new List<Criteria>();
            this.originDataSet = originDataSet;
            this.destineDataSet = destineDataSet;
        }
        public void addCriteria(Criteria toAdd)
        {
            criteria.Add(toAdd);
        }
        public void classify()
        {
            destineDataSet.index = originDataSet.index;
            foreach (var row in originDataSet.dataRow)
            {
                bool isMatched = true;
                foreach (var c in criteria)
                {
                    if (row[c.fieldIndex] != c.shouldEqual)
                    {
                        isMatched = false;
                        continue;
                    }
                }
                if (isMatched)
                {
                    destineDataSet.addDataRow(row);
                }
            }
        }
    }
}
