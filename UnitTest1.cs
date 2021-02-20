using System;
using System.Threading;
using System.Timers;
using NUnit.Framework;

namespace cpop_client
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            new CpopSubscriber().Subscribe();
            Thread.Sleep(10000);
            Console.WriteLine("exitting");
            Assert.Pass();
        }
    }
}