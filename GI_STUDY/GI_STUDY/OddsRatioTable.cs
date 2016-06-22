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
        static List<ORandRRData> table = new List<ORandRRData>();
        static string path;
        public static void addOddRatio(string name, double OR, double LL, double UL, double RR, double RRLL, double RRUL, int PP, int PN, int MP, int MN, bool significant)
        {
            table.Add(new ORandRRData()
            {
                name = name,
                OddsRatio = OR,
                UL = UL,
                LL = LL,
                RR = RR,
                RRLL = RRLL,
                RRUL = RRUL,
                PP=PP,
                PN=PN,
                MP=MP,
                MN=MN,
                significant=significant
            });

        }
        public static void setPath(string path)
        {
            OddsRatioTable.path = path;
        }

        public static void writeToFile()
        {
            using (var sw = new StreamWriter(path, false, Encoding.Default))
            {
                string title = "name\tOR\t95%CI LL\t95%CI UL\tRR\t95%CI LL\t95%CI UL\tPrimary Positive\tPrimary Negative\tMatch Positive\tMatch Negative\tTest Times\tSignificant Counts";
                sw.WriteLine(title);
                IEnumerable<string> distinctName = (from q in table
                                    select q.name).Distinct();
                foreach (string name in distinctName)
                {
                    var datas = from q in table
                                where q.name == name
                                orderby q.OddsRatio
                                select q;
                    var significantCount = (from q in datas
                                            where q.significant
                                            select q).Count();
                    List < ORandRRData > dataList = new List<ORandRRData>(datas);
                    var data = dataList[dataList.Count / 2];
                    sw.WriteLine(data.name + "\t" + data.OddsRatio + "\t" + data.LL + "\t" + data.UL + "\t" +
                        data.RR + "\t" + data.RRLL + "\t" + data.RRUL
                        + "\t" + data.PP + "\t" + data.PN + "\t" + data.MP + "\t" + data.MN + "\t" + dataList.Count + "\t" + significantCount);
                }
            }
        }

        public static void clear()
        {
            table = new List<ORandRRData>();
        }

    }

    internal class ORandRRData : IComparable
    {
        public string name;
        public double OddsRatio, UL, LL, RR, RRUL, RRLL;
        public int PP, PN, MP, MN;
        public bool significant;
        public int CompareTo(object obj)
        {
            return this.OddsRatio.CompareTo((obj as ORandRRData).OddsRatio);
        }
    }

}
