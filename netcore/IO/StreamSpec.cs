//using System;
//using System.IO;
//using System.Xml;
//using System.Text;
//using Xunit;
//using System.Threading.Tasks;

//namespace netcore.IO
//{
//    public class StreamSpec
//    {
//        [Fact]
//        public async Task StreamOpen()
//        {
//            using (FileStream fileStream = new FileStream("./Xml/bbcseed.xml", FileMode.Open))
//            {
//                Console.WriteLine(fileStream.Name);
//                Console.WriteLine(string.Format("The current position is {0}", fileStream.Position));

//                Console.WriteLine(Convert.ToChar(fileStream.ReadByte()));
//                Console.WriteLine(string.Format("The current position is {0}", fileStream.Position));

//                byte[] buffer = new byte[10];
//                fileStream.Read(buffer, 0, 10);

//                Console.WriteLine(Encoding.UTF8.GetString(buffer));

//                for(int i = 0; i < buffer.Length; i++)
//                {
//                    Console.WriteLine(Convert.ToChar(buffer[i]));
//                }

//                Console.WriteLine(string.Format("The current position is {0}", fileStream.Position));

//                byte[] buffer2 = new byte[100];
//                await fileStream.ReadAsync(buffer2, 0, 100).ConfigureAwait(false);

//                Console.WriteLine(Encoding.UTF8.GetString(buffer2));
//                Console.WriteLine(string.Format("The current position is {0}", fileStream.Position));
//            }
//            // Assert.Equal("rss", xml.ChildNodes[2].Name);
//        }
//    }
//}