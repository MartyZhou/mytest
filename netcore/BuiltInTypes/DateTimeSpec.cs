using System;
using Xunit;

namespace netcore.BuiltInTypes
{
    public class DateTimeSpec
    {
        [Fact]
        public void DateTimeDeclare()
        {
            DateTime date = new DateTime(2016, 10, 27);

            Assert.Equal(27, date.Day);
        }

        [Fact]
        // more format here https://msdn.microsoft.com/en-us/library/8kb3ddd4%28v=vs.110%29.aspx?f=255&MSPPError=-2147217396
        public void DateTimeFormat()
        {
            DateTime date = new DateTime(2016, 10, 27);
            string dateString = date.ToString("yyyyMMdd");
            string dateString2 = string.Format("{0:yyyyMMdd}", date);

            Assert.Equal("20161027", dateString);
            Assert.Equal("20161027", dateString2);
        }
    }
}