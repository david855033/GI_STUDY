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
            FileStream filestream = new FileStream(@"D:\GI DATA\out.txt", FileMode.Append);
            var streamwriter = new StreamWriter(filestream);
            streamwriter.AutoFlush = true;
            Console.SetOut(streamwriter);
            Console.SetError(streamwriter);
            Console.WriteLine($"Execution log======================{DateTime.Now}");
            study();
            Console.WriteLine(".");
            Console.WriteLine(".");
            Console.WriteLine(".");
            Console.WriteLine("End of Program. Press Any Key To Exit.");
        }
        static string basefolder;
        static int repeat;
        static void study()
        {
            //breast feed group PS01=1, non breast feed PS01=0
            DataSet originDataSet = DataReader.LoadData(@"D:\GI DATA\FA_19609筆_修改 AGE GROUP_BREAST FEEDING.txt");
            showDataCount(originDataSet);
            Criteria.index = originDataSet.index;

            Console.WriteLine("remove data without CASESEX and AGE data...");
            List<Criteria> BadData_toExclude = new List<Criteria>()  {
                new Criteria("CASESEX", "") ,
                new Criteria("AGE", "9999")
            };
            DataSet dataSet_removeNoAgeOrNoSex = originDataSet.select(null, BadData_toExclude);
            showDataCount(dataSet_removeNoAgeOrNoSex);

            Console.WriteLine("select Children...");
            DataSet DataSets_Children = new DataSet();
            for (int i = 0; i < 4; i++)
            {
                List<Criteria> toInclude = new List<Criteria>() {
                    new Criteria("AGEGROUP", (i + 8).ToString())
                };
                DataSets_Children = DataSets_Children.joinData(dataSet_removeNoAgeOrNoSex.select(toInclude, null));
            }
            showDataCount(DataSets_Children);

            List<Criteria> Vegetarian_toInclude = new List<Criteria>()  {
                new Criteria("PS03", "2") ,
                new Criteria("PS03", "3") ,
                new Criteria("PS03", "4") ,
                new Criteria("PS03", "5")
            };
            List<Criteria> non_Vegetarian_toInclude = new List<Criteria>()  {
                new Criteria("PS03", "1")
            };

            List<Criteria> DoctorFoodAllergy_toInclude = new List<Criteria>()  {
                new Criteria("FS21", "1")
            };

            Console.WriteLine("select Vegetarian...");
            DataSet dataSet_vegetarian = DataSets_Children.select(Vegetarian_toInclude, null);
            showDataCount(dataSet_vegetarian);

            Console.WriteLine("select non-Vegetarian...");
            DataSet dataSet_non_vegetarian = DataSets_Children.select(non_Vegetarian_toInclude, null);
            showDataCount(dataSet_non_vegetarian);

            int agegroups =4;
            int firstAgeGroup = 8;
            DataSet[] dataSet_vegetarian_byAge = new DataSet[agegroups];
            for (int i = 0; i < agegroups; i++)
            {
                Console.WriteLine($"select Vegetarian. age group{i + 1}");
                dataSet_vegetarian_byAge[i] = dataSet_vegetarian.select(new List<Criteria>() { new Criteria("AGEGROUP", (i + firstAgeGroup).ToString()) }, null);
                showDataCount(dataSet_vegetarian_byAge[i]);
            }

            DataSet[] dataSet_non_vegetarian_byAge = new DataSet[agegroups];
            for (int i = 0; i < agegroups; i++)
            {
                Console.WriteLine($"select non-Vegetarian. age group{i + 1}");
                dataSet_non_vegetarian_byAge[i] = dataSet_non_vegetarian.select(new List<Criteria>() { new Criteria("AGEGROUP", (i + firstAgeGroup).ToString()) }, null);
                showDataCount(dataSet_non_vegetarian_byAge[i]);
            }

            initializeTestList();

            repeat = 100;
            basefolder = $@"D:\GI Data\result\";
            OddsRatioTable.clear();
            OddsRatioTable.setPath(basefolder + $@"\Odd Ratio Table.txt");

            DoTestAndWriteResult(dataSet_vegetarian, dataSet_non_vegetarian, "Veg vs NonVeg");

            for (int i = 0; i < dataSet_vegetarian_byAge.Length; i++)
            {
                DoTestAndWriteResult(dataSet_vegetarian_byAge[i], dataSet_non_vegetarian_byAge[i], $"Veg vs NonVeg _subgrouping by age {i + 1}");
            }

            OddsRatioTable.writeToFile();


        }
        static void DoTestAndWriteResult(DataSet primary, DataSet match, string studyGroup)
        {
            foreach (var fieldnameSet in testList)
            {
                DoTestAndWriteResult(fieldnameSet, primary, match, studyGroup);
            }
        }
        static void DoTestAndWriteResult(FieldNameToTest fieldnameSet, DataSet primary, DataSet match, string studyGroup)
        {
            int matchcount = 1;
            if (match.dataRow.Count / primary.dataRow.Count > 12)
            {
                matchcount = 10;
            }
            else
            if (match.dataRow.Count / primary.dataRow.Count > 5)
            {
                matchcount = 4;
            }
            else
            if (match.dataRow.Count / primary.dataRow.Count > 3)
            {
                matchcount = 2;
            }
            
            var s = MatchAndCalculateChiSquare($"{studyGroup} on {fieldnameSet.fieldname}", repeat, primary, match, matchcount, fieldnameSet.fieldname, fieldnameSet.positiveValue);
            string folder = basefolder + $@"\{studyGroup}";
            writeResult(s, folder, $"{fieldnameSet.fieldname}({fieldnameSet.info}).txt");
        }
        static List<FieldNameToTest> testList;
        static void initializeTestList()
        {
            testList = new List<FieldNameToTest>();
            testList.Add(new FieldNameToTest("PS04", "1", "是否曾有其他過敏性疾病"));
            testList.Add(new FieldNameToTest("PS04_1", "1", "過敏性鼻炎"));
            testList.Add(new FieldNameToTest("PS04_2", "1", "氣喘"));
            testList.Add(new FieldNameToTest("PS04_3", "1", "異位性皮膚炎"));
            testList.Add(new FieldNameToTest("PS04_4", "1", "蕁麻疹"));
            testList.Add(new FieldNameToTest("FS01", "1", "是否可能發生過食物過敏"));
            testList.Add(new FieldNameToTest("FS05", "1", "是否知道對何種食物過敏"));
            testList.Add(new FieldNameToTest("FS21", "1", "請問是否曾求醫，檢查確定是上述食物過敏"));
            testList.Add(new FieldNameToTest("FS13", "1", "對蛋過敏"));
            testList.Add(new FieldNameToTest("FS15", "1", "對奶過敏"));
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
            int succussefulMatch = 0;
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
                if (Primary.dataRow.Count * repeat >= Match.dataRow.Count) succussefulMatch++;
                if (i == repeat - 1)
                {
                    result.AppendLine("Match Detail");
                    result.Append(SummarizeMatchPools.printCount(matchPools, dataMatcher));
                    result.AppendLine("");
                }
            }
            Console.WriteLine($"calculate Chi square table {name} for {repeat} times");
            string summary = $"Significant result: Primary > Matched:{primaryMore},  Matched > Primary:{matchMore}, Non-Significant:{repeat - primaryMore - matchMore}";
            string summary2 = ($"[match raito] 1:{matchCount}, success matching = {Math.Round(((double)succussefulMatch * 100 / repeat), 1)}%");
            return name + "\r\n" + summary + "\r\n" + summary2 + "\r\n" + result.ToString();
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
            int M = (from q in input.dataRow
                     where q[input.getIndex("CASESEX")] == "1"
                     select q).Count();
            int F = (from q in input.dataRow
                     where q[input.getIndex("CASESEX")] == "0"
                     select q).Count();

            Console.WriteLine($">>data count: {input.dataRow.Count}  M:{M } F:{F}");
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
