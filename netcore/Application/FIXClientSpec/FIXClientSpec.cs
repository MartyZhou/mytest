namespace FIXClientSpec
{
    public class FIXClientSpec
    {
        [Fact]
        public void GenerateTestMessage()
        {
            string message = "8=FIX.4.2 | 9=178 | 35=8 | 49=PHLX | 56=PERS | 52=20071123-05:30:00.000 | 11=ATOMNOCCC9990900 | 20=3 | 150=E | 39=E | 55=MSFT | 167=CS | 54=1 | 38=15 | 40=2 | 44=15 | 58=PHLX EQUITY TESTING | 59=0 | 47=C | 32=0 | 31=0 | 151=15 | 14=0 | 6=0 | 10=128 | ";
            char[] messageBytes = message.ToCharArray();
            FileStream fileStream = new FileStream("./TestData/simplemessage.txt", FileMode.Create);
        }
    }
}