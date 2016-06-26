using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GI_STUDY
{
    static class DataJoiner
    {
        static public DataSet joinData(this DataSet originDataSet, DataSet joinDataSet)
        {
            DataSet destineDataSet = new DataSet();
            destineDataSet.copyIndexFromDataSet(originDataSet);

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