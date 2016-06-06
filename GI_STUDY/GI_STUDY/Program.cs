using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace GI_STUDY
{
    class Program
    {
        static void Main(string[] args)
        {
            DataSet dataSet = new DataSet();
            DataReader dataReader = new DataReader(dataSet);
            dataReader.LoadData(@"D:\GI DATA\RawData.txt");

            DataSet studyGroup = new DataSet();
            DataClassifier classifier = new DataClassifier(dataSet, studyGroup);
            classifier.addCriteria(new Criteria(dataSet.getIndex("PS03"), "3"));

            Console.WriteLine("End of Program. Press Any Key To Exit.");
            Console.ReadKey();
        }
    }

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
            destineDataSet = new DataSet();
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

    class Criteria
    {
        public int fieldIndex;
        public string shouldEqual;
        public Criteria(int fieldIndex, string shouldEqual)
        {
            this.fieldIndex = fieldIndex;
            this.shouldEqual = shouldEqual;
        }
    }
    
 
}
