using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GI_STUDY
{
    class DataSet
    {
        public Dictionary<string, int> index = new Dictionary<string, int>();
        public List<string[]> dataRow = new List<string[]>();
        public DataSet()
        {

        }
        public void copyIndexFromDataSet(DataSet dataSet)
        {
            index = new Dictionary<string, int>();
            foreach (var d in dataSet.index)
            {
                index.Add(d.Key, d.Value);
            }
        }
        public void setIndexFromTitle(string title)
        {
            string[] splitline = title.Split('\t');
            for (int i = 0; i < splitline.Length; i++)
            {
                index.Add(splitline[i], i);
            }
        }
        public void addDataRow(string line)
        {
            dataRow.Add(line.Split('\t'));
        }
        public void addDataRow(string[] line)
        {
            dataRow.Add(line);
        }

        public int getIndex(string lookUpString)
        {
            return index[lookUpString];
        }
        public void ExportData(string path)
        {
            using (var sw = new StreamWriter(path, false, Encoding.Default))
            {
                StringBuilder Content = new StringBuilder();
                StringBuilder title = new StringBuilder();
                foreach (var i in index)
                {
                    title.Append(i.Key + "\t");
                }
                Content.AppendLine(title.ToString().TrimEnd('\t'));
                foreach (var r in dataRow)
                {
                    StringBuilder row = new StringBuilder();
                    foreach (var c in r)
                    {
                        row.Append(c + "\t");
                    }
                    Content.AppendLine(row.ToString().TrimEnd('\t'));
                }
                sw.Write(Content);
            }
        }
    }
}