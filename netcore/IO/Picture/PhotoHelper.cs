using System;
using System.Threading.Tasks;

namespace Cluj.Photo
{
    public class PhotoHelper
    {
        public static void Main()
        {
            Console.WriteLine("PhotoHelper is running.");

            var processTask = PhotoProcessor.Process();
            Task.WaitAll(processTask);

            Console.WriteLine("PhotoHelper finished working");
            Console.ReadKey();
        }
    }
}