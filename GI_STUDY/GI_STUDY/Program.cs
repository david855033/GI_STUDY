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
            study_BreastFeeding_Atopic();
            Console.WriteLine("End of Program. Press Any Key To Exit.");
            Console.ReadKey();
        }
        static void study_BreastFeeding_Atopic()
        {
            //breast feed group PS01=1, non breast feed PS01=0
            DataSet originDataSet = DataReader.LoadData(@"D:\GI DATA\FA_19609筆_修改 AGE GROUP_BREAST FEEDING.txt");
            showDataCount(originDataSet);

            Console.WriteLine("remove data without CASESEX and AGE data...");
            DataSelector removeNoAgeOrNoSex = new DataSelector(originDataSet);
            removeNoAgeOrNoSex.addExcludeCriteria(new Criteria(originDataSet.getIndex("CASESEX"), ""));
            removeNoAgeOrNoSex.addExcludeCriteria(new Criteria(originDataSet.getIndex("AGE"), "9999"));
            DataSet dataSet_removeNoAgeOrNoSex = removeNoAgeOrNoSex.select();
            showDataCount(dataSet_removeNoAgeOrNoSex);

            Console.WriteLine("select children (AGEGROUP 1-6) ...");
            DataSet ChildrenGroup = new DataSet();
            DataSelector[] selectAgeGroup = new DataSelector[6];
            for (int i = 0; i < 6; i++)
            {
                selectAgeGroup[i] = new DataSelector(dataSet_removeNoAgeOrNoSex);
                selectAgeGroup[i].addIncludeCriteria(new Criteria(originDataSet.getIndex("AGEGROUP"), (i + 1).ToString()));
                ChildrenGroup = new DataJoiner(ChildrenGroup).joinData(selectAgeGroup[i].select());
            }
            showDataCount(ChildrenGroup);


            DataSet dataSet_BreastMilk, dataSet_NonBreastMilk;

            Console.WriteLine("Select Breast Feed Group: (PS01 = 1)...");
            DataSelector selectStudyGroup = new DataSelector(ChildrenGroup);
            selectStudyGroup.addIncludeCriteria(new Criteria(originDataSet.getIndex("PS01"), "1"));
            dataSet_BreastMilk = selectStudyGroup.select();
            showDataCount(dataSet_BreastMilk);

            DataSet[] dataSet_subgroup_Pure_Breast_Feeding = new DataSet[6];
            initializeDataSetArray(dataSet_subgroup_Pure_Breast_Feeding);
            for (int i = 0; i < dataSet_subgroup_Pure_Breast_Feeding.Length; i++)
            {
                Console.WriteLine($"Select subgroup - pure breast{i + 1}");
                DataSelector selectSubGroup = new DataSelector(dataSet_BreastMilk);
                selectSubGroup.addIncludeCriteria(new Criteria(originDataSet.getIndex("BREAST_PURE_GROUP"), (i + 1).ToString()));
                dataSet_subgroup_Pure_Breast_Feeding[i] = selectSubGroup.select();
                showDataCount(dataSet_subgroup_Pure_Breast_Feeding[i]);
            }

            DataSet[] dataSet_subgroup_Partial_Breast_Feeding = new DataSet[6];
            initializeDataSetArray(dataSet_subgroup_Partial_Breast_Feeding);
            for (int i = 0; i < dataSet_subgroup_Partial_Breast_Feeding.Length; i++)
            {
                Console.WriteLine($"Select subgroup - partial breast{i + 1}");
                DataSelector selectSubGroup = new DataSelector(dataSet_BreastMilk);
                selectSubGroup.addIncludeCriteria(new Criteria(originDataSet.getIndex("BREAST_PARTIAL_GROUP"), (i + 1).ToString()));
                dataSet_subgroup_Partial_Breast_Feeding[i] = selectSubGroup.select();
                showDataCount(dataSet_subgroup_Partial_Breast_Feeding[i]);
            }


            Console.WriteLine("Select Non-Breast Feed Group: (PS01 = 0)...");
            DataSelector selectNormalGroup = new DataSelector(ChildrenGroup);
            selectNormalGroup.addIncludeCriteria(new Criteria(originDataSet.getIndex("PS01"), "0"));
            dataSet_NonBreastMilk = selectNormalGroup.select();
            showDataCount(dataSet_NonBreastMilk);


            string result = "";
            result = MatchAndCalculateChiSquare("NonBreastMilk vs BreastMilk on PS04", 5, dataSet_NonBreastMilk, dataSet_BreastMilk, 1, "PS04", "1", result);

            if (!Directory.Exists(@"D:\GI Data\result\")) { Directory.CreateDirectory(@"D:\GI Data\result\"); }
            var sw = new StreamWriter(@"D:\GI Data\result\result.txt");
            sw.Write(result);
            sw.Close();
        }
        static string MatchAndCalculateChiSquare(string name, int repeat, DataSet Primary, DataSet Match, int matchCount,
            string CalculateField, string fieldPostiveCriteria, string Result)
        {
            StringBuilder result = new StringBuilder(Result);
            result.AppendLine(name);
            for (int i = 0; i < repeat; i++)
            {
                DataMatcher dataMatcher = new DataMatcher();
                dataMatcher.addMatchKey("AGEGROUP");
                dataMatcher.addMatchKey("CASESEX");
                initializeGroupContent(dataMatcher);
                dataMatcher.setPrimaryData(Primary);
                dataMatcher.setMatchData(Match);
                List<RowMatchPool> matchPools = dataMatcher.matchIntoPools();
                SampleRandomizely.sampleTheMatchList(matchPools, matchCount);
                ChiSquareCalculator chiSquare = new ChiSquareCalculator(Primary.getIndex(CalculateField), fieldPostiveCriteria, matchPools);
                chiSquare.calculate();
                result.AppendLine($"Test #{i+1}");
                result.AppendLine(ChiSquareCalculator.printTitle());
                result.Append(chiSquare.printResult());
                result.AppendLine("");
            }

            return result.ToString();
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
        static void showDataCount(DataSet input)
        {
            Console.WriteLine($">>data count: {input.dataRow.Count}");
        }
        static void initializeDataSetArray(DataSet[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = new DataSet();
            }
        }
    }






}
