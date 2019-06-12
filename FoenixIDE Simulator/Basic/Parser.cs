using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FoenixIDE.Basic
{
    public class Parser
    {
        public const char START_ARG = '(';
        public const char END_ARG = ')';
        public const char END_LINE = '\n';

        class Cell
        {
            internal Cell(double value, char action)
            {
                Value = value;
                Action = action;
            }

            internal double Value { get; set; }
            internal char Action { get; set; }
        }

        public static double process(string data)
        {
            // Get rid of spaces and check parenthesis
            string expression = preprocess(data);
            int from = 0;

            return loadAndCalculate(data, ref from, END_LINE);
        }

        static string preprocess(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                throw new ArgumentException("Loaded empty data");
            }

            int parentheses = 0;
            StringBuilder result = new StringBuilder(data.Length);

            for (int i = 0; i < data.Length; i++)
            {
                char ch = data[i];
                switch (ch)
                {
                    case ' ':
                    case '\t':
                    case '\n': continue;
                    case END_ARG:
                        parentheses--;
                        break;
                    case START_ARG:
                        parentheses++;
                        break;
                }
                result.Append(ch);
            }

            if (parentheses != 0)
            {
                throw new ArgumentException("Uneven parenthesis");
            }

            return result.ToString();
        }

        public static double loadAndCalculate(string data, ref int from, char to = END_LINE)
        {
            if (from >= data.Length || data[from] == to)
            {
                throw new ArgumentException("Loaded invalid data: " + data);
            }

            List<Cell> listToMerge = new List<Cell>(16);
            StringBuilder item = new StringBuilder();

            do
            { // Main processing cycle of the first part.
                char ch = data[from++];
                if (stillCollecting(item.ToString(), ch, to))
                { // The char still belongs to the previous operand.
                    item.Append(ch);
                    if (from < data.Length && data[from] != to)
                    {
                        continue;
                    }
                }

                // We are done getting the next token. The getValue() call below may
                // recursively call loadAndCalculate(). This will happen if extracted
                // item is a function or if the next item is starting with a START_ARG '('.
                ParserFunction func = new ParserFunction(data, ref from, item.ToString(), ch);
                double value = func.getValue(data, ref from);

                char action = validAction(ch) ? ch
                                              : updateAction(data, ref from, ch, to);

                listToMerge.Add(new Cell(value, action));
                item.Clear();

            } while (from < data.Length && data[from] != to);

            if (from < data.Length &&
               (data[from] == END_ARG || data[from] == to))
            { // This happens when called recursively: move one char forward.
                from++;
            }

            Cell baseCell = listToMerge[0];
            int index = 1;

            return merge(baseCell, ref index, listToMerge);
        }

        static bool stillCollecting(string item, char ch, char to)
        {
            // Stop collecting if either got END_ARG ')' or to char, e.g. ','.
            char stopCollecting = (to == END_ARG || to == END_LINE) ?
                                   END_ARG : to;
            return (item.Length == 0 && (ch == '-' || ch == END_ARG)) ||
                  !(validAction(ch) || ch == START_ARG || ch == stopCollecting);
        }

        static bool validAction(char ch)
        {
            return ch == '*' || ch == '/' || ch == '+' || ch == '-' || ch == '^';
        }

        static char updateAction(string item, ref int from, char ch, char to)
        {
            if (from >= item.Length || item[from] == END_ARG || item[from] == to)
            {
                return END_ARG;
            }

            int index = from;
            char res = ch;
            while (!validAction(res) && index < item.Length)
            { // Look for the next character in string until a valid action is found.
                res = item[index++];
            }

            from = validAction(res) ? index
                                    : index > from ? index - 1
                                                   : from;
            return res;
        }

        // From outside this function is called with mergeOneOnly = false.
        // It also calls itself recursively with mergeOneOnly = true, meaning
        // that it will return after only one merge.
        static double merge(Cell current, ref int index, List<Cell> listToMerge,
                     bool mergeOneOnly = false)
        {
            while (index < listToMerge.Count)
            {
                Cell next = listToMerge[index++];

                while (!canMergeCells(current, next))
                { // If we cannot merge cells yet, go to the next cell and merge
                  // next cells first. E.g. if we have 1+2*3, we first merge next
                  // cells, i.e. 2*3, getting 6, and then we can merge 1+6.
                    merge(next, ref index, listToMerge, true /* mergeOneOnly */);
                }
                mergeCells(current, next);
                if (mergeOneOnly)
                {
                    return current.Value;
                }
            }

            return current.Value;
        }

        static void mergeCells(Cell leftCell, Cell rightCell)
        {
            switch (leftCell.Action)
            {
                case '^':
                    leftCell.Value = Math.Pow(leftCell.Value, rightCell.Value);
                    break;
                case '*':
                    leftCell.Value *= rightCell.Value;
                    break;
                case '/':
                    if (rightCell.Value == 0)
                    {
                        throw new ArgumentException("Division by zero");
                    }
                    leftCell.Value /= rightCell.Value;
                    break;
                case '+':
                    leftCell.Value += rightCell.Value;
                    break;
                case '-':
                    leftCell.Value -= rightCell.Value;
                    break;
            }
            leftCell.Action = rightCell.Action;
        }

        static bool canMergeCells(Cell leftCell, Cell rightCell)
        {
            return getPriority(leftCell.Action) >= getPriority(rightCell.Action);
        }

        static int getPriority(char action)
        {
            switch (action)
            {
                case '^': return 4;
                case '*':
                case '/': return 3;
                case '+':
                case '-': return 2;
            }
            return 0;
        }
    }

    public class ParserFunction
    {
        public ParserFunction()
        {
            m_impl = this;
        }

        // A "virtual" Constructor
        internal ParserFunction(string data, ref int from, string item, char ch)
        {
            if (item.Length == 0 && ch == Parser.START_ARG)
            {
                // There is no function, just an expression in parentheses
                m_impl = s_idFunction;
                return;
            }

            if (m_functions.TryGetValue(item, out m_impl))
            {
                // Function exists and is registered (e.g. pi, exp, etc.)
                return;
            }

            // Function not found, will try to parse this as a number.
            s_strtodFunction.Item = item;
            m_impl = s_strtodFunction;
        }

        public static void addFunction(string name, ParserFunction function)
        {
            m_functions[name] = function;
        }

        public double getValue(string data, ref int from)
        {
            return m_impl.evaluate(data, ref from);
        }

        protected virtual double evaluate(string data, ref int from)
        {
            // The real implementation will be in the derived classes.
            return 0;
        }

        private ParserFunction m_impl;
        private static Dictionary<string, ParserFunction> m_functions = new Dictionary<string, ParserFunction>();

        private static StrtodFunction s_strtodFunction = new StrtodFunction();
        private static IdentityFunction s_idFunction = new IdentityFunction();
    }

    class StrtodFunction : ParserFunction
    {
        protected override double evaluate(string data, ref int from)
        {
            double num;
            if (!Double.TryParse(Item, out num))
            {
                throw new ArgumentException("Could not parse token [" + Item + "]");
            }
            return num;
        }
        public string Item { private get; set; }
    }

    class IdentityFunction : ParserFunction
    {
        protected override double evaluate(string data, ref int from)
        {
            return Parser.loadAndCalculate(data, ref from, Parser.END_ARG);
        }
    }

    class PiFunction : ParserFunction
    {
        protected override double evaluate(string data, ref int from)
        {
            return 3.141592653589793;
        }
    }
    class ExpFunction : ParserFunction
    {
        protected override double evaluate(string data, ref int from)
        {
            double arg = Parser.loadAndCalculate(data, ref from, Parser.END_ARG);
            return Math.Exp(arg);
        }
    }
    class PowFunction : ParserFunction
    {
        protected override double evaluate(string data, ref int from)
        {
            double arg1 = Parser.loadAndCalculate(data, ref from, ',');
            double arg2 = Parser.loadAndCalculate(data, ref from, Parser.END_ARG);

            return Math.Pow(arg1, arg2);
        }
    }
    class SinFunction : ParserFunction
    {
        protected override double evaluate(string data, ref int from)
        {
            double arg = Parser.loadAndCalculate(data, ref from, Parser.END_ARG);
            return Math.Sin(arg);
        }
    }
    class SqrtFunction : ParserFunction
    {
        protected override double evaluate(string data, ref int from)
        {
            double arg = Parser.loadAndCalculate(data, ref from, Parser.END_ARG);
            return Math.Sqrt(arg);
        }
    }
    class AbsFunction : ParserFunction
    {
        protected override double evaluate(string data, ref int from)
        {
            double arg = Parser.loadAndCalculate(data, ref from, Parser.END_ARG);
            return Math.Abs(arg);
        }
    }

    class Parsetest
    {
        static void calculate(string expr, double expected)
        {
            double result = Parser.process(expr);

            string outcome = result == expected ? "OK" : "NOK " + expected.ToString();
            Console.WriteLine("{0} --> {1} ({2})", expr, result, outcome);
        }

        static void Example()
        {
            ParserFunction.addFunction("pi", new PiFunction());
            ParserFunction.addFunction("exp", new ExpFunction());
            ParserFunction.addFunction("pow", new PowFunction());
            ParserFunction.addFunction("sin", new SinFunction());
            ParserFunction.addFunction("abs", new AbsFunction());
            ParserFunction.addFunction("sqrt", new SqrtFunction());

            calculate("(((-5.5)))", (((-5.5))));
            calculate("1-2", 1 - 2);
            calculate("(1-(2))", (1 - (2)));
            calculate("3+2*6-1", 3 + 2 * 6 - 1);
            calculate("3-2*6-1", 3 - 2 * 6 - 1);
            calculate("1-2-3-(4-(5-(6-7)))", 1 - 2 - 3 - (4 - (5 - (6 - 7))));
            calculate("2-3*sin(pi)", 2 - 3 * Math.Sin(Math.PI));
            calculate("1-(exp(10*7-sqrt((1+1)*20*10)))", 1 - (Math.Exp(10 * 7 - Math.Sqrt((1 + 1) * 20 * 10))));
            calculate("3-(5-6)-(2-(3-(1-2)))", 3 - (5 - 6) - (2 - (3 - (1 - 2))));
            calculate("3-(5-6)-(2-(3-(1+2)))+2-(-1+7)*(9-2)/((16-3)-3)+15/2*5",
              3 - (5 - 6) - (2 - (3 - (1 + 2))) + 2 - (-1 + 7) * (9 - 2) / ((16.0 - 3) - 3.0) + 15 / 2.0 * 5);
            calculate("(-1+7)*(9-2)", (-1 + 7) * (9 - 2));
            calculate("((16-3)-3)+15/2*5", ((16 - 3) - 3) + 15 / 2.0 * 5);
            calculate("1+15/2*5", 1 + 15 / 2.0 * 5);
            calculate("3-2/6-1", 3 - 2 / 6.0 - 1);
            calculate("3*50-3*2^4*3", 3 * 50 - 3 * Math.Pow(2, 4) * 3);
            calculate("5-1/2^2-3", 5 - 1 / Math.Pow(2, 2) - 3);
            calculate("(((1/4/2-(8/2/3+5))))", (((1 / 4.0 / 2.0 - (8 / 2.0 / 3.0 + 5)))));
            calculate("pow(2,3)", Math.Pow(2, 3));
            calculate("abs(3*-50-2*3/4)/3*2", Math.Abs(3.0 * -50 - 2 * 3.0 / 4.0) / 3.0 * 2);

            Console.ReadKey();
        }
    }
}
