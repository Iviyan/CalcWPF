using System;
using Xunit;
using Calc;

namespace Calc_XUnitTest
{
    public class CalculatorUnitTest
    {
        delegate double CalculateMethod(string exp);
        static readonly CalculateMethod Calculate = Calculator.Calculate;
        [Fact]
        public void Calculate_EmptyString_ReturnsZero()
        {
            var num = Calculate("");

            Assert.Equal(0, num);
        }

        [Theory]
        [InlineData("0", 0)]
        [InlineData("123", 123)]
        public void Calculate_SinglePositiveNumber_ReturnsSameNumber(string input, double expected)
        {
            var num = Calculate(input);

            Assert.Equal(expected, num);
        }

        [Theory]
        [InlineData("-0", 0)]
        [InlineData("-123", -123)]
        public void Calculate_SingleNegativeNumber_ReturnsSameNumber(string input, double expected)
        {
            var num = Calculate(input);

            Assert.Equal(expected, num);
        }

        [Theory]
        [InlineData("1+2", 3)]
        [InlineData("12+2", 14)]
        [InlineData("1+23", 24)]
        [InlineData("18+23", 41)]
        public void Calculate_SumTwoNumbers(string input, double expected)
        {
            var num = Calculate(input);

            Assert.Equal(expected, num);
        }
        
        [Theory]
        [InlineData("2-1", 1)]
        [InlineData("12-2", 10)]
        [InlineData("1-23", -22)]
        [InlineData("18-23", -5)]
        [InlineData("-17-23", -40)]
        public void Calculate_SubstractTwoNumbers(string input, double expected)
        {
            var num = Calculate(input);

            Assert.Equal(expected, num);
        }
        
        [Theory]
        [InlineData("2*1", 2)]
        [InlineData("12*2", 24)]
        [InlineData("1*23", 23)]
        [InlineData("11*11", 121)]
        [InlineData("-11*11", -121)]
        public void Calculate_MultiplyTwoNumbers(string input, double expected)
        {
            var num = Calculate(input);

            Assert.Equal(expected, num);
        }
        
        [Theory]
        [InlineData("1+2*2+1", 6)]
        public void Calculate_CheckCorrectSequenceOfOperations(string input, double expected)
        {
            var num = Calculate(input);

            Assert.Equal(expected, num);
        }
        
        [Theory]
        [InlineData("2-(2-1)", 1)]
        [InlineData("2*(2+3*2)", 16)]
        public void Calculate_CheckExpressionsWithBrackets(string input, double expected)
        {
            var num = Calculate(input);

            Assert.Equal(expected, num);
        }
    }
}
