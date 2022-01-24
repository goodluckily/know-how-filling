using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("-----------------");


            #region 按顺序执行

            //Task task = new Task(() =>
            //{
            //    Console.WriteLine("     第一个任务");
            //});
            //接下来第二个任务
            //task.ContinueWith(t =>
            //{
            //    Console.WriteLine($"     第二个任务");
            //});
            //task.Start();
            #endregion


            #region 任务① 和 任务② 属于同级并行任务
            Task task = new Task(() =>
            {
                Console.WriteLine("第一个任务");
            });
            // 任务①
            task.ContinueWith(t =>
            {
                for (int i = 0; i < 5; i++)
                {
                    Console.WriteLine($"    任务① ");
                }
            });
            // 任务②
            task.ContinueWith(t =>
            {
                for (int j = 0;  j< 5; j++)
                {
                    Console.WriteLine($"    任务②");
                }
            });
            // 任务① 和 任务② 属于同级并行任务
            task.Start();
            #endregion


            #region 两个任务没有从属关系，是独立的
            //Task<int> task = new Task<int>(() =>
            //{
            //    // 非子任务
            //    Task task1 = new Task(() =>
            //    {
            //        for (int i = 0; i < 5; i++)
            //        {
            //            Console.WriteLine($"     内层任务{i}");
            //            Thread.Sleep(TimeSpan.FromSeconds(0.5));
            //        }
            //    });
            //    task1.Start();
            //    return 666;
            //});
            //task.Start();
            //Console.WriteLine($"任务运算结果是：{task.Result}");
            //Console.WriteLine("\n-------------------\n");
            #endregion


            #region 父任务等待子任务完成后，才能算完成任务，然后返回结果
            //TaskCreationOptions.AttachedToParent 需要嵌套关系才能生效
            // 父子任务
            //Task<int> task = new Task<int>(() =>
            //{
            //    // 子任务
            //    Task task1 = new Task(() =>
            //    {
            //        for (int i = 0; i < 5; i++)
            //        {
            //            Console.WriteLine($"     内层任务{i}");
            //            Thread.Sleep(TimeSpan.FromSeconds(0.5));
            //        }
            //    }, TaskCreationOptions.AttachedToParent);
            //    task1.Start();
            //    Console.WriteLine("最外层任务");
            //    return 666;
            //});
            //task.Start();
            //Console.WriteLine($"任务运算结果是：{task.Result}");
            //Console.WriteLine("\n-------------------\n");

            #endregion


            // 父子任务
            //Task<int> task = new Task<int>(() =>
            //{
            //    // 子任务
            //    Task task1 = new Task(() =>
            //    {
            //        Thread.Sleep(TimeSpan.FromSeconds(1));
            //        Console.WriteLine("     内层任务1");
            //        Thread.Sleep(TimeSpan.FromSeconds(0.5));
            //    }, TaskCreationOptions.AttachedToParent);
            //    // 子 子任务
            //    task1.ContinueWith(t =>
            //    {
            //        Thread.Sleep(TimeSpan.FromSeconds(1));
            //        Console.WriteLine("     内层任务1====>内层任务2");
            //        Thread.Sleep(TimeSpan.FromSeconds(0.5));
            //    }, TaskContinuationOptions.AttachedToParent);
            //    task1.Start();
            //    Console.WriteLine("最外层任务");
            //    return 666;
            //});
            //task.Start();
            //Console.WriteLine($"任务运算结果是：{task.Result}");
            //Console.WriteLine("\n-------------------\n");

            #region 相当于多个任务，生成一个任务

            //返回的 Task 对象可以用来判断任务执行情况。

            //List<Task<int>> tasks = new List<Task<int>>();
            //for (int i = 0; i < 5; i++)
            //    tasks.Add(Task.Run<int>(() =>
            //    {
            //        Console.WriteLine($"任务开始执行");
            //        return new Random().Next(0, 10);
            //    }));
            //Task<int[]> taskOne = Task.WhenAll(tasks);
            //foreach (var item in taskOne.Result)
            //{
            //    Console.WriteLine(item);
            //} 
            #endregion


            //Task.WaitAll() 会让当前线程等待所有任务执行完毕。并且 Task.WaitAll() 是没有泛型的，也么没有返回结果。
            //并行任务的 Task.WhenAny Task.WhenAny() 和 Task.WhenAll() 使用上差不多，
            //Task.WhenAll() 当所有任务都完成时，才算完成，
            //Task.WhenAny() 只要其中一个任务完成，都算完成

            //List<Task> tasks = new List<Task>();
            //for (int i = 0; i < 5; i++) 
            //{
            //    int tmps = i;
            //    tasks.Add(Task.Run(() =>
            //    {
            //        Console.WriteLine($"i = {tmps}");
            //    }));
            //}

            //Task taskOne = Task.WhenAll(tasks);
            //taskOne.Wait();


            // 父子任务

            Console.ReadKey();
        }
    }
}
