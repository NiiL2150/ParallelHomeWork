using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ParallelHomeWork
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int tmp = Int32.Parse(Console.ReadLine());
            if (tmp == 1) { Task1(); }
            else if (tmp == 2) { Task2(); }
            else if (tmp == 3) { Task3(); }
        }

        static void Task1()
        {
            Task<long> task = Task.Run(() =>
            {
                int factNum = Int32.Parse(Console.ReadLine());
                if (factNum > 20) { factNum = 20; }
                long factRes = 1;
                Parallel.For(2, factNum + 1, (item) =>
                {
                    factRes *= item;
                });
                return factRes;
            });
            task.Wait();
            Console.WriteLine($"Factorial: {task.Result}");

            Task<int> task1 = Task.Run(() =>
            {
                string tmp = task.Result.ToString();
                return tmp.Length;
            });
            Task<int> task2 = Task.Run(() =>
            {
                int tmp = 0;
                long taskResCopy = task.Result;
                Parallel.For(0, task1.Result, (item) =>
                {
                    tmp += (int)(taskResCopy % 10);
                    taskResCopy /= 10;
                });
                return tmp;
            });
            Task.WaitAll(task1, task2);
            Console.WriteLine($"Count: {task1.Result}");
            Console.WriteLine($"Sum: {task2.Result}");
        }

        static void Task2()
        {
            object tmp = new object();
            string strTmp = tmp as string;
            int a = Int32.Parse(Console.ReadLine());
            int b = Int32.Parse(Console.ReadLine());
            Task task = Task.Run(() =>
            {
                Parallel.For(a, b + 1, (item) =>
                {
                    Parallel.For(1, 11, (item2) =>
                    {
                        lock (tmp)
                        {
                            strTmp += $"{item} * {item2} = {item * item2}\n";
                        }
                    });
                });
            });
            task.Wait();
            strTmp = strTmp.SortStringLines();
            Console.WriteLine(strTmp);
            Parallel.Invoke(async () =>
            {
                await File.WriteAllTextAsync("log.txt", strTmp);
            });
            task.Wait();
        }

        static void Task3()
        {
            string file = "log.txt";
            string str = File.ReadAllText(file);
            List<int> list = str.ExtractNumbers();
            List<long> longList = new List<long>();

            Task task = Task.Run(() =>
            {
                Parallel.ForEach(list, (item) =>
                {
                    int factNum = item;
                    long factRes = 1;
                    Parallel.For(2, factNum + 1, (item2) =>
                    {
                        factRes *= item2;
                    });
                    longList.Add(factRes);
                });
            });
            task.Wait();
            Parallel.ForEach(list, (item) =>
            {
                Console.WriteLine($"original {item}");
            });
            Parallel.ForEach(longList, (item) =>
            {
                Console.WriteLine($"factorial {item}");
            });
        }
    }
}
