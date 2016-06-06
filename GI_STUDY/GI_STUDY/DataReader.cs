using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GI_STUDY
{
    class DataReader
    {
        DataSet dataSet;
        public DataReader(DataSet dataSet) { this.dataSet = dataSet; }
        public void LoadData(string path)
        {
            using (var sr = new StreamReader(path, Encoding.Default))
            {
                string title = sr.ReadLine();
                dataSet.setIndexFromTitle(title);
                while (!sr.EndOfStream)
                {
                    dataSet.addDataRow(sr.ReadLine());
                }
            }
        }
    }

}
