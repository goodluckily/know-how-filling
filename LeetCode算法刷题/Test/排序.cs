using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeetCode算法刷题.Test
{
    public static class 排序
    {
        public static void paixuLeftRight(int num) 
        {
            var arry = new int[] { 1, 3, 2, 6, 5, 7, 1, 2, 5, 9 };
            var eor = 0;
            for (int i = 0; i < arry.Length; i++)
            {
                var thisI = arry[eor];
                if (thisI <= num)
                {
                    arry[i] = arry[eor];
                }
                eor = i + 1;
            }
            Console.WriteLine(arry);

        }
    }
}
