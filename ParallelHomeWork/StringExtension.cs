using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParallelHomeWork
{
    public static class StringExtension
    {
        public static string SortStringLines(this string str)
        {
            string[] subStr = str.Split('\n');
            for (int i = 0; i < subStr.Length - 2; i++)
            {
                for (int l = 0; l < subStr.Length - i - 2; l++)
                {
                    if (subStr[l][0] > subStr[l + 1][0])
                    {
                        string tmp = subStr[l];
                        subStr[l] = subStr[l + 1];
                        subStr[l + 1] = tmp;
                    }
                }
            }
            string tmp1 = "";
            foreach (var item in subStr)
            {
                tmp1 += item;
                tmp1 += '\n';
            }
            return tmp1;
        }

        public static List<int> ExtractNumbers(this string str)
        {
            string tmp = new string(str.Where(c => Char.IsDigit(c) || c == ' ').ToArray());
            var list = tmp.Split(' ').Where(s => s != "").ToArray();
            List<int> result = new List<int>();
            foreach (var item in list)
            {
                result.Add(Int32.Parse(item));
            }
            return result;
        }
    }
}
