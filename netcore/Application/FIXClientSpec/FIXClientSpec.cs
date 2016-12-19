using System.IO;
using System.Text;
using Xunit;
using SmallFIX;
using System;
using System.Reflection;

namespace FIXClientSpec
{
    public class FIXClientSpec
    {
        public const string SOH = "\u0001";
        [Fact]
        public void GenerateTestMessage()
        {
            string message = "8=FIX.4.2\u00019=178\u000135=8\u000149=PHLX\u000156=PERS\u000152=20071123-05:30:00.000\u000111=ATOMNOCCC9990900\u000120=3\u0001150=E\u000139=E\u000155=MSFT\u0001167=CS\u000154=1\u000138=15\u000140=2\u000144=15\u000158=PHLX EQUITY TESTING\u000159=0\u000147=C\u000132=0\u000131=0\u0001151=15\u000114=0\u00016=0\u000110=128\u0001";
            byte[] rawData = Encoding.UTF8.GetBytes(message);
            FileStream fileStream = new FileStream("./Application/FIXClientSpec/TestData/simplemessage.txt", FileMode.Create);
            fileStream.Write(rawData, 0, rawData.Length);
            fileStream.Flush();
        }

        [Fact]
        public void GetFIXTagFromMessage()
        {
            FIXMessage message = new FIXMessage();
            message.BeginString = "FIX 4.2";
            message.AvgPx = 12.21F;

            Type type = message.GetType();
            foreach(var pi in type.GetProperties())
            {
                
            }
        }
    }
}