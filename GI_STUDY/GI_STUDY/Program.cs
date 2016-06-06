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
            classifier.classify();


            Console.WriteLine("End of Program. Press Any Key To Exit.");
            Console.ReadKey();
        }
    }

   
  

  
  
}
