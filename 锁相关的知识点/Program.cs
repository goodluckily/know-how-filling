using System;
using System.Collections.Generic;
using System.Threading;

namespace 锁相关的知识点
{
    class Program
    {

        //ReaderWriterLock 类：定义支持单个写线程和多个读线程的锁。
        //ReaderWriterLock readerWriterLock = new ReaderWriterLock();


        //ReaderWriterLockSlim 类：表示用于管理资源访问的锁定状态，可实现多线程读取或进行独占式写入访问
        //ReaderWriterLockSlim readerWriterLockSlim = new ReaderWriterLockSlim();



        //两者的 API 十分接近，而且 ReaderWriterLockSlim 相对 ReaderWriterLock 来说 更加安全


        private static ReaderWriterLockSlim ReaderWriterLockSlim = new ReaderWriterLockSlim();


        //===================================================================*******=========================================================
        //===================================================================***{start}****======================================================
        //===================================================================*******=========================================================
        //EnterReadLock() 尝试进入读取模式锁定状态。
        //EnterUpgradeableReadLock()  尝试进入可升级模式锁定状态。
        //EnterWriteLock()    尝试进入写入模式锁定状态。
        //ExitReadLock()  减少读取模式的递归计数，并在生成的计数为 0（零）时退出读取模式。
        //ExitUpgradeableReadLock()   减少可升级模式的递归计数，并在生成的计数为 0（零）时退出可升级模式。
        //ExitWriteLock() 减少写入模式的递归计数，并在生成的计数为 0（零）时退出写入模式。
        //TryEnterReadLock(Int32) 尝试进入读取模式锁定状态，可以选择整数超时时间。
        //TryEnterReadLock(TimeSpan)  尝试进入读取模式锁定状态，可以选择超时时间。
        //TryEnterUpgradeableReadLock(Int32)  尝试进入可升级模式锁定状态，可以选择超时时间。
        //TryEnterUpgradeableReadLock(TimeSpan)   尝试进入可升级模式锁定状态，可以选择超时时间。
        //TryEnterWriteLock(Int32)    尝试进入写入模式锁定状态，可以选择超时时间。
        //TryEnterWriteLock(TimeSpan) 尝试进入写入模式锁定状态，可以选择超时时间。
        //===================================================================*******=========================================================
        //===================================================================***{end}****======================================================
        //===================================================================*******=========================================================



        //EnterReadLock() / TryEnterReadLock 和 ExitReadLock() 成对出现。
        //EnterWriteLock() / TryEnterWriteLock() 和 ExitWriteLock() 成对出现。

        //EnterUpgradeableReadLock() 进入可升级的读模式锁定状态。
        //EnterReadLock() 使用 EnterUpgradeableReadLock() 进入升级状态，在恰当时间点 通过 EnterWriteLock() 进入写模式。（也可以倒过来）


        //===================================================================*******=========================================================
        //===================================================================***{end}****======================================================
        //===================================================================*******=========================================================

        static void Main(string[] args)
        {
            Console.WriteLine("=============");

            try
            {
                ReaderWriterLockSlim.EnterReadLock();           // 获取读取锁
            }
            catch { }
            finally
            {
                ReaderWriterLockSlim.ExitReadLock();            // 释放读取锁
            }

            Console.ReadKey();

        }
    }
}
