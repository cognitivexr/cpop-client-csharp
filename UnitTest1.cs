using System;
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
            Console.WriteLine("exitting");
            Assert.Pass();
        }
    }
}