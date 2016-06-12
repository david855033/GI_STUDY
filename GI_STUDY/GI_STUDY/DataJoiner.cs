using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GI_STUDY
{
    class DataJoiner
    {
        DataSet originDataSet;
        public DataJoiner(DataSet originDataSet)
        {
            this.originDataSet = originDataSet;
        }
        public DataSet joinData(DataSet joinDataSet)
        {
            DataSet destineDataSet = new DataSet();
            foreach (var i in joinDataSet.index)
            {
                destineDataSet.index.Add(i.Key, i.Value);
            }
            foreach (var row in originDataSet.dataRow)
            {
                destineDataSet.addDataRow(row);
            }
            foreach (var row in joinDataSet.dataRow)
            {
                destineDataSet.addDataRow(row);
            }
            return destineDataSet;
        }
    }
}