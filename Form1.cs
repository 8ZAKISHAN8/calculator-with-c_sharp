using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        void PressNumber(object x, EventArgs y)
        {
            textBox1.Text = textBox1.Text + (x as Button).Text;
        }
        private void EqualPressed(object sender, EventArgs e)
        {
            try
            {
                string infix = textBox1.Text;
                string postfix = InfixToPostfix(infix);
                double result = EvaluatePostfix(postfix);
                textBox1.Text = result.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                textBox1.Text = "";
            }
        }

        private void btnBackspace_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox1.Text))
            {
                textBox1.Text = textBox1.Text.Substring(0, textBox1.Text.Length - 1);
            }
        }

        // ----- INFIX TO POSTFIX -----
        private string InfixToPostfix(string infix)
        {
            List<string> output = new List<string>();
            Stack<string> stack = new Stack<string>();

            string pattern = @"(\d+(\.\d+)?|\+|\-|\*|\/|\^|\(|\))";
            MatchCollection matches = Regex.Matches(infix, pattern);

            bool lastWasOperatorOrLeftParen = true;

            for (int i = 0; i < matches.Count; i++)
            {
                string token = matches[i].Value;

                // Handle unary minus for negative numbers
                if (token == "-" && lastWasOperatorOrLeftParen)
                {
                    // Look ahead for the number to attach the minus to
                    if (i + 1 < matches.Count && Regex.IsMatch(matches[i + 1].Value, @"\d+(\.\d+)?"))
                    {
                        string negativeNumber = "-" + matches[i + 1].Value;
                        output.Add(negativeNumber);
                        i++; // skip next token
                        lastWasOperatorOrLeftParen = false;
                        continue;
                    }
                }

                if (Regex.IsMatch(token, @"\d+(\.\d+)?")) // number
                {
                    output.Add(token);
                    lastWasOperatorOrLeftParen = false;
                }
                else if (token == "(")
                {
                    stack.Push(token);
                    lastWasOperatorOrLeftParen = true;
                }
                else if (token == ")")
                {
                    while (stack.Count > 0 && stack.Peek() != "(")
                        output.Add(stack.Pop());
                    if (stack.Count > 0 && stack.Peek() == "(")
                        stack.Pop(); // discard "("
                    lastWasOperatorOrLeftParen = false;
                }
                else if (IsOperator(token))
                {
                    while (stack.Count > 0 && IsOperator(stack.Peek()) &&
                           GetPrecedence(token) <= GetPrecedence(stack.Peek()))
                    {
                        output.Add(stack.Pop());
                    }
                    stack.Push(token);
                    lastWasOperatorOrLeftParen = true;
                }
            }

            while (stack.Count > 0)
                output.Add(stack.Pop());

            return string.Join(" ", output);
        }

        // ----- POSTFIX EVALUATOR -----
        private double EvaluatePostfix(string postfix)
        {
            Stack<double> stack = new Stack<double>();
            string[] tokens = postfix.Split(' ');

            foreach (string token in tokens)
            {
                if (double.TryParse(token, out double num))
                {
                    stack.Push(num);
                }
                else
                {
                    double b = stack.Pop();
                    double a = stack.Pop();

                    switch (token)
                    {
                        case "+": stack.Push(a + b); break;
                        case "-": stack.Push(a - b); break;
                        case "*": stack.Push(a * b); break;
                        case "/": stack.Push(a / b); break;
                        case "^": stack.Push(Math.Pow(a, b)); break;
                        default: throw new Exception("Unknown operator: " + token);
                    }
                }
            }

            return stack.Pop();
        }

        // ----- HELPERS -----
        private bool IsOperator(string token)
        {
            return token == "+" || token == "-" || token == "*" || token == "/" || token == "^";
        }

        private int GetPrecedence(string op)
        {
            switch (op)
            {
                case "^": return 3;
                case "*":
                case "/": return 2;
                case "+":
                case "-": return 1;
                default: return 0;
            }
        }

        private void DrawGraph(Func<double, double> func, string label = "f(x)")
        {
            // Create a new bitmap for drawing the graph
            Bitmap bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Graphics g = Graphics.FromImage(bmp);
            g.Clear(Color.White); // Set background color to white

            // Create pens for axis and graph line
            Pen axisPen = new Pen(Color.Black, 1);
            Pen graphPen = new Pen(Color.Blue, 2);
            Font font = new Font("Arial", 10);

            int width = bmp.Width;
            int height = bmp.Height;
            int midX = width / 2;  // X-axis midpoint
            int midY = height / 2; // Y-axis midpoint

            // Draw axes
            g.DrawLine(axisPen, 0, midY, width, midY); // X-axis
            g.DrawLine(axisPen, midX, 0, midX, height); // Y-axis

            // Plot the function
            Point? prevPoint = null;
            for (int px = 0; px < width; px++)
            {
                // Scale x to match the picture box width, centered around 0
                double x = (px - midX) / 50.0; // 50 is the scale factor for horizontal stretch

                // Evaluate y using the passed function (like sin, cos, etc.)
                double y = func(x);

                // Handle cases where y is too large or undefined (e.g., tan(x) at multiples of 90 degrees)
                if (double.IsNaN(y) || double.IsInfinity(y))
                {
                    prevPoint = null;
                    continue; // Skip plotting this point
                }

                // Scale y to match the picture box height, and invert for correct orientation
                int py = midY - (int)(y * 50); // 50 is the scale factor for vertical stretch

                if (py < 0 || py >= height) // Ignore points outside the bounds
                {
                    prevPoint = null;
                    continue;
                }

                // Create a point for plotting
                Point point = new Point(px, py);

                // Draw the line between the current point and the previous point
                if (prevPoint != null)
                    g.DrawLine(graphPen, prevPoint.Value, point);

                prevPoint = point;
            }

            // Draw the label for the graph at the top-left corner
            g.DrawString(label, font, Brushes.Black, 5, 5);

            // Set the PictureBox's image to the generated graph
            pictureBox1.Image = bmp;
        }
        private double EvaluateSimpleExpression(string expr, double x)
        {
            try
            {
                expr = expr.Replace("x", x.ToString(System.Globalization.CultureInfo.InvariantCulture));
                var dt = new System.Data.DataTable();
                var result = dt.Compute(expr, "");
                return Convert.ToDouble(result);
            }
            catch
            {
                return double.NaN;
            }
        }

        private void btnDraw_Click(object sender, EventArgs e)
        {
            string expr = textBox1.Text.Trim().ToLower();

            if (expr == "sin(x)")
            {
                DrawGraph(Math.Sin, "sin(x)");
            }
            else if (expr == "cos(x)")
            {
                DrawGraph(Math.Cos, "cos(x)");
            }
            else if (expr == "tan(x)")
            {
                DrawGraph(Math.Tan, "tan(x)");
            }
            else if (expr.Contains("x")) // fallback to evaluate basic algebra like x+5 or 2*x+3
            {
                DrawGraph(x => EvaluateSimpleExpression(expr, x), "f(x) = " + expr);
            }
            else
            {
                MessageBox.Show("Unsupported function. Try sin(x), cos(x), tan(x), or expressions like x+5.");
            }
        }
    }
}
  