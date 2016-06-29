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
        public string outputByField(List<string> fields)
        {
            StringBuilder title = new StringBuilder();
            List<string> toRemove = new List<string>();
            foreach (var s in fields)
            {
                if (index.ContainsKey(s))
                {
                    title.Append(s + "\t");
                }
                else
                {
                    toRemove.Add(s);
                }
            }
            fields.RemoveAll(x => toRemove.Contains(x));
            List<int> indexs = new List<int>();
            foreach (var s in fields)
            {
                indexs.Add(getIndex(s));
            }
            StringBuilder content = new StringBuilder();
            content.AppendLine(title.ToString().Trim('\t'));
            foreach (var row in dataRow)
            {
                StringBuilder line = new StringBuilder();
                foreach (var i in indexs)
                {
                    line.Append(row[i] + "\t");
                }
                content.AppendLine(line.ToString().Trim('\t'));
            }
            return content.ToString();
        }
        public void addfield(IDaterConvertor dataConvertor, string fieldname)
        {
            if (index.ContainsKey(fieldname))
                return;
            index.Add(fieldname, dataRow.First().Count());
            {

            }
        }
    }
}

interface IDaterConvertor
{
    string getFieldContent();
}