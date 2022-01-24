using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeetCode算法刷题.Test
{
    /// <summary>
    /// 给你一个整数 x ，如果 x 是一个回文整数，返回 true ；否则，返回 false 。
    ///回文数是指正序（从左向右）和倒序（从右向左）读都是一样的整数。例如，121 是回文，而 123 不是
    /// </summary>
    public static class 回文数
    {
        public static bool IsPalindrome(int x)
        {
            if (x < 0)
                return false;
            int rem = 0, y = 0;
            int quo = x;
            while (quo != 0)
            {
                rem = quo % 10;//每次位数 向前进一位 得到 相当于小数点往前走一位 之后的那一位数 
                quo = quo / 10;//然后在 去除那一位数 重新赋值
                y = y * 10 + rem; //然后 在乘以 位的倍数 + 现在小数点后的一位 实现反转
            }
            return y == x;
        }
    }
}
