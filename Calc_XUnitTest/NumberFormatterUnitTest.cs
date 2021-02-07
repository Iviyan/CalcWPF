using Xunit;
using Calc;
using System.Windows;
using System;

namespace Calc_XUnitTest
{
    public class NumberFormatterUnitTest
    {
        [Theory]
        [InlineData("0", "0")]
        [InlineData("123", "123")]
        [InlineData("12345", "12 345")]
        [InlineData("123456", "123 456")]
        public void NumberFormat_TestDECNumberFormat(string input, string expected)
        {
            var res = NumberFormatter.FormatNumber(input, 10);
            Assert.Equal(expected, res);
        }
        [Theory]
        [InlineData("0", "0")]
        [InlineData("1111", "1111")]
        [InlineData("100101", "10 0101")]
        [InlineData("11111111", "1111 1111")]
        public void NumberFormat_TestBINNumberFormat(string input, string expected)
        {
            var res = NumberFormatter.FormatNumber(input, 2);
            Assert.Equal(expected, res);
        }
    }
}
