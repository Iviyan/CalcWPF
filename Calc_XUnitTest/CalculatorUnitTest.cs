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
        [InlineData("1+2", 1+2)]
        [InlineData("12+2", 12+2)]
        [InlineData("1+23", 1+23)]
        [InlineData("18+23", 18+23)]
        [InlineData("-18+23", -18+23)]
        [InlineData("-18,7+23,3", -18.7+23.3)]
        public void Calculate_SumTwoNumbers(string input, double expected)
        {
            var num = Calculate(input);
            Assert.Equal(expected, num);
        }
        
        [Theory]
        [InlineData("2-1", 2-1)]
        [InlineData("18-23", 18-23)]
        [InlineData("-17-23", -17-23)]
        public void Calculate_SubstractTwoNumbers(string input, double expected)
        {
            var num = Calculate(input);
            Assert.Equal(expected, num);
        }
        
        [Theory]
        [InlineData("2*1", 2*1)]
        [InlineData("11*11,6", 11*11.6)]
        [InlineData("-11*11", -11*11)]
        [InlineData("-15,8*-113", -15.8*-113)]
        public void Calculate_MultiplyTwoNumbers(string input, double expected)
        {
            var num = Calculate(input);
            Assert.Equal(expected, num);
        }
        
        [Theory]
        [InlineData("2/4", 2d/4)]
        [InlineData("11/11", 11/11)]
        [InlineData("-7/2,5", -7/2.5)]
        [InlineData("-15/-113", -15d/-113)]
        public void Calculate_DivideTwoNumbers(string input, double expected)
        {
            var num = Calculate(input);
            Assert.Equal(expected, num);
        }
        
        [Theory]
        [InlineData("2^4", 16)]
        [InlineData("-3^0", 1)]
        [InlineData("6,25^-2,5", 0.01024)]
        public void Calculate_CheckExponentiation(string input, double expected)
        {
            var num = Calculate(input);
            Assert.Equal(expected, num);
        }
        
        [Theory]
        [InlineData("3!", 6)]
        [InlineData("(3!!/3!/4!)!", 120)]
        public void Calculate_CheckFactorial(string input, double expected)
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
        [InlineData("2((2+3*2)(2^2))", 64)]
        public void Calculate_CheckExpressionsWithBrackets(string input, double expected)
        {
            var num = Calculate(input);
            Assert.Equal(expected, num);
        }
        
        [Theory]
        [InlineData("sin(0)", 0)]
        [InlineData("cos(0)", 1)]
        [InlineData("tan(0)", 0)]
        [InlineData("cot(pi/4)", 1)]
        public void Calculate_CheckTrigonometricFunctions(string input, double expected)
        {
            var num = Calculate(input);
            Assert.Equal(expected, num);
        }
        
        [Theory]
        [InlineData("sqrt(4)", 2)]
        [InlineData("sqrt3(27)", 3)]
        public void Calculate_CheckSqrt(string input, double expected)
        {
            var num = Calculate(input);
            Assert.Equal(expected, num);
        }
    }
}
