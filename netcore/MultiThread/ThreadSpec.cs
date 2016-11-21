using System;
using System.Threading;
using Xunit;

namespace ThreadSpec
{
    public class ThreadSpec
    {
        [Fact]
        public void StartThread()
        {
            Thread thread = new Thread(new ThreadStart(() =>
            {
                Console.WriteLine("code executed in a new thread");
            }));

            thread.Start();
        }

        [Fact]
        public void StartThreadException()
        {
            Assert.ThrowsAny<ThreadStateException>(() =>
                        {

                            Thread thread = new Thread(new ThreadStart(() =>
                                        {
                                            Console.WriteLine("code executed in a new thread");
                                        }));

                            thread.Start();
                            thread.Start();
                        });
        }

        [Fact]
        public void StartThreadWithParameter()
        {
            Thread thread = new Thread(new ThreadStart(() =>
            {
                Console.WriteLine("code executed in a new thread");
            }));

            thread.Start();
        }
    }
}