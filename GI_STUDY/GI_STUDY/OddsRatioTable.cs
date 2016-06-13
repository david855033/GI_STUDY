using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace GI_STUDY
{
    static class OddsRatioTable
    {
        static List<OddsRatioData> table = new List<OddsRatioData>();
        static string path;
        public static  void addOddRatio(string name, double OR, double LL, double UL)
        {
            table.Add(new OddsRatioData() { name = name, OddsRatio = OR, UL = UL, LL = LL });

        }
        public static void setPath(string path)
        {
            OddsRatioTable.path = path;
        }
        public static void writeToFile()
        {
            using (var sw = new StreamWriter(path, false, Encoding.Default))
            {
                string title = "name\tOR\tLL\tUL";
                sw.WriteLine(title);
                foreach (var data in table)
                {
                    sw.WriteLine(data.name + "\t" + data.OddsRatio + "\t" + data.LL + "\t" + data.UL);
                }
            }
        }
        public static void clear()
        {
            table = new List<OddsRatioData>();
        }

    }

    internal class OddsRatioData
    {
        public string name;
        public double OddsRatio, UL, LL;
    }

}
