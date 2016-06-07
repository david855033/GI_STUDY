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
            DataSet originDataSet = DataReader.LoadData(@"D:\GI DATA\FA_19609筆_加入ADULT_CASESEX.txt");

            DataSelector removeNoAgeOrNoSex = new DataSelector(originDataSet);
            removeNoAgeOrNoSex.addExcludeCriteria(new Criteria(originDataSet.getIndex("CASESEX"), ""));
            removeNoAgeOrNoSex.addExcludeCriteria(new Criteria(originDataSet.getIndex("AGE"), "9999"));
            DataSet dataSet_removeNoAgeOrNoSex = removeNoAgeOrNoSex.select();

            DataSet dataSet_EggMilkVeg, dataSet_NormalPopulation;

            DataSelector selectNormalGroup = new DataSelector(dataSet_removeNoAgeOrNoSex);
            selectNormalGroup.addIncludeCriteria(new Criteria(dataSet_removeNoAgeOrNoSex.getIndex("PS03"), "1"));
            dataSet_NormalPopulation = selectNormalGroup.select();

            DataSelector selectEggMilkVeg = new DataSelector(dataSet_removeNoAgeOrNoSex);
            selectEggMilkVeg.addIncludeCriteria(new Criteria(dataSet_removeNoAgeOrNoSex.getIndex("PS03"), "2"));
            dataSet_EggMilkVeg = selectEggMilkVeg.select();

            DataMatcher dataMatcher = new DataMatcher();
            dataMatcher.addMatchKey("AGE");
            dataMatcher.addMatchKey("CASESEX");
            dataMatcher.setPrimaryData(dataSet_EggMilkVeg);
            dataMatcher.setMatchData(dataSet_NormalPopulation);

            List<RowMatch> matchTable = dataMatcher.doMatch();

            SampleRandomizely.sampleTheMatchList(matchTable, 5);

            Console.WriteLine("End of Program. Press Any Key To Exit.");
            Console.ReadKey();
        }
    }






}
