using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Calculator
{
    public partial class Form1 : Form
    {

        #region

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        #endregion


        #region delete_text

        private void delete_Click(object sender, EventArgs e)
        {
            if (this.user_input.SelectionStart != 0)
            { 

                int selection_start = this.user_input.SelectionStart -1;

                this.user_input.Text = this.user_input.Text.Remove(selection_start, 1);
                this.user_input.SelectionStart = selection_start;

            }

            focus();
        }

        private void all_clear_Click(object sender, EventArgs e)
        {
            this.user_input.Clear();
            this.answer.Text = "0";
            focus();
        }

        #endregion


        #region Numbers_input

        private void number_7_Click(object sender, EventArgs e)
        {
            Input("7");
            focus();
        }

        private void number_8_Click(object sender, EventArgs e)
        {
            Input("8");
            focus();
        }

        private void numebr_9_Click(object sender, EventArgs e)
        {
            Input("9");
            focus();
        }

        private void number_4_Click(object sender, EventArgs e)
        {
            Input("4");
            focus();
        }

        private void number_5_Click(object sender, EventArgs e)
        {
            Input("5");
            focus();
        }

        private void number_6_Click(object sender, EventArgs e)
        {
            Input("6");
            focus();
        }

        private void number_1_Click(object sender, EventArgs e)
        {
            Input("1");
            focus();
        }

        private void number_2_Click(object sender, EventArgs e)
        {
            Input("2");
            focus();
        }

        private void number_3_Click(object sender, EventArgs e)
        {
            Input("3");
            focus();
        }

        private void number_0_Click(object sender, EventArgs e)
        {
            Input("0");
            focus();
        }

        private void number_00_Click(object sender, EventArgs e)
        {
            Input("00");
            focus();
        }

        private void dot_Click(object sender, EventArgs e)
        {
            Input(".");
            focus();
        }


        #endregion


        #region Operator

        private void devide_Click(object sender, EventArgs e)
        {
            Input("÷");
            focus();
        }

        private void time_Click(object sender, EventArgs e)
        {
            Input("×");
            focus();
        }

        private void plus_Click(object sender, EventArgs e)
        {
            Input("+");
            focus();
        }

        private void minus_Click(object sender, EventArgs e)
        {
            Input("-");
            focus();
        }


        #endregion


        #region helpers

        private void focus()
        {
            this.user_input.Focus();
        }

        private void Input(string number)
        {
            int selection_start = this.user_input.SelectionStart;

            this.user_input.Text = this.user_input.Text.Insert(this.user_input.SelectionStart, number);
            this.user_input.SelectionStart = selection_start + number.Length;
        }


        #endregion


        #region equal calculation

        private void equal_Click(object sender, EventArgs e)
        {
            bool operator_included = false;

            if (this.user_input.Text == "")
            {
                this.answer.Text = "";
            }
            else 
            {
                foreach (char i in "+-×÷")
                {
                    if (this.user_input.Text.Contains(i))
                    {
                        this.answer.Text = parse_operation();
                        operator_included = true;
                        break;

                    }
                }
                if(!operator_included)
                {
                    int count = 0;

                    foreach (char b in this.user_input.Text)
                    {
                        if (b == '.')
                            count += 1;
                    }

                    if (count <= 1)
                    {
                        string input = this.user_input.Text;
                        int dot_index = input.IndexOf(".");
                        
                        if(dot_index == -1)
                        {
                            this.answer.Text = input;
                        }
                        else if (dot_index == 0)
                        {

                            if (dot_index == input.Length-1)
                                this.answer.Text = "0";
                            else
                                this.answer.Text = $"0{input}";
                        }
                        else
                        {
                            string cleaned_input = input.Remove(dot_index);
                            this.answer.Text = cleaned_input;
                        }
                    }
                    else
                    {
                        this.answer.Text = "Syntax error";
                    }
                }
            }
            
            focus();

        }

        private string parse_operation()
        {
            try
            {
                
                string input = this.user_input.Text;
                input = input.Replace(" ", "");

                Operation operation = new Operation();
                bool leftSide = true;

                
                while (true)
                {
                    if ( "×÷".Any(o => input.Contains(o)) && "+-".Any(o => input.Contains(o)))
                        input = Arithmetic(input);
                    else
                        break;
                }
                

                for (int i = 0; i < input.Length; i++)
                {
                    if ("0123456789.".Any(c => input[i] == c))
                    {
                        if (leftSide)
                            operation.Left_side = join_number(operation.Left_side, input[i]);
                        else
                            operation.Right_side = join_number(operation.Right_side, input[i]);

                    }
                    else if ("+-×÷".Any(c => input[i] == c))
                    {

                        if (!leftSide)
                        {
                            var operatorType = GetOperationType(input[i]);

                            if (operation.Right_side.Length == 0)
                            {
                                if (operatorType != Operation_type.minus)
                                    throw new InvalidOperationException($"Operator (+ * / or more than one -) specified without an right side number");

                                operation.Right_side += input[i];
                            }
                            else
                            {

                                operation.Left_side = CalculateOperation(operation);

                                operation.OperationType = operatorType;

                                operation.Right_side = string.Empty;
                            }
                        }
                        else
                        {
                            var operatorType = GetOperationType(input[i]);

                            if (operation.Left_side.Length == 0)
                            {
                                if (operatorType != Operation_type.minus)
                                    throw new InvalidOperationException($"Operator (+ * / or more than one -) specified without an left side number");

                                operation.Left_side += input[i];
                            }
                            else
                            {
                                operation.OperationType = operatorType;

                                leftSide = false;
                            }
                        }
                    }
                }




                return CalculateOperation(operation);
            }
            catch (Exception e)
            {
                return $"Invalid equation. {e.Message}";
            }

        }

        private string join_number(string current_number , char character, bool reverse = false)
        {
            if(character == '.' && current_number.Contains("."))
                throw new InvalidOperationException($"Number {current_number} already contains a '.' and another cannot be added ");

            if (!reverse)
            {
                return current_number + character;
            }
            else
            {
                return character + current_number;
            }
        }

        private Operation_type GetOperationType(char character)
        {
            switch (character)
            {
                case '+':
                    return Operation_type.plus;
                case '-':
                    return Operation_type.minus;
                case '÷':
                    return Operation_type.divide;
                case '×':
                    return Operation_type.time;
                default:
                    throw new InvalidOperationException($"Unknown operator type { character }");
            }
        }

        private string CalculateOperation(Operation operation)
        {

            decimal left = 0;
            decimal right = 0;

            if (string.IsNullOrEmpty(operation.Left_side) || !decimal.TryParse(operation.Left_side, out left))
                throw new InvalidOperationException($"Left side of the operation was not a number. {operation.Left_side}");

            if (string.IsNullOrEmpty(operation.Right_side) || !decimal.TryParse(operation.Right_side, out right))
                throw new InvalidOperationException($"Right side of the operation was not a number. {operation.Right_side}");

            try
            {
                switch (operation.OperationType)
                {
                    case Operation_type.time:
                        return (left * right).ToString();
                    case Operation_type.divide:
                        return (left / right).ToString();
                    case Operation_type.plus:
                        return (left + right).ToString();
                    case Operation_type.minus:
                        return (left - right).ToString();
                    
                    
                    default:
                        throw new InvalidOperationException($"Unknown operator type when calculating operation. { operation.OperationType }");
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to calculate operation {operation.Left_side} {operation.OperationType} {operation.Right_side}. {ex.Message}");
            }
        }


        private string Arithmetic(string calculation)
        {
            Operation operation = new Operation();

            for (int i = 0; i < calculation.Length; i++)
            {

                if ("×÷".Any(a => a == calculation[i]) && "+-".Any(o => calculation.Contains(o)))
                {
                    var operatorType = GetOperationType(calculation[i]);

                    int end = 0;
                    string word = "";

                    for (int b = i - 1; b >= 0; b--)
                    {
                        if ("+-×÷".Any(x => x == calculation[b]))
                            break;

                        if ("0123456789.".Any(c => calculation[b] == c))
                            operation.Left_side = join_number(operation.Left_side, calculation[b], reverse: true);
                        word = operation.Left_side;
                    }


                    for (int e = i + 1; e < calculation.Length; e++)
                    {
                        if ("+-×÷".Any(x => x == calculation[e]))
                            break;

                        if ("0123456789.".Any(y => calculation[e] == y))
                            operation.Right_side = join_number(operation.Right_side, calculation[e]);
                        end = e;
                    }

                    operation.OperationType = operatorType;

                    operation.Left_side = CalculateOperation(operation);

                    calculation = calculation.Replace(word + calculation[i] + operation.Right_side, operation.Left_side);

                    operation.Left_side = String.Empty;
                    operation.Right_side = String.Empty;

                }
            }

            return calculation;
        }


        #endregion 
    }
}