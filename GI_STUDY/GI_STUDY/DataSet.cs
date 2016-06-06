using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GI_STUDY
{
    class DataSet
    {
        public Dictionary<string, int> index;
        public List<string[]> dataRow;
        public DataSet()
        {
            dataRow = new List<string[]>();
        }
        public void setIndexFromTitle(string title)
        {
            index = new Dictionary<string, int>();
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
    }
}
