using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator_WPF_MVVM.Models
{
    class CalculatorModel
    {
        private string result;

        public string FirstOperand { get; set; }
        public string SecondOperand { get; set; }
        public string Operation { get; set; }
        public string Result { get { return result; } }

        private void ValidateOperand(string operand) //Проверка корректности введенных значений
        {
            try
            {
                Convert.ToDouble(operand);
            }
            catch (Exception)
            {
                result = "Введено некорректное значение: " + operand;
            }
        }

        private void ValidateOperation(string operation) //Проверка наличия возможности выполнения операции
        {
            switch (operation)
            {
                case "/":
                case "*":
                case "-":
                case "+":
                case "x^2":
                case "x^-2":
                    break;
                default:
                    result = "Неизвестная операция: " + operation;
                    throw new ArgumentException("Неизвестная операция: " + operation, "operation");
            }
        }

        private void ValidateData() //Проверка всех входящих данных
        {
            switch (Operation)
            {
                case "/":
                case "*":
                case "-":
                case "+":
                    ValidateOperand(FirstOperand);
                    ValidateOperand(SecondOperand);
                    break;
                case "x^2":
                case "x^-2":
                    ValidateOperand(FirstOperand);
                    break;
                default:
                    result = "Неизвестная операция: " + Operation;
                    throw new ArgumentException("Неизвестная операция: " + Operation, "operation");
            }
        }

        public void CalculateResult()
        {
            ValidateData();

            try
            {
                switch (Operation)
                {
                    case "+":
                        result = (Convert.ToDouble(FirstOperand) + Convert.ToDouble(SecondOperand)).ToString();
                        break;

                    case "-":
                        result = (Convert.ToDouble(FirstOperand) - Convert.ToDouble(SecondOperand)).ToString();
                        break;

                    case "*":
                        result = (Convert.ToDouble(FirstOperand) * Convert.ToDouble(SecondOperand)).ToString();
                        break;

                    case "/":
                        result = (Convert.ToDouble(FirstOperand) / Convert.ToDouble(SecondOperand)).ToString();
                        break;
                    case "x^2":
                        result = Math.Pow(Convert.ToDouble(FirstOperand), 2).ToString();
                        break;
                    case "x^-2":
                        if (Convert.ToDouble(FirstOperand) < 0)
                        {
                            result = "Попытка извлечения корня из отрицательного числа.";
                        }
                        else
                        {
                            result = Math.Sqrt(Convert.ToDouble(FirstOperand)).ToString();
                        }
                        break;
                }
            }
            catch (Exception)
            {
                result = "!Ошибка. Неизвестная операция.";
            }
        }

        public CalculatorModel(string firstOperand, string secondOperand, string operation)
        {
            ValidateOperand(firstOperand);
            ValidateOperand(secondOperand);
            ValidateOperation(operation);

            FirstOperand = firstOperand;
            SecondOperand = secondOperand;
            Operation = operation;
            result = string.Empty;
        }

        public CalculatorModel(string firstOperand, string operation)
        {
            ValidateOperand(firstOperand);
            ValidateOperation(operation);

            FirstOperand = firstOperand;
            SecondOperand = string.Empty;
            Operation = operation;
            result = string.Empty;
        }

        public CalculatorModel()
        {
            FirstOperand = string.Empty;
            SecondOperand = string.Empty;
            Operation = string.Empty;
            result = string.Empty;
        }
    }
}
