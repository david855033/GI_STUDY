using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GI_STUDY
{
    static class DataReader
    {
        public static DataSet LoadData(string path)
        {
            Console.WriteLine($"Loading data from {path}");
            DataSet dataSet = new DataSet();
            using (var sr = new StreamReader(path, Encoding.Default))
            {
                string title = sr.ReadLine();
                dataSet.setIndexFromTitle(title);
                while (!sr.EndOfStream)
                {
                    dataSet.addDataRow(sr.ReadLine());
                }
            }
            return dataSet;
        }
    }

}
