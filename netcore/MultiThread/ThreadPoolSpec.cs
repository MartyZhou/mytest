using System;
using System.Threading.Tasks;
using Xunit;

namespace ThreadPoolSpec
{
    public class ThreadPoolSpec
    {
        [Fact]
        public void TestThreadPool()
        {
            Task t = new Task(() =>
            {
                Console.WriteLine("this is a task");
            });

            Assert.Equal<TaskStatus>(TaskStatus.Created, t.Status);

            t.Start();
            t.Wait();
            Assert.Equal<TaskStatus>(TaskStatus.RanToCompletion, t.Status);
        }

        [Fact]
        public void TestThreadPool_TaskRun()
        {
            Task t = Task.Run(() =>
            {
                Task.Delay(200);
                Console.WriteLine("this is a task");
            });

            Task.Delay(20); // give it time for the system to schedule the task to run

            Assert.Equal<TaskStatus>(TaskStatus.Running, t.Status);

            t.Wait();
            Assert.Equal<TaskStatus>(TaskStatus.RanToCompletion, t.Status);
        }
    }
}