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
            FileStream filestream = new FileStream(@"D:\GI DATA\out.txt", FileMode.Create);
            var streamwriter = new StreamWriter(filestream);
            streamwriter.AutoFlush = true;
            Console.SetOut(streamwriter);
            Console.SetError(streamwriter);
            Console.WriteLine($"Execution log======================{DateTime.Now}");
            //LogisticRegressionOutPut();
            //study_RR();
            //CountWork();
            study20161207();
            Console.WriteLine($"End at{DateTime.Now}======================");
        }
        static string basefolder;
        static int repeat;

        static void study20161207()
        {

            //load data set
            DataSet originDataSet = DataReader.LoadData(@"D:\GI DATA\FA_19609筆_修改 AGE GROUP_BREAST FEEDING.txt");
            Console.WriteLine("Load Oringinal Data");
            showDataCount(originDataSet);

            //additional field proccess use dataConverter
            originDataSet.addField(new FamilyIDDataConvertor(), "FamilyID");
            originDataSet.addField(new LocationDataConvertor(), "Location");
            originDataSet.addField(new SchoolDataConvertor(), "School");
            originDataSet.addField(new GenderConvertor(), "Gender");
            originDataSet.addField(new GradeDataConvertor(), "Grade");
            originDataSet.addField(new ClassDataConvertor(), " Class");
            originDataSet.addField(new FamilyMemberDataConvertor(), "FamilyMemberType");
            originDataSet.addField(new BlankDataConvertor(), "FatherIndex");
            originDataSet.addField(new BlankDataConvertor(), "MotherIndex");
            originDataSet.addField(new BlankDataConvertor(), "ParentVegetarian");
            originDataSet.addField(new BlankDataConvertor(), "ParentAsthma");
            
            //fill index to static classes
            Criteria.index = originDataSet.index;
            FieldNameToTest.index = originDataSet.index;

            //initialize criterias
            #region criterias
             List<Criteria> HasGenderData = new List<Criteria>()  {
                new Criteria("Gender", "0"),
                new Criteria("Gender", "1"),
            };

             List<Criteria> NoAgeData = new List<Criteria>()  {
                new Criteria("AGE", "9999")
            };

             List<Criteria> isChild_toInclude =
                new List<Criteria>() { new Criteria("FamilyMemberType", "1") };
             List<Criteria> hasFather_toExclude =
                new List<Criteria>() { new Criteria("FatherIndex", "") };
             List<Criteria> hasMother_toExclude =
                new List<Criteria>() { new Criteria("MotherIndex", "") };
             List<Criteria> hasBothParent_toExclude =
                new List<Criteria>() { new Criteria("FatherIndex", ""), new Criteria("MotherIndex", "") };

             List<Criteria> Vegetarian_toInclude = new List<Criteria>()  {
                new Criteria("PS03", "2") ,
                new Criteria("PS03", "3") ,
                new Criteria("PS03", "4") ,
                new Criteria("PS03", "5")
            };
            #endregion

            DataSet processDataSet;

            Console.WriteLine("remove data without Gender");
            processDataSet = originDataSet.select(HasGenderData, null);
            showDataCount(processDataSet);

            Console.WriteLine("remove data without AGE");
            processDataSet = processDataSet.select(null, NoAgeData);
            showDataCount(processDataSet);

            //在學生資料內加入父母的index
            foreach (var thisRow in processDataSet.dataRow)
            {
                int familyMemberTypeFieldIndex = originDataSet.getIndex("FamilyMemberType");
                int familyIDFieldIndex = originDataSet.getIndex("FamilyID");
                int fatherFieledIndex = originDataSet.getIndex("FatherIndex");
                int motherFieledIndex = originDataSet.getIndex("MotherIndex");
                int parentVegetarian = originDataSet.getIndex("ParentVegetarian");
                int parentAsthma = originDataSet.getIndex("ParentAsthma");
                string thisFamilyID = thisRow[familyIDFieldIndex];
                if (thisRow[familyMemberTypeFieldIndex] == "1")
                {
                    int indexOfFather =
                        processDataSet.dataRow.FindIndex(
                            x => x[familyIDFieldIndex] == thisFamilyID && x[familyMemberTypeFieldIndex] == "2");
                    if (indexOfFather >= 0)
                    {
                        thisRow[fatherFieledIndex] = indexOfFather.ToString();
                        string isFatherVegegarian = 
                            processDataSet.dataRow[indexOfFather].matchAnyOfOne(Vegetarian_toInclude) ? "1" : "0";
                        thisRow[parentVegetarian] = isFatherVegegarian;      
                    }

                    int indexOfMother =
                        processDataSet.dataRow.FindIndex(
                            x => x[familyIDFieldIndex] == thisFamilyID && x[familyMemberTypeFieldIndex] == "3");
                    if (indexOfMother >= 0)
                    {
                        thisRow[motherFieledIndex] = indexOfMother.ToString();
                        string isMotherVegegarian =
                           processDataSet.dataRow[indexOfMother].matchAnyOfOne(Vegetarian_toInclude) ? "1" : "0";
                        thisRow[parentVegetarian] = isMotherVegegarian;
                    }
                }
            }

            DataSet ChildDataSet = processDataSet.select(isChild_toInclude, null);
            DataSet ChildWithFatherDataSet =
                ChildDataSet.select(null, hasFather_toExclude);
            DataSet ChildWithMotherDataSet =
                ChildDataSet.select(null, hasMother_toExclude);
            DataSet ChildWithBothParentsDataSet =
                ChildDataSet.select(null, hasBothParent_toExclude);
        }

     
        static void countVegetarianSubGroup(DataSet Input, List<Criteria> AdditionalCriteriaList, string name)
        {
            DataSet dataSet_forCount;
            Console.WriteLine($"*****   [{name}] -- Vegetration Subgroup    *****");
            DataSet Preselected = Input.select(null, null);
            foreach (var criteria in AdditionalCriteriaList)
            {
                List<Criteria> List = new List<Criteria>() { criteria };
                Preselected = Preselected.select(List, null);
            }

            List<Criteria> EggMilkVeg_toInclude = new List<Criteria>()
            {
                new Criteria("PS03", "2"),
            };
            Console.WriteLine("select EggMilkVeg...");
            dataSet_forCount = Preselected.select(EggMilkVeg_toInclude, null);
            showDataCount(dataSet_forCount);

            List<Criteria> EggVeg_toInclude = new List<Criteria>()
            {
                new Criteria("PS03", "3"),
            };
            Console.WriteLine("select EggVeg...");
            dataSet_forCount = Preselected.select(EggVeg_toInclude, null);
            showDataCount(dataSet_forCount);


            List<Criteria> MilkVeg_toInclude = new List<Criteria>()
            {
                new Criteria("PS03", "4"),
            };
            Console.WriteLine("select MilkVeg...");
            dataSet_forCount = Preselected.select(MilkVeg_toInclude, null);
            showDataCount(dataSet_forCount);

            List<Criteria> AllVeg_toInclude = new List<Criteria>()
            {
                new Criteria("PS03", "5"),
            };
            Console.WriteLine("select AllVeg...");
            dataSet_forCount = Preselected.select(AllVeg_toInclude, null);
            showDataCount(dataSet_forCount);
        }
        static void countFoodAllergySubGroup(DataSet Input, List<Criteria> AdditionalCriteriaList, string name)
        {
            DataSet dataSet_forCount;
            Console.WriteLine($"*****   [{name}] -- Food Allergy SubGroup     *****");
            DataSet Preselected = Input.select(null, null);
            foreach (var criteria in AdditionalCriteriaList)
            {
                List<Criteria> List = new List<Criteria>() { criteria };
                Preselected = Preselected.select(List, null);
            }

            List<Criteria> HistoryTaking_toInclude = new List<Criteria>()
            {
                new Criteria("FS22_1", "1"),
            };
            Console.WriteLine("select HistoryTaking...");
            dataSet_forCount = Preselected.select(HistoryTaking_toInclude, null);
            showDataCount(dataSet_forCount);

            List<Criteria> SkinTest_toInclude = new List<Criteria>()
            {
                new Criteria("FS22_2", "1"),
            };
            Console.WriteLine("select SkinTest...");
            dataSet_forCount = Preselected.select(SkinTest_toInclude, null);
            showDataCount(dataSet_forCount);


            List<Criteria> BloodTest_toInclude = new List<Criteria>()
            {
                new Criteria("FS22_3", "1"),
            };
            Console.WriteLine("select BloodTest...");
            dataSet_forCount = Preselected.select(BloodTest_toInclude, null);
            showDataCount(dataSet_forCount);


            List<Criteria> FoodChallenge_toInclude = new List<Criteria>()
            {
                new Criteria("FS22_4", "1"),
            };
            Console.WriteLine("select FoodChallenge...");
            dataSet_forCount = Preselected.select(FoodChallenge_toInclude, null);
            showDataCount(dataSet_forCount);

            List<Criteria> NoTreat_toInclude = new List<Criteria>()
            {
                new Criteria("FS23_1", "1"),
            };
            Console.WriteLine("select NoTreat...");
            dataSet_forCount = Preselected.select(NoTreat_toInclude, null);
            showDataCount(dataSet_forCount);

            List<Criteria> DietControl_toInclude = new List<Criteria>()
            {
                new Criteria("FS23_2", "1"),
            };
            Console.WriteLine("select DietControl...");
            dataSet_forCount = Preselected.select(DietControl_toInclude, null);
            showDataCount(dataSet_forCount);

            List<Criteria> SeekDoctor_toInclude = new List<Criteria>()
            {
                new Criteria("FS23_3", "1"),
            };
            Console.WriteLine("select SeekDoctor...");
            dataSet_forCount = Preselected.select(NoTreat_toInclude, null);
            showDataCount(dataSet_forCount);


        }

        static void CountWork()
        {
            DataSet originDataSet = DataReader.LoadData(@"D:\GI DATA\FA_19609筆_修改 AGE GROUP_BREAST FEEDING.txt");
            showDataCount(originDataSet);
            Criteria.index = originDataSet.index;
            FieldNameToTest.index = originDataSet.index;

            Console.WriteLine("remove data without CASESEX and AGE data...");
            List<Criteria> BadData_toExclude = new List<Criteria>()  {
                new Criteria("CASESEX", "") ,
                new Criteria("AGE", "9999")
            };
            DataSet dataSet_removeNoAgeOrNoSex = originDataSet.select(null, BadData_toExclude);
            showDataCount(dataSet_removeNoAgeOrNoSex);

            Console.WriteLine("select Children...");
            DataSet DataSets_Children = new DataSet();
            for (int i = 0; i < 7; i++)
            {
                List<Criteria> ageGroupToInclude = new List<Criteria>() {
                    new Criteria("AGEGROUP", (i + 1).ToString())
                };
                DataSets_Children = DataSets_Children.joinData(dataSet_removeNoAgeOrNoSex.select(ageGroupToInclude, null));
            }
            showDataCount(DataSets_Children);

            List<Criteria> Vegetarian_toInclude = new List<Criteria>()  {
                new Criteria("PS03", "2") ,
                new Criteria("PS03", "3") ,
                new Criteria("PS03", "4") ,
                new Criteria("PS03", "5")
            };

            Console.WriteLine("select Vegetarian...");
            DataSet dataSet_vegetarian = DataSets_Children.select(Vegetarian_toInclude, null);
            showDataCount(dataSet_vegetarian);

            countVegetarianSubGroup(dataSet_vegetarian, new List<Criteria>() { new Criteria("PS04_2", "1") }, "氣喘");
            countVegetarianSubGroup(dataSet_vegetarian, new List<Criteria>() { new Criteria("PS04_1", "1") }, "Allergic Rhinitis");
            countVegetarianSubGroup(dataSet_vegetarian, new List<Criteria>() { new Criteria("PS04_3", "1") }, "Atopic Dermatitis");
            countVegetarianSubGroup(dataSet_vegetarian, new List<Criteria>() { new Criteria("PS04_4", "1") }, "Urticaria");

            var doctorDiagnosedFoodAllergy = new Criteria("FS21", "1");

            countVegetarianSubGroup(dataSet_vegetarian,
                new List<Criteria>() { new Criteria("FS21", "1"), doctorDiagnosedFoodAllergy }, "Any of food allergy");
            countVegetarianSubGroup(dataSet_vegetarian,
                new List<Criteria>() { new Criteria("FS07", "1"), doctorDiagnosedFoodAllergy }, "魚類過敏");
            countVegetarianSubGroup(dataSet_vegetarian,
                new List<Criteria>() { new Criteria("FS12", "1"), doctorDiagnosedFoodAllergy }, "花生過敏");
            countVegetarianSubGroup(dataSet_vegetarian,
                new List<Criteria>() { new Criteria("FS13", "1"), doctorDiagnosedFoodAllergy }, "對蛋過敏");
            countVegetarianSubGroup(dataSet_vegetarian,
                new List<Criteria>() { new Criteria("FS13_01", "1"), doctorDiagnosedFoodAllergy }, "對蛋白過敏");
            countVegetarianSubGroup(dataSet_vegetarian,
                new List<Criteria>() { new Criteria("FS13_02", "1"), doctorDiagnosedFoodAllergy }, "對蛋黃過敏");
            countVegetarianSubGroup(dataSet_vegetarian,
                new List<Criteria>() { new Criteria("FS15", "1"), doctorDiagnosedFoodAllergy }, "對奶過敏");

            countFoodAllergySubGroup(dataSet_vegetarian,
                new List<Criteria>() { new Criteria("FS21", "1"), doctorDiagnosedFoodAllergy }, "Any of food allergy");
            countFoodAllergySubGroup(dataSet_vegetarian,
                new List<Criteria>() { new Criteria("FS07", "1"), doctorDiagnosedFoodAllergy }, "魚類過敏");
            countFoodAllergySubGroup(dataSet_vegetarian,
                new List<Criteria>() { new Criteria("FS12", "1"), doctorDiagnosedFoodAllergy }, "花生過敏");
            countFoodAllergySubGroup(dataSet_vegetarian,
                new List<Criteria>() { new Criteria("FS13", "1"), doctorDiagnosedFoodAllergy }, "對蛋過敏");
            countFoodAllergySubGroup(dataSet_vegetarian,
                new List<Criteria>() { new Criteria("FS15", "1"), doctorDiagnosedFoodAllergy }, "對奶過敏");
        }

        static void LogisticRegressionOutPut()
        {
            DataSet originDataSet = DataReader.LoadData(@"D:\GI DATA\FA_19609筆_修改 AGE GROUP_BREAST FEEDING.txt");
            showDataCount(originDataSet);
            Criteria.index = originDataSet.index;
            FieldNameToTest.index = originDataSet.index;
            Console.WriteLine("remove data without CASESEX and AGE data...");
            List<Criteria> BadData_toExclude = new List<Criteria>()  {
                new Criteria("CASESEX", "") ,
                new Criteria("AGE", "9999")
            };
            DataSet dataSet_removeNoAgeOrNoSex = originDataSet.select(null, BadData_toExclude);
            showDataCount(dataSet_removeNoAgeOrNoSex);

            Console.WriteLine("select Children...");
            DataSet DataSets_Children = new DataSet();
            DataSets_Children.copyIndexFromDataSet(originDataSet);
            for (int i = 0; i < 7; i++)
            {
                List<Criteria> ageGroupToInclude = new List<Criteria>() {
                    new Criteria("AGEGROUP", (i + 1).ToString())
                };
                DataSets_Children = DataSets_Children.joinData(dataSet_removeNoAgeOrNoSex.select(ageGroupToInclude, null));
            }
            showDataCount(DataSets_Children);


            List<string> outputFields = new List<string>() { "AGEGROUP" };
            outputFields.Add(DataSets_Children.addField(new GenderDataConvertor(), "Gender"));

            outputFields.Add(DataSets_Children.addField(new isVegetarianDataconvertor(), "Vegetarian"));
            outputFields.Add(DataSets_Children.addField(new dietHabitDataConvertor(), "DietHabit"));

            outputFields.Add(DataSets_Children.addField(new BinaryDataConvertor("PS04_1", "AllergicRhinitis"), "AllergicRhinitis"));
            outputFields.Add(DataSets_Children.addField(new BinaryDataConvertor("PS04_2", "Ashtma"), "Asthma"));
            outputFields.Add(DataSets_Children.addField(new BinaryDataConvertor("PS04_3", "AtopicDermatitis"), "AtopicDermatitis"));
            outputFields.Add(DataSets_Children.addField(new BinaryDataConvertor("PS04_4", "Urticaria"), "Urticaria"));
            outputFields.Add(DataSets_Children.addField(new BinaryDataConvertor("FS21", "FoodAllergy"), "FoodAllergy"));

            outputFields.Add(DataSets_Children.addField(new DoubleBinaryDataConvertor("FS21", "FS06", "ShellFish"), "ShellFish"));
            outputFields.Add(DataSets_Children.addField(new DoubleBinaryDataConvertor("FS21", "FS07", "Fish"), "Fish"));
            outputFields.Add(DataSets_Children.addField(new DoubleBinaryDataConvertor("FS21", "FS08", "Meat"), "Meat"));
            outputFields.Add(DataSets_Children.addField(new DoubleBinaryDataConvertor("FS21", "FS09", "Vegetable"), "Vegetable"));
            outputFields.Add(DataSets_Children.addField(new DoubleBinaryDataConvertor("FS21", "FS10", "Fruit"), "Fruit"));
            outputFields.Add(DataSets_Children.addField(new DoubleBinaryDataConvertor("FS21", "FS11", "Nuts"), "Nuts"));
            outputFields.Add(DataSets_Children.addField(new DoubleBinaryDataConvertor("FS21", "FS12", "Peanut"), "Peanut"));
            outputFields.Add(DataSets_Children.addField(new DoubleBinaryDataConvertor("FS21", "FS13", "Egg"), "Egg"));
            outputFields.Add(DataSets_Children.addField(new DoubleBinaryDataConvertor("FS21", "FS14", "Wheat"), "Wheat"));
            outputFields.Add(DataSets_Children.addField(new DoubleBinaryDataConvertor("FS21", "FS15", "Dairy"), "Dairy"));
            outputFields.Add(DataSets_Children.addField(new DoubleBinaryDataConvertor("FS21", "FS16", "Soybean"), "Soybean"));
            outputFields.Add(DataSets_Children.addField(new DoubleBinaryDataConvertor("FS21", "FS17", "BrinedMeat"), "BrinedMeat"));
            outputFields.Add(DataSets_Children.addField(new DoubleBinaryDataConvertor("FS21", "FS18", "Seasoning"), "Seasoning"));
            outputFields.Add(DataSets_Children.addField(new DoubleBinaryDataConvertor("FS21", "FS19", "Alcohol"), "Alcohol"));

            var result = DataSets_Children.outputByField(outputFields);
            using (var sw = new StreamWriter(@"D:\GI DATA\result of field selection.txt", false, Encoding.Default))
            {
                sw.Write(result);
            }
        }
        static void study_RR()
        {
            DataSet originDataSet = DataReader.LoadData(@"D:\GI DATA\FA_19609筆_修改 AGE GROUP_BREAST FEEDING.txt");
            showDataCount(originDataSet);
            Criteria.index = originDataSet.index;
            FieldNameToTest.index = originDataSet.index;

            Console.WriteLine("remove data without CASESEX and AGE data...");
            List<Criteria> BadData_toExclude = new List<Criteria>()  {
                new Criteria("CASESEX", "") ,
                new Criteria("AGE", "9999")
            };
            DataSet dataSet_removeNoAgeOrNoSex = originDataSet.select(null, BadData_toExclude);
            showDataCount(dataSet_removeNoAgeOrNoSex);

            Console.WriteLine("select Children...");
            DataSet DataSets_Children = new DataSet();
            DataSets_Children.copyIndexFromDataSet(originDataSet);
            for (int i = 0; i < 7; i++)
            {
                List<Criteria> ageGroupToInclude = new List<Criteria>() {
                    new Criteria("AGEGROUP", (i + 1).ToString())
                };
                DataSets_Children = DataSets_Children.joinData(dataSet_removeNoAgeOrNoSex.select(ageGroupToInclude, null));
            }
            showDataCount(DataSets_Children);



            List<Criteria> non_Vegetarian_toInclude = new List<Criteria>()  {
                new Criteria("PS03", "1")
            };

            List<Criteria> EggMilkVeg_toInclude = new List<Criteria>()
            {
                new Criteria("PS03", "2") ,
            };

            List<Criteria> EggVeg_toInclude = new List<Criteria>()
            {
                new Criteria("PS03", "3") ,
            };

            List<Criteria> MilkVeg_toInclude = new List<Criteria>()
            {
                new Criteria("PS03", "4") ,
            };

            List<Criteria> Pure_Veg_toInclude = new List<Criteria>()
            {
                new Criteria("PS03", "5") ,
            };


            //Console.WriteLine("select Vegetarian...");
            //DataSet dataSet_vegetarian = DataSets_Children.select(Vegetarian_toInclude, null);
            //showDataCount(dataSet_vegetarian);

            //Console.WriteLine("select non-Vegetarian...");
            //DataSet dataSet_non_vegetarian = DataSets_Children.select(non_Vegetarian_toInclude, null);
            //showDataCount(dataSet_non_vegetarian);

            //Console.WriteLine("select Egg-Milk-Vegetarian...");
            //DataSet dataSet_Egg_Milk_vegetarian = DataSets_Children.select(EggMilkVeg_toInclude, null);
            //showDataCount(dataSet_Egg_Milk_vegetarian);

            //Console.WriteLine("select Egg-Vegetarian...");
            //DataSet dataSet_Egg_vegetarian = DataSets_Children.select(EggVeg_toInclude, null);
            //showDataCount(dataSet_Egg_vegetarian);

            //Console.WriteLine("select Milk-Vegetarian...");
            //DataSet dataSet_Milk_vegetarian = DataSets_Children.select(MilkVeg_toInclude, null);
            //showDataCount(dataSet_Milk_vegetarian);

            //Console.WriteLine("select Pure-Vegetarian...");
            //DataSet dataSet_pure_vegetarian = DataSets_Children.select(Pure_Veg_toInclude, null);
            //showDataCount(dataSet_pure_vegetarian);

            //int agegroups = 7;
            //int firstAgeGroup = 1;
            //DataSet[] dataSet_vegetarian_byAge = new DataSet[agegroups];
            //DataSet[] dataSet_non_vegetarian_byAge = new DataSet[agegroups];
            //for (int i = 0; i < agegroups; i++)
            //{
            //    var AgeToInclude = new List<Criteria>()
            //    {
            //        new Criteria("AGEGROUP", (i + firstAgeGroup).ToString())
            //    };

            //    Console.WriteLine($"select Vegetarian. age group{i + 1}");
            //    dataSet_vegetarian_byAge[i] = dataSet_vegetarian.select(AgeToInclude, null);
            //    showDataCount(dataSet_vegetarian_byAge[i]);

            //    Console.WriteLine($"select non-Vegetarian. age group{i + 1}");
            //    dataSet_non_vegetarian_byAge[i] = dataSet_non_vegetarian.select(AgeToInclude, null);
            //    showDataCount(dataSet_non_vegetarian_byAge[i]);
            //}

            //initializeTestList();

            //repeat = 100;
            //basefolder = $@"D:\GI Data\result Veg Subgrouping";
            //OddsRatioTable.clear();
            //OddsRatioTable.setPath(basefolder + $@"\Odd Ratio Table match=4.txt");

            //DoTestAndWriteResultAllTest(dataSet_vegetarian, dataSet_non_vegetarian, "Veg vs NonVeg");


            //DoTestAndWriteResultAllTest(dataSet_Egg_Milk_vegetarian, dataSet_non_vegetarian, "EggMilkVeg vs NonVeg");
            //DoTestAndWriteResultAllTest(dataSet_Egg_vegetarian, dataSet_non_vegetarian, "EggVeg vs NonVeg");
            //DoTestAndWriteResultAllTest(dataSet_Milk_vegetarian, dataSet_non_vegetarian, "MilkVeg vs NonVeg");
            //DoTestAndWriteResultAllTest(dataSet_pure_vegetarian, dataSet_non_vegetarian, "PureVeg vs NonVeg");

            //for (int i = 0; i < dataSet_vegetarian_byAge.Length; i++)
            //{
            //    DoTestAndWriteResultAllTest(dataSet_vegetarian_byAge[i], dataSet_non_vegetarian_byAge[i], $"Veg vs NonVeg _subgrouping by age {i + 1}");
            //}

            OddsRatioTable.writeToFile();
        }


        static void DoTestAndWriteResultAllTest(DataSet primary, DataSet match, string studyGroup)
        {
            foreach (var fieldnameSetList in testList)
            {
                DoTestAndWriteResult(fieldnameSetList, primary, match, studyGroup);
            }
        }
        static void DoTestAndWriteResult(List<FieldNameToTest> fieldnameSetList, DataSet primary, DataSet match, string studyGroup)
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
            // matchcount = 4;  //*******8
            var s = MatchAndCalculateChiSquare($"{studyGroup} on {fieldnameSetList[0].fieldname}", repeat, primary, match, matchcount, fieldnameSetList);
            string folder = basefolder + $@"\{studyGroup}";
            writeResult(s, folder, $"{fieldnameSetList[0].fieldname}({fieldnameSetList[0].info}).txt");
        }
        static List<List<FieldNameToTest>> testList;
        static void initializeTestList()
        {
            testList = new List<List<FieldNameToTest>>();
            testList.Add(new List<FieldNameToTest>() { new FieldNameToTest("PS04", "1", "是否曾有其他過敏性疾病") });
            testList.Add(new List<FieldNameToTest>() { new FieldNameToTest("PS04_1", "1", "過敏性鼻炎") });
            testList.Add(new List<FieldNameToTest>() { new FieldNameToTest("PS04_2", "1", "氣喘") });
            testList.Add(new List<FieldNameToTest>() { new FieldNameToTest("PS04_3", "1", "異位性皮膚炎") });
            testList.Add(new List<FieldNameToTest>() { new FieldNameToTest("PS04_4", "1", "蕁麻疹") });
            testList.Add(new List<FieldNameToTest>() { new FieldNameToTest("FS01", "1", "是否可能發生過食物過敏") });
            testList.Add(new List<FieldNameToTest>() { new FieldNameToTest("FS05", "1", "是否知道對何種食物過敏") });
            testList.Add(new List<FieldNameToTest>() { new FieldNameToTest("FS21", "1", "請問是否曾求醫，檢查確定是上述食物過敏") });
            testList.Add(new List<FieldNameToTest>() {
                new FieldNameToTest("FS06", "1", "有殼海鮮過敏"),
                new FieldNameToTest("FS21", "1", "請問是否曾求醫，檢查確定是上述食物過敏")});
            testList.Add(new List<FieldNameToTest>() {
                new FieldNameToTest("FS07", "1", "魚類過敏") ,
                new FieldNameToTest("FS21", "1", "請問是否曾求醫，檢查確定是上述食物過敏")});
            testList.Add(new List<FieldNameToTest>() {
                new FieldNameToTest("FS08", "1", "肉類過敏") ,
                new FieldNameToTest("FS21", "1", "請問是否曾求醫，檢查確定是上述食物過敏")});
            testList.Add(new List<FieldNameToTest>() {
                new FieldNameToTest("FS09", "1", "蔬菜過敏") ,
                new FieldNameToTest("FS21", "1", "請問是否曾求醫，檢查確定是上述食物過敏")});
            testList.Add(new List<FieldNameToTest>() {
                new FieldNameToTest("FS10", "1", "水果過敏") ,
                new FieldNameToTest("FS21", "1", "請問是否曾求醫，檢查確定是上述食物過敏")});
            testList.Add(new List<FieldNameToTest>() {
                new FieldNameToTest("FS11", "1", "堅果過敏"),
                new FieldNameToTest("FS21", "1", "請問是否曾求醫，檢查確定是上述食物過敏")});
            testList.Add(new List<FieldNameToTest>() {
                new FieldNameToTest("FS12", "1", "花生過敏") ,
                new FieldNameToTest("FS21", "1", "請問是否曾求醫，檢查確定是上述食物過敏")});
            testList.Add(new List<FieldNameToTest>() {
                new FieldNameToTest("FS13", "1", "對蛋過敏") ,
                new FieldNameToTest("FS21", "1", "請問是否曾求醫，檢查確定是上述食物過敏")});
            testList.Add(new List<FieldNameToTest>() {
                new FieldNameToTest("FS13_01", "1", "對蛋白過敏") ,
                new FieldNameToTest("FS21", "1", "請問是否曾求醫，檢查確定是上述食物過敏")});
            testList.Add(new List<FieldNameToTest>() {
                new FieldNameToTest("FS13_02", "1", "對蛋黃過敏") ,
                new FieldNameToTest("FS21", "1", "請問是否曾求醫，檢查確定是上述食物過敏")});
            testList.Add(new List<FieldNameToTest>() {
                new FieldNameToTest("FS14", "1", "麥類過敏") ,
                new FieldNameToTest("FS21", "1", "請問是否曾求醫，檢查確定是上述食物過敏")});
            testList.Add(new List<FieldNameToTest>() {
                new FieldNameToTest("FS15", "1", "對奶過敏") ,
                new FieldNameToTest("FS21", "1", "請問是否曾求醫，檢查確定是上述食物過敏")});
            testList.Add(new List<FieldNameToTest>() {
                new FieldNameToTest("FS16", "1", "黃豆過敏") ,
                new FieldNameToTest("FS21", "1", "請問是否曾求醫，檢查確定是上述食物過敏")});
            testList.Add(new List<FieldNameToTest>() {
                new FieldNameToTest("FS19", "1", "酒類過敏") ,
                new FieldNameToTest("FS21", "1", "請問是否曾求醫，檢查確定是上述食物過敏")});
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
            List<FieldNameToTest> CalculateField)
        {
            StringBuilder result = new StringBuilder();
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
                ChiSquareCalculator chiSquare = new ChiSquareCalculator(CalculateField, matchPools);
                chiSquare.countPosAndNegInBothGroup();
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

            Console.WriteLine($">>data count: {input.dataRow.Count}  M:{M } F:{F}\r\n");
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
