using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Calc
{
    public static class Calculator
    {
        public abstract class Entity
        {
            public abstract double Calculate();
        }
        class Number : Entity
        {
            public double Num { get; set; }
            public Number(double num) => Num = num;

            public static implicit operator Number(double num) => new Number(num);
            public static implicit operator double(Number number) => number.Num;

            public override double Calculate() => Num;
        }
        abstract class Operation : Entity { }
        abstract class Binary : Operation
        {
            public Entity Left { get; set; }
            public Entity Right { get; set; }

            public Binary(Entity left = null, Entity right = null) =>
                (Left, Right) = (left, right);
        }
        abstract class Unary : Operation
        {
            public Entity Entity { get; set; }

            public Unary(Entity entity = null) =>
                Entity = entity;
        }

        class Addition : Binary
        {
            public Addition(Entity left = null, Entity right = null) : base(left, right) { }
            public override double Calculate() => Left.Calculate() + Right.Calculate();
        }
        class Subtraction : Binary
        {
            public Subtraction(Entity left = null, Entity right = null) : base(left, right) { }
            public override double Calculate() => Left.Calculate() - Right.Calculate();
        }
        class Multiplication : Binary
        {
            public Multiplication(Entity left = null, Entity right = null) : base(left, right) { }
            public override double Calculate() => Left.Calculate() * Right.Calculate();
        }
        class Division : Binary
        {
            public Division(Entity left = null, Entity right = null) : base(left, right) { }
            public override double Calculate() => Left.Calculate() / Right.Calculate();
        }
        class Exponentiation : Binary
        {
            public Exponentiation(Entity left = null, Entity right = null) : base(left, right) { }
            public override double Calculate() => Math.Pow(Left.Calculate(), Right.Calculate());
        }

        class Negation : Unary
        {
            public Negation(Entity entity = null) : base(entity) { }
            public override double Calculate() => -Entity.Calculate();
        }
        class Sin : Unary
        {
            public Sin(Entity entity = null) : base(entity) { }
            public override double Calculate() => Math.Sin(Entity.Calculate());
        }
        class Cos : Unary
        {
            public Cos(Entity entity = null) : base(entity) { }
            public override double Calculate() => Math.Cos(Entity.Calculate());
        }

        public static double Calculate(string exp)
        {
            if (String.IsNullOrWhiteSpace(exp))
                return 0;

            if (double.TryParse(exp, out double result))
                return result;

            Expression expression = new Expression(exp.Replace(" ", ""));
            Entity treeExp = expression.Calculate();

            return treeExp.Calculate();
        }

        public class Expression
        {
            public string Exp { get; set; }
            int pos = 0;
            public Expression(string exp) => Exp = exp;

            public Entity Calculate()
            {
                pos = 0;
                return ParseAddSub();
            }
            public Entity ParseAddSub()
            {
                Entity result = ParseMulDiv();

                while (true)
                {
                    if (CheckOperation('+')) result = new Addition(result, ParseMulDiv());
                    else if (CheckOperation('-')) result = new Subtraction(result, ParseMulDiv());
                    else return result;

                }
            }
            public Entity ParseMulDiv()
            {
                Entity result = ParseExp();

                while (true)
                {
                    if (CheckOperation('*')) result = new Multiplication(result, ParseExp());
                    else if (CheckOperation('('))
                    {
                        result = new Multiplication(result, ParseAddSub());
                        if (ReadOperation() != ")")
                            throw new FormatException("Отсутствует )");
                    }
                    else if (CheckOperation('/')) result = new Division(result, ParseExp());
                    else return result;
                }
            }
            public Entity ParseExp()
            {
                Entity result = ParseNumberUnaryBrackets();

                while (true)
                {
                    if (CheckOperation('^')) result = new Exponentiation(result, ParseNumberUnaryBrackets());
                    else return result;
                }
            }
            public Entity ParseNumberUnaryBrackets()
            {
                Entity result = null;

                string op = ReadOperation();
                switch (op?.ToLower())
                {
                    case "-": result = new Negation(ParseNumberUnaryBrackets()); break;
                    case "(":
                        result = ParseAddSub();
                        if (ReadOperation() != ")")
                            throw new FormatException("Отсутствует )");
                        break;

                    case "sin":
                        result = new Sin(ParseNumberUnaryBrackets());
                        break;
                    case "cos":
                        result = new Cos(ParseNumberUnaryBrackets());
                        break;

                    case "":
                        int startPos = pos;
                        while (pos < Exp.Length)
                        {
                            char c = Exp[pos];
                            if (Char.IsDigit(c) || Char.ToLower(c) is '.' or ',' or 'e')
                                ++pos;
                            else
                                break;
                        }

                        if (double.TryParse(Exp[startPos..pos], out double num))
                            result = new Number(num);
                        else
                            throw new FormatException($"Ошибка распознавания {Exp[startPos..pos]}");

                        break;
                    case null:
                        throw new FormatException("Отсутствует число");
                }
                if (result == null)
                    throw new FormatException($"Оператора {op} не существует");
                return result;
            }

            string ReadOperation()
            {
                if (pos >= Exp.Length) return null;

                char c = Exp[pos];

                if (c is '-' or '(' or ')')
                {
                    ++pos;
                    return c.ToString();
                }

                int startPos = pos;
                while (pos < Exp.Length)
                {
                    c = Exp[pos];
                    if (Char.IsDigit(c) || c is '.' or ',' or '(')
                        break;
                    else
                        ++pos;
                }

                return Exp[startPos..pos];
            }
            bool CheckOperation(char op)
            {
                if (pos >= Exp.Length) return false;

                if (Exp[pos] == op) { ++pos; return true; }
                return false;
            }
        }
    }
}
