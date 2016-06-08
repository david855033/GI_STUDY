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
            study1();   
            Console.WriteLine("End of Program. Press Any Key To Exit.");
            Console.ReadKey();
        }
        static void study1()
        {
            //breast feed group PS01=1, non breast feed PS01=0
            DataSet originDataSet = DataReader.LoadData(@"D:\GI DATA\FA_19609筆_修改 AGE GROUP.txt");

            DataSelector removeNoAgeOrNoSex = new DataSelector(originDataSet);
            removeNoAgeOrNoSex.addExcludeCriteria(new Criteria(originDataSet.getIndex("CASESEX"), ""));
            removeNoAgeOrNoSex.addExcludeCriteria(new Criteria(originDataSet.getIndex("AGE"), "9999"));
            DataSet dataSet_removeNoAgeOrNoSex = removeNoAgeOrNoSex.select();

            DataSet dataSet_StudyGroup, dataSet_NormalPopulation;

            DataSelector selectNormalGroup = new DataSelector(dataSet_removeNoAgeOrNoSex);
            selectNormalGroup.addExcludeCriteria(new Criteria(dataSet_removeNoAgeOrNoSex.getIndex("FS21"), "1"));
            dataSet_NormalPopulation = selectNormalGroup.select();

            DataSelector selectStudyGroup = new DataSelector(dataSet_removeNoAgeOrNoSex);
            selectStudyGroup.addIncludeCriteria(new Criteria(dataSet_removeNoAgeOrNoSex.getIndex("FS21"), "1"));
            dataSet_StudyGroup = selectStudyGroup.select();

            DataMatcher dataMatcher = new DataMatcher();
            dataMatcher.addMatchKey("AGEGROUP");
            dataMatcher.addMatchKey("CASESEX");

            initializeGroupContent(dataMatcher);


            dataMatcher.setPrimaryData(dataSet_StudyGroup);
            dataMatcher.setMatchData(dataSet_NormalPopulation);

            List<RowMatchPool> matchPools = dataMatcher.selectIntoMatchPools();

            SampleRandomizely.sampleTheMatchList(matchPools, 2);

            ChiSquareCalculator chiSquare = new ChiSquareCalculator(originDataSet.getIndex("PS04_2"), "1", matchPools);
            chiSquare.calculate();

            if (!Directory.Exists(@"D:\GI Data\result\")) { Directory.CreateDirectory(@"D:\GI Data\result\"); }
            var sw = new StreamWriter(@"D:\GI Data\result\result.txt");
            sw.WriteLine("===Count Table===");
            sw.WriteLine(chiSquare.printResult());
            sw.WriteLine("===Group Count===");
            sw.WriteLine(SummarizeMatchPools.printCount(matchPools, dataMatcher));
            sw.WriteLine();
            sw.Close();

        }

        static void initializeGroupContent(DataMatcher dataMatcher)
        {
            for (int i = 1; i <= 11; i++)
            {
                for (int j = 0; j <= 1; j++)
                {
                    dataMatcher.addGroupContentList(new string[] { i.ToString(), j.ToString() });
                }
            }
        }
    }






}
