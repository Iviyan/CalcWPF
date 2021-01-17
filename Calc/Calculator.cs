using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Calc
{
    public static class Calculator
    {
        enum Operation : byte
        {
            add,
            subtract,
            multiply,
            divide
        }
        static Operation GetOperation(char op) => op switch
        {
            '+' => Operation.add,
            '-' => Operation.subtract,
            '*' => Operation.multiply,
            '/' => Operation.divide,
            _ => throw new NotImplementedException(),
        };

        class Entity { }
        class Number : Entity
        {
            public double Num { get; set; }
            public Number(double num) => Num = num;

            public static implicit operator Number(double num) => new Number(num);
            public static implicit operator double(Number number) => number.Num;
        }
        class Node : Entity
        {
            public Entity Left { get; set; }
            public Operation Operation { get; set; }
            public Entity Right { get; set; }
            public Node() { }
            public Node(Operation operation, Entity left = null, Entity right = null) =>
                (Operation, Left, Right) = (operation, left, right);
            public double Calculate()
            {
                double left = Left switch
                {
                    Node node => node.Calculate(),
                    Number number => number,
                    _ => throw new FormatException()
                };

                double right = Right switch
                {
                    Node node => node.Calculate(),
                    Number number => number,
                    _ => throw new FormatException()
                };

                return Operation switch
                {
                    Operation.add => left + right,
                    Operation.subtract => left - right,
                    Operation.multiply => left * right,
                    Operation.divide => left / right,
                    _ => throw new FormatException()
                };
            }
        }

        public static double Calculate(string exp)
        {
            Entity treeExp = GenerateTree(exp);

            return treeExp switch
            {
                Node node => node.Calculate(),
                Number number => number,
                _ => throw new FormatException()
            };
        }

        const string operations = "+-*/";
        static Entity GenerateTree(ReadOnlySpan<char> exp)
        {
            if (exp.All(c => Char.IsDigit(c)))
                return new Number(double.Parse(exp));

            Node last = null;
            int startParseIndex = 0;
            for (int i = 0; i < exp.Length; i++)
            {
                char c = exp[i];
                if (operations.Contains(c))
                {
                    if (startParseIndex == i)
                        throw new FormatException();

                    double num = double.Parse(exp.Slice(startParseIndex, i - startParseIndex));
                    startParseIndex = i + 1;

                    Operation op = GetOperation(c);
                    if (last == null)
                    {
                        last = new Node(op, new Number(num));
                    }
                    else
                    {
                        if ((last.Operation == Operation.add || last.Operation == Operation.subtract)
                            && (op == Operation.multiply || op == Operation.divide))
                        {
                            var priorityOperationsTree = GeneratePriorityOperationsTree(num, op, exp.Slice(startParseIndex));
                            last.Right = priorityOperationsTree.entity;
                            
                            startParseIndex += priorityOperationsTree.length;
                            if (startParseIndex == exp.Length)
                                return last;

                            last = new Node(GetOperation(exp[startParseIndex]), last);
                            startParseIndex++;
                            i = startParseIndex;
                        }
                        else
                        {
                            last.Right = new Number(num);
                            last = new Node(op, last);
                        }
                    }
                }
            }

            if (startParseIndex == exp.Length)
                throw new FormatException();

            last.Right = new Number(double.Parse(exp.Slice(startParseIndex)));
            return last;
        }

        static (Entity entity, int length) GeneratePriorityOperationsTree(double number, Operation operation, ReadOnlySpan<char> exp)
        {
            if (exp.All(c => Char.IsDigit(c)))
                return (new Node(operation, new Number(number), new Number(double.Parse(exp))), exp.Length);

            Node last = new Node(operation, new Number(number));
            int startParseIndex = 0;
            for (int i = 0; i < exp.Length; i++)
            {
                char c = exp[i];
                if (operations.Contains(c))
                {
                    if (startParseIndex == i)
                        throw new FormatException();

                    double num = double.Parse(exp.Slice(startParseIndex, i - startParseIndex));
                    startParseIndex = i + 1;

                    Operation op = GetOperation(c);

                    if (op == Operation.add || op == Operation.subtract)
                    {
                        last.Right = new Number(num);
                        return (last, i);
                    }

                    last.Right = new Number(num);
                    last = new Node(op, last);
                }
            }

            if (startParseIndex == exp.Length)
                throw new FormatException();

            last.Right = new Number(double.Parse(exp.Slice(startParseIndex)));
            return (last, exp.Length);
        }

        public static bool All<TSource>(this ReadOnlySpan<TSource> source, Predicate<TSource> predicate)
        {
            if (source == null || predicate == null)
                throw new ArgumentNullException();

            foreach (TSource element in source)
                if (!predicate(element))
                    return false;

            return true;
        }
    }
}
