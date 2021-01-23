using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Calc
{
    public static class Calculator
    {
        public static bool IsInt(double num) => Math.Abs(num % 1) < 1E-14;
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
        class Sqrt : Binary
        {
            public Sqrt(Entity left = null, Entity right = null) : base(left, right) { }
            public override double Calculate() => Math.Pow(Left.Calculate(), 1d / Right.Calculate());
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
        class Tan : Unary
        {
            public Tan(Entity entity = null) : base(entity) { }
            public override double Calculate() => Math.Tan(Entity.Calculate());
        }
        class Cot : Unary
        {
            public Cot(Entity entity = null) : base(entity) { }
            public override double Calculate() => 1d / Math.Tan(Entity.Calculate());
        }
        class Factorial : Unary
        {
            public Factorial(Entity entity = null) : base(entity) { }
            public override double Calculate()
            {
                double res = 1;
                var num_ = Entity.Calculate();

                if (!IsInt(num_) || num_ < 0)
                    throw new FormatException($"Невозможно вычислить факториал из {num_}");

                int num = (int)num_;
                for (int i = 1; i < num; i++, res *= i) ;

                return res;
            }
        }

        public static double Calculate(string exp)
        {
            if (String.IsNullOrWhiteSpace(exp))
                return 0;

            if (double.TryParse(exp, out double result))
                return result;

            Expression expression = new Expression(exp.Replace(" ", ""));
            Entity treeExp = expression.Calculate();

            return Math.Round(treeExp.Calculate(), 14);
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
                Entity result = ParseExpFact();

                while (true)
                {
                    if (CheckOperation('*')) result = new Multiplication(result, ParseExpFact());
                    else if (CheckOperation('('))
                    {
                        result = new Multiplication(result, ParseAddSub());
                        if (ReadOperation() != ")")
                            throw new FormatException("Отсутствует )");
                    }
                    else if (CheckOperation('/')) result = new Division(result, ParseExpFact());
                    else return result;
                }
            }
            public Entity ParseExpFact()
            {
                Entity result = ParseNumberUnaryBracketsFunctions();

                while (true)
                {
                    if (CheckOperation('^')) result = new Exponentiation(result, ParseNumberUnaryBracketsFunctions());
                    if (CheckOperation('!')) result = new Factorial(result);
                    else return result;
                }
            }
            public Entity ParseNumberUnaryBracketsFunctions()
            {
                Entity result = null;

                string op = ReadOperation();
                switch (op?.ToLower())
                {
                    case "-": result = new Negation(ParseNumberUnaryBracketsFunctions()); break;
                    case "(":
                        result = ParseAddSub();
                        if (!CheckOperation(')'))
                            throw new FormatException("Отсутствует )");
                        break;
                    case "sqrt":
                        if (CheckOperation('(', false))
                        {
                            result = new Sqrt(ParseNumberUnaryBracketsFunctions(), new Number(2));
                        }
                        else
                        {
                            var result_ = new Sqrt();
                            if (pos >= Exp.Length || !Char.IsDigit(Exp[pos]) || !TryReadNaturalNumber(out double num))
                                throw new FormatException("Степень корня должна быть натуральным чисом");
                            else
                                result_.Right = new Number(num);

                            result_.Left = ParseNumberUnaryBracketsFunctions();
                            result = result_;
                        }
                        break;

                    case "sin":
                        result = new Sin(ParseNumberUnaryBracketsFunctions());
                        break;
                    case "cos":
                        result = new Cos(ParseNumberUnaryBracketsFunctions());
                        break;
                    case "tan":
                    case "tg":
                        result = new Tan(ParseNumberUnaryBracketsFunctions());
                        break;
                    case "cot":
                    case "ctg":
                        result = new Cot(ParseNumberUnaryBracketsFunctions());
                        break;
                    case "pi":
                        result = new Number(Math.PI);
                        break;
                    case "e":
                        result = new Number(Math.E);
                        break;

                    case "":
                        result = new Number(ReadNumber());
                        break;
                    case null:
                        throw new FormatException("Отсутствует число");
                }
                if (result == null)
                    throw new FormatException($"Оператора {op} не существует");
                return result;
            }

            double ReadNumber()
            {
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
                    return num;
                else
                    throw new FormatException($"Ошибка распознавания {Exp[startPos..pos]}");
            }
            bool TryReadNaturalNumber(out double number)
            {
                number = 0;
                int startPos = pos;

                while (pos < Exp.Length)
                {
                    char c = Exp[pos];
                    if (Char.IsDigit(c))
                        ++pos;
                    else
                    {
                        if (c is '.' or ',' or 'e' or 'E')
                            return false;
                        break;
                    }
                }

                if (double.TryParse(Exp[startPos..pos], out number))
                    return true;
                else
                    throw new FormatException($"Ошибка распознавания {Exp[startPos..pos]}");
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
                    if (Char.IsDigit(c) || ".,()+-*/^!".Contains(c))
                        break;
                    else
                        ++pos;
                }

                return Exp[startPos..pos];
            }
            bool CheckOperation(char op, bool read = true)
            {
                if (pos >= Exp.Length) return false;

                if (Exp[pos] == op) { if (read) ++pos; return true; }
                return false;
            }
        }
    }
}
