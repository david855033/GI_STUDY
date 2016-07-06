using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GI_STUDY
{
    interface IDaterConvertor
    {
        string getFieldContent(string[] row, Dictionary<string, int> index);
    }

    class GenderDataConvertor : IDaterConvertor
    {
        public string getFieldContent(string[] row, Dictionary<string, int> index)
        {
            if (row[index["CASESEX"]] == "1")
            {
                return "MALE";
            }
            return "FEMALE";
        }
    }

    class BinaryDataConvertor : IDaterConvertor
    {
        string field;
        string name;

        public BinaryDataConvertor(string field, string name)
        {
            this.field = field; this.name = name;
        }
        public string getFieldContent(string[] row, Dictionary<string, int> index)
        {
            if (row[index[field]] == "1")
            {
                return name;
            }
            return $"non-{name}";
        }
    }

    class DoubleBinaryDataConvertor : IDaterConvertor
    {
        string field1, field2;
        string name;

        public DoubleBinaryDataConvertor(string field1, string field2, string name)
        {
            this.field1 = field1; this.field2 = field2; this.name = name;
        }
        public string getFieldContent(string[] row, Dictionary<string, int> index)
        {
            if (row[index[field1]] == "1" && row[index[field2]] == "1")
            {
                return name;
            }
            return $"non-{name}";
        }
    }

    class isVegetarianDataconvertor : IDaterConvertor
    {
        public string getFieldContent(string[] row, Dictionary<string, int> index)
        {
            string PS03 = row[index["PS03"]];
            if (PS03 == "2" || PS03 == "3" || PS03 == "4" || PS03 == "5")
            {
                return "Vegetarian";
            }
            return "non-Vegetarian";

        }
    }
    class dietHabitDataConvertor : IDaterConvertor
    {
        public string getFieldContent(string[] row, Dictionary<string, int> index)
        {
            string PS03 = row[index["PS03"]];
            if (PS03 == "1")
            {
                return "RegularDiet";
            }
            else if (PS03 == "2")
            {
                return "EggMilkVeg";
            }
            else if (PS03 == "3")
            {
                return "EggVeg";
            }
            else if (PS03 == "4")
            {
                return "MilkVeg";
            }
            else if (PS03 == "5")
            {
                return "PureVeg";
            }
            return "OtherDietHabit";
        }
    }

}
