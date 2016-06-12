using System;
using System.Collections.Generic;
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
    }
}
