﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using static System.Threading.Thread;
using static System.Console;

namespace PreventCompetitionByLock
{
    class Program
    {
        static void Main(string[] args)
        {
            WriteLine("Incorrect counter");
            var c = new Counter();

            var t1 = new Thread(() => TestCounter(c));
            var t2 = new Thread(() => TestCounter(c));
            var t3 = new Thread(() => TestCounter(c));
            t1.Start();
            t2.Start();
            t3.Start();
            t1.Join();
            t2.Join();
            t3.Join();

            WriteLine($"Total count:{c.Count}");
            WriteLine("--------------------");

            WriteLine("Correct counter");

            var c1 = new CounterWithLock();

            t1 = new Thread(() => TestCounter(c1));
            t2 = new Thread(() => TestCounter(c1));
            t3 = new Thread(() => TestCounter(c1));
            t1.Start();
            t2.Start();
            t3.Start();
            t1.Join();
            t2.Join();
            t3.Join();

            WriteLine($"Total count:{c1.Count}");
            Read();
        }
        static void TestCounter(CounterBase c)
        {
            for (int i = 0; i < 100000; i++)
            {
                c.Increment();
                c.Decrement();
            }
        }
    }
    abstract class CounterBase
    {
        public abstract void Increment();
        public abstract void Decrement();
    }
    class Counter : CounterBase
    {
        public int Count { get; private set; }

        public override void Increment()
        {
            Count++;
        }
        public override void Decrement()
        {
            Count--;
        }
    }
    class CounterWithLock : CounterBase
    {
        private readonly object _syncRoot = new object();
        public int Count { get; private set; }
        public override void Increment()
        {
            lock (_syncRoot)
            {
                Count++;
            }
        }
        public override void Decrement()
        {
            lock (_syncRoot)
            {
                Count--;
            }
        }
    }
}
