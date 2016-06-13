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
            //study_BreastFeeding_Atopic();
            study_BreastFeeding_Atopic2();
            Console.WriteLine(".");
            Console.WriteLine(".");
            Console.WriteLine(".");
            Console.WriteLine("End of Program. Press Any Key To Exit.");
            Console.ReadKey();
        }
        static void study_BreastFeeding_Atopic2()
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
            DataSelector[] selectAgeGroup = new DataSelector[7];
            for (int i = 0; i < 7; i++)
            {
                selectAgeGroup[i] = new DataSelector(dataSet_removeNoAgeOrNoSex);
                selectAgeGroup[i].addIncludeCriteria(new Criteria(originDataSet.getIndex("AGEGROUP"), (i + 1).ToString()));
                ChildrenGroup = new DataJoiner(ChildrenGroup).joinData(selectAgeGroup[i].select());
            }
            showDataCount(ChildrenGroup);

            Console.WriteLine("select children (AGEGROUP 1-6) ...");
            DataSet SmallChildrenGroup = new DataSet();
            DataSelector[] SmallselectAgeGroup = new DataSelector[3];
            for (int i = 0; i < 3; i++)
            {
                SmallselectAgeGroup[i] = new DataSelector(dataSet_removeNoAgeOrNoSex);
                SmallselectAgeGroup[i].addIncludeCriteria(new Criteria(originDataSet.getIndex("AGEGROUP"), (i + 1).ToString()));
                SmallChildrenGroup = new DataJoiner(SmallChildrenGroup).joinData(SmallselectAgeGroup[i].select());
            }
            showDataCount(SmallChildrenGroup);

            Console.WriteLine("select children (AGEGROUP 1-6) ...");
            DataSet OldChildrenGroup = new DataSet();
            DataSelector[] OldselectAgeGroup = new DataSelector[4];
            for (int i = 0; i < 4; i++)
            {
                OldselectAgeGroup[i] = new DataSelector(dataSet_removeNoAgeOrNoSex);
                OldselectAgeGroup[i].addIncludeCriteria(new Criteria(originDataSet.getIndex("AGEGROUP"), (i + 3).ToString()));
                OldChildrenGroup = new DataJoiner(OldChildrenGroup).joinData(OldselectAgeGroup[i].select());
            }
            showDataCount(OldChildrenGroup);



            DataSet dataSet_EggMilk_Allergy_small, dataSet_NonAllergy_small ;

            Console.WriteLine("Select dataSet_EggMilk_Allergy");
            DataSelector selectStudyGroupSmall = new DataSelector(SmallChildrenGroup);
            selectStudyGroupSmall.addIncludeCriteria(new Criteria(originDataSet.getIndex("PS03"), "2"));
            dataSet_EggMilk_Allergy_small = selectStudyGroupSmall.select();
            showDataCount(dataSet_EggMilk_Allergy_small);

            Console.WriteLine("Select dataSet_EggMilk_Allergy");
            DataSelector selectControlGroupSmall= new DataSelector(SmallChildrenGroup);
            selectControlGroupSmall.addExcludeCriteria(new Criteria(originDataSet.getIndex("PS03"), "2"));
            dataSet_NonAllergy_small = selectControlGroupSmall.select();
            showDataCount(dataSet_NonAllergy_small);


            DataSet dataSet_EggMilk_Allergy_old, dataSet_NonAllergy_old;

            Console.WriteLine("Select dataSet_EggMilk_Allergy");
            DataSelector selectStudyGroupOld = new DataSelector(OldChildrenGroup);
            selectStudyGroupOld.addIncludeCriteria(new Criteria(originDataSet.getIndex("PS03"), "2"));
            dataSet_EggMilk_Allergy_old = selectStudyGroupOld.select();
            showDataCount(dataSet_EggMilk_Allergy_old);

            Console.WriteLine("Select dataSet_EggMilk_Allergy");
            DataSelector selectControlGroupOld = new DataSelector(OldChildrenGroup);
            selectControlGroupOld.addExcludeCriteria(new Criteria(originDataSet.getIndex("PS03"), "2"));
            dataSet_NonAllergy_old = selectControlGroupOld.select();
            showDataCount(dataSet_NonAllergy_old);


            string result = ""; string studyGroup;
            string folder;
            string fieldname, info;
            int repeat = 100, matchcount = 4;
            DataSet primary, match;
            OddsRatioTable.clear();
            OddsRatioTable.setPath($@"D:\GI Data\result Egg Milk small old\Odd Ratio Table.txt");
            studyGroup = "EggMilk_Veg vs NonEggMilk_Veg";
            folder = $@"D:\GI Data\result Egg Milk small old\{studyGroup}";
            primary = dataSet_EggMilk_Allergy_old; match = dataSet_NonAllergy_old;

            fieldname = "PS04"; info = "是否曾有其他過敏性疾病";
            result = MatchAndCalculateChiSquare($"{studyGroup} on {fieldname}", repeat, primary, match, matchcount, fieldname, "1");
            writeResult(result, folder, $"{fieldname}({info}).txt");
            fieldname = "PS04_1"; info = "過敏性鼻炎";
            result = MatchAndCalculateChiSquare($"{studyGroup} on {fieldname}", repeat, primary, match, matchcount, fieldname, "1");
            writeResult(result, folder, $"{fieldname}({info}).txt");
            fieldname = "PS04_2"; info = "氣喘";
            result = MatchAndCalculateChiSquare($"{studyGroup} on {fieldname}", repeat, primary, match, matchcount, fieldname, "1");
            writeResult(result, folder, $"{fieldname}({info}).txt");
            fieldname = "PS04_3"; info = "異位性皮膚炎";
            result = MatchAndCalculateChiSquare($"{studyGroup} on {fieldname}", repeat, primary, match, matchcount, fieldname, "1");
            writeResult(result, folder, $"{fieldname}({info}).txt");
            fieldname = "PS04_4"; info = "蕁麻疹";
            result = MatchAndCalculateChiSquare($"{studyGroup} on {fieldname}", repeat, primary, match, matchcount, fieldname, "1");
            writeResult(result, folder, $"{fieldname}({info}).txt");
            fieldname = "FS01"; info = "是否可能發生過食物過敏";
            result = MatchAndCalculateChiSquare($"{studyGroup} on {fieldname}", repeat, primary, match, matchcount, fieldname, "1");
            writeResult(result, folder, $"{fieldname}({info}).txt");
            fieldname = "FS05"; info = "是否知道對何種食物過敏";
            result = MatchAndCalculateChiSquare($"{studyGroup} on {fieldname}", repeat, primary, match, matchcount, fieldname, "1");
            writeResult(result, folder, $"{fieldname}({info}？).txt");
            fieldname = "FS21"; info = "請問是否曾求醫，檢查確定是上述食物過敏";
            result = MatchAndCalculateChiSquare($"{studyGroup} on {fieldname}", repeat, primary, match, matchcount, fieldname, "1");
            writeResult(result, folder, $"{fieldname}({info}).txt");

            OddsRatioTable.writeToFile();
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
            DataSelector[] selectAgeGroup = new DataSelector[7];
            for (int i = 0; i < 7; i++)
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

            string result = ""; string studyGroup;
            string folder;
            string fieldname, info;
            int repeat = 5;
            DataSet primary, match;
            OddsRatioTable.clear();
            OddsRatioTable.setPath($@"D:\GI Data\result\Odd Ratio Table.txt");
            studyGroup = "NonBreastMilk vs BreastMilk";
            folder = $@"D:\GI Data\result\{studyGroup}";
            primary = dataSet_NonBreastMilk; match = dataSet_BreastMilk;

            fieldname = "PS04"; info = "是否曾有其他過敏性疾病";
            result = MatchAndCalculateChiSquare($"{studyGroup} on {fieldname}", repeat, primary, match, 1, fieldname, "1");
            writeResult(result, folder, $"{fieldname}({info}).txt");
            fieldname = "PS04_1"; info = "過敏性鼻炎";
            result = MatchAndCalculateChiSquare($"{studyGroup} on {fieldname}", repeat, primary, match, 1, fieldname, "1");
            writeResult(result, folder, $"{fieldname}({info}).txt");
            fieldname = "PS04_2"; info = "氣喘";
            result = MatchAndCalculateChiSquare($"{studyGroup} on {fieldname}", repeat, primary, match, 1, fieldname, "1");
            writeResult(result, folder, $"{fieldname}({info}).txt");
            fieldname = "PS04_3"; info = "異位性皮膚炎";
            result = MatchAndCalculateChiSquare($"{studyGroup} on {fieldname}", repeat, primary, match, 1, fieldname, "1");
            writeResult(result, folder, $"{fieldname}({info}).txt");
            fieldname = "PS04_4"; info = "蕁麻疹";
            result = MatchAndCalculateChiSquare($"{studyGroup} on {fieldname}", repeat, primary, match, 1, fieldname, "1");
            writeResult(result, folder, $"{fieldname}({info}).txt");
            fieldname = "FS01"; info = "是否可能發生過食物過敏";
            result = MatchAndCalculateChiSquare($"{studyGroup} on {fieldname}", repeat, primary, match, 1, fieldname, "1");
            writeResult(result, folder, $"{fieldname}({info}).txt");
            fieldname = "FS05"; info = "是否知道對何種食物過敏";
            result = MatchAndCalculateChiSquare($"{studyGroup} on {fieldname}", repeat, primary, match, 1, fieldname, "1");
            writeResult(result, folder, $"{fieldname}({info}？).txt");
            fieldname = "FS21"; info = "請問是否曾求醫，檢查確定是上述食物過敏";
            result = MatchAndCalculateChiSquare($"{studyGroup} on {fieldname}", repeat, primary, match, 1, fieldname, "1");
            writeResult(result, folder, $"{fieldname}({info}).txt");

            for (int i = 0; i < 6; i++)
            {
                studyGroup = $"Pure Breast Feeding Group{(i + 1).ToString()} vs Non Breast Feeding";
                folder = $@"D:\GI Data\result\{studyGroup} ";
                primary = dataSet_subgroup_Pure_Breast_Feeding[i]; match = dataSet_NonBreastMilk;

                fieldname = "PS04"; info = "是否曾有其他過敏性疾病";
                result = MatchAndCalculateChiSquare($"{studyGroup} on {fieldname}", repeat, primary, match, 1, fieldname, "1");
                writeResult(result, folder, $"{fieldname}({info}).txt");
                fieldname = "PS04_1"; info = "過敏性鼻炎";
                result = MatchAndCalculateChiSquare($"{studyGroup} on {fieldname}", repeat, primary, match, 1, fieldname, "1");
                writeResult(result, folder, $"{fieldname}({info}).txt");
                fieldname = "PS04_2"; info = "氣喘";
                result = MatchAndCalculateChiSquare($"{studyGroup} on {fieldname}", repeat, primary, match, 1, fieldname, "1");
                writeResult(result, folder, $"{fieldname}({info}).txt");
                fieldname = "PS04_3"; info = "異位性皮膚炎";
                result = MatchAndCalculateChiSquare($"{studyGroup} on {fieldname}", repeat, primary, match, 1, fieldname, "1");
                writeResult(result, folder, $"{fieldname}({info}).txt");
                fieldname = "PS04_4"; info = "蕁麻疹";
                result = MatchAndCalculateChiSquare($"{studyGroup} on {fieldname}", repeat, primary, match, 1, fieldname, "1");
                writeResult(result, folder, $"{fieldname}({info}).txt");
                fieldname = "FS01"; info = "是否可能發生過食物過敏";
                result = MatchAndCalculateChiSquare($"{studyGroup} on {fieldname}", repeat, primary, match, 1, fieldname, "1");
                writeResult(result, folder, $"{fieldname}({info}).txt");
                fieldname = "FS05"; info = "是否知道對何種食物過敏";
                result = MatchAndCalculateChiSquare($"{studyGroup} on {fieldname}", repeat, primary, match, 1, fieldname, "1");
                writeResult(result, folder, $"{fieldname}({info}？).txt");
                fieldname = "FS21"; info = "請問是否曾求醫，檢查確定是上述食物過敏";
                result = MatchAndCalculateChiSquare($"{studyGroup} on {fieldname}", repeat, primary, match, 1, fieldname, "1");
                writeResult(result, folder, $"{fieldname}({info}).txt");
            }

            for (int i = 0; i < 6; i++)
            {
                studyGroup = $"Partial Breast Feeding Group{(i + 1).ToString()} vs Non Breast Feeding";
                folder = $@"D:\GI Data\result\{studyGroup} ";
                primary = dataSet_subgroup_Partial_Breast_Feeding[i]; match = dataSet_NonBreastMilk;

                fieldname = "PS04"; info = "是否曾有其他過敏性疾病";
                result = MatchAndCalculateChiSquare($"{studyGroup} on {fieldname}", repeat, primary, match, 1, fieldname, "1");
                writeResult(result, folder, $"{fieldname}({info}).txt");
                fieldname = "PS04_1"; info = "過敏性鼻炎";
                result = MatchAndCalculateChiSquare($"{studyGroup} on {fieldname}", repeat, primary, match, 1, fieldname, "1");
                writeResult(result, folder, $"{fieldname}({info}).txt");
                fieldname = "PS04_2"; info = "氣喘";
                result = MatchAndCalculateChiSquare($"{studyGroup} on {fieldname}", repeat, primary, match, 1, fieldname, "1");
                writeResult(result, folder, $"{fieldname}({info}).txt");
                fieldname = "PS04_3"; info = "異位性皮膚炎";
                result = MatchAndCalculateChiSquare($"{studyGroup} on {fieldname}", repeat, primary, match, 1, fieldname, "1");
                writeResult(result, folder, $"{fieldname}({info}).txt");
                fieldname = "PS04_4"; info = "蕁麻疹";
                result = MatchAndCalculateChiSquare($"{studyGroup} on {fieldname}", repeat, primary, match, 1, fieldname, "1");
                writeResult(result, folder, $"{fieldname}({info}).txt");
                fieldname = "FS01"; info = "是否可能發生過食物過敏";
                result = MatchAndCalculateChiSquare($"{studyGroup} on {fieldname}", repeat, primary, match, 1, fieldname, "1");
                writeResult(result, folder, $"{fieldname}({info}).txt");
                fieldname = "FS05"; info = "是否知道對何種食物過敏";
                result = MatchAndCalculateChiSquare($"{studyGroup} on {fieldname}", repeat, primary, match, 1, fieldname, "1");
                writeResult(result, folder, $"{fieldname}({info}？).txt");
                fieldname = "FS21"; info = "請問是否曾求醫，檢查確定是上述食物過敏";
                result = MatchAndCalculateChiSquare($"{studyGroup} on {fieldname}", repeat, primary, match, 1, fieldname, "1");
                writeResult(result, folder, $"{fieldname}({info}).txt");
            }
            OddsRatioTable.writeToFile();
        }


        static void writeResult(string result, string folder, string filename)
        {
            string path = folder + "\\" + filename;
            Console.WriteLine($"write to result file: >>  {path} <<");
            if (!Directory.Exists(folder)) { Directory.CreateDirectory(folder); }
            var sw = new StreamWriter(path);
            sw.Write(result);
            sw.Close();
        }
        static string MatchAndCalculateChiSquare(string name, int repeat, DataSet Primary, DataSet Match, int matchCount,
           string CalculateField, string fieldPostiveCriteria)
        {
            return MatchAndCalculateChiSquare(name, repeat, Primary, Match, matchCount,
             CalculateField, fieldPostiveCriteria, "");
        }
        static string MatchAndCalculateChiSquare(string name, int repeat, DataSet Primary, DataSet Match, int matchCount,
            string CalculateField, string fieldPostiveCriteria, string Result)
        {
            StringBuilder result = new StringBuilder(Result);
            int primaryMore = 0, matchMore = 0;
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
                result.AppendLine($"Test #{i + 1}");
                result.AppendLine(ChiSquareCalculator.printTitle());
                result.Append(chiSquare.printResult(ref primaryMore, ref matchMore, name));
                result.AppendLine("");
                if (i == repeat - 1)
                {
                    result.AppendLine("Match Detail");
                    result.Append(SummarizeMatchPools.printCount(matchPools, dataMatcher));
                    result.AppendLine("");
                }
            }
            Console.WriteLine($"calculate Chi square table {name} for {repeat} times");
            string summary = $"Significant result: Primary > Matched:{primaryMore},  Matched > Primary:{matchMore}, Non-Significant:{repeat - primaryMore - matchMore}";
            return name + "\r\n" + summary + "\r\n" + result.ToString();
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
