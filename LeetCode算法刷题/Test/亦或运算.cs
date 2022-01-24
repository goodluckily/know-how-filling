using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeetCode算法刷题.Test
{
    public static class 亦或运算
    {
        public static void optionData()
        {


            var a = 11;
            var b = 22;

            //1.请听题 在不使用第三个变量的情况下 交换双方

            //解法一
            a = a + b;//a = 33
            b = a - b;//b = (33 - 22) = 11;
            a = a - b;//a = (33 - 11) = 22;
            Console.WriteLine($"a:{a}");
            Console.WriteLine($"B:{b}");

            var x = 11;
            var y = 22;

            //^ 亦或运算 相同为0 相异为1   (相同得0，相异得1)
            //  a ^ b = ?
            //  0 ^ 1 = 1
            //  0 ^ 0 = 0
            //  1 ^ 0 = 1
            //  1 ^ 1 = 0


            //前提是 他们的内存地址 不在一个地方 要不然 会磨为0;
            //相关特性
            //1.  0^N = N    N^N =0   //0和任何数^都是N   N和N^ 都是0

            //2. ^运算满足交换率和结合率
            //a^b = b^a
            //a^b^c = a^(b^c)
            //3.同一批数^ 结果都是一致的

            x = x ^ y;// x = 11^22, y=22
            y = x ^ y;// 11^22^22   y=11
            x = x ^ y;// 11^22^11   x=22
            Console.WriteLine($"x:{x}");
            Console.WriteLine($"y:{y}");


            //1.
            var eor = 0;
            var arry = new int[] { 4, 6, 8, 10, 11,22,44,24,28,100};
            for (int i = 0; i < arry.Length; i++)
            {
                eor ^= arry[i];
            }
            Console.WriteLine($"eor:{eor}");
        }
    }
}
