using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace MonitorSpec
{
    public class MonitorSpec
    {
        [Fact]
        public void Monitor_with_Value_Type()
        {
            // This AggregateException will contain SynchronizationLockException;
            Assert.ThrowsAny<AggregateException>(() =>
            {
                int forLock = 0;

                Task t1 = Task.Run(() =>
                {
                    Task.Delay(200);
                    Monitor.Enter(forLock);
                    forLock++;
                    Monitor.Exit(forLock);
                });

                Task t2 = Task.Run(() =>
                {
                    Task.Delay(200);
                    Monitor.Enter(forLock);
                    forLock++;
                    Monitor.Exit(forLock);
                });

                Task.WaitAll(t1, t2);
            });
        }

        [Fact]
        public void Monitor_with_Object_Type()
        {
            object o = new object();
            int forLock = 0;

            Task t1 = Task.Run(() =>
            {
                Task.Delay(200);
                Monitor.Enter(o);
                forLock++;
                Monitor.Exit(o);
            });

            Task t2 = Task.Run(() =>
            {
                Task.Delay(200);
                Monitor.Enter(o);
                forLock++;
                Monitor.Exit(o);
            });

            Task.WaitAll(t1, t2);

            Assert.Equal<int>(2, forLock);
        }

        [Fact]
        public void InterlockedSpec()
        {
            object o = new object();
            int forLock = 0;

            Task t1 = Task.Run(() =>
            {
                Task.Delay(100);
                Monitor.Enter(o);
                forLock++;
                Assert.Equal<int>(1, forLock);
                Monitor.Exit(o);
            });

            Task.Delay(100);

            Task t2 = Task.Run(() =>
            {
                Task.Delay(200);
                Monitor.Enter(o);
                forLock++;
                Assert.Equal<int>(2, forLock);
                Monitor.Exit(o);
            });

            Task.Delay(100);

            Task t3 = Task.Run(() =>
            {
                Task.Delay(200);
                Monitor.Enter(o);
                forLock++;
                Assert.Equal<int>(3, forLock);
                Monitor.Exit(o);
            });

            Task.WaitAll(t1, t2, t3);

            Assert.Equal<int>(3, forLock);
        }
    }
}