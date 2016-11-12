using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Xunit;

namespace AsyncSpec
{
    public class AsyncSpec
    {
        [Fact]
        public async Task TestAsync()
        {
            var expensiveTask = Task.Run(() => DoSomethingExpensive());
            var result = await expensiveTask;

            Assert.True(result);
        }

        [Fact]
        public async Task AsyncVoidTest()
        {
            List<object> list = new List<object>();
            DoSomethingStupid(list);
            await Task.Delay(2000);

            Assert.Equal(0, list.Count);
        }

        [Fact]
        public async Task AsyncVoidTest2()
        {
            List<object> list = new List<object>();
            DoSomethingStupid(list);
            await Task.Delay(2020);

            Assert.Equal(1, list.Count);
        }

        [Fact]
        public void ColdTaskSpec()
        {
            Mock<IList<object>> mockList = new Mock<IList<object>>();
            Task task = new Task(() => { });

            Assert.Equal(TaskStatus.Created, task.Status);
        }

        // async void should be only used on the first level of async
        // it has fire and forget feature
        private async void DoSomethingStupid(List<object> list)
        {
            await Task.Delay(2010);
            var o = new object();
            list.Add(o);
        }

        private bool DoSomethingExpensive()
        {
            Task.Delay(2000);

            return true;
        }

        private async Task<bool> DoSomethingExpensiveAsync()
        {
            await Task.Delay(2000);
            return true;
        }
    }
}