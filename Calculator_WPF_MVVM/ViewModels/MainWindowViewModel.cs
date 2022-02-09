using Calculator_WPF_MVVM.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Calculator_WPF_MVVM.ViewModels
{
    class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged([CallerMemberName] string ProprtyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(ProprtyName));
        }

        private CalculatorModel calculation;

        private string lastOperation;
        private bool newDisplayRequired = false;
        private string display;

        public MainWindowViewModel()
        {
            this.calculation = new CalculatorModel();
            this.display = "0";
            this.FirstOperand = string.Empty;
            this.SecondOperand = string.Empty;
            this.Operation = string.Empty;
            this.lastOperation = string.Empty;
            OperationButtonPressCommand = new RelayCommand(OnOperationButtonPressExecute, CanOperationButtonPressCommandExecuted);
            SingularOperationButtonPressCommand = new RelayCommand(OnSingularOperationButtonPressCommandExecute, CanSingularOperationButtonPressCommandExecuted);
            SymbolButtonPressCommand = new RelayCommand(OnSymbolButtonPressCommandExecute, null);
        }

        public string FirstOperand
        {
            get { return calculation.FirstOperand; }
            set { calculation.FirstOperand = value; }
        }

        public string SecondOperand
        {
            get { return calculation.SecondOperand; }
            set { calculation.SecondOperand = value; }
        }

        public string Operation
        {
            get { return calculation.Operation; }
            set { calculation.Operation = value; }
        }

        public string LastOperation
        {
            get { return lastOperation; }
            set { lastOperation = value; }
        }

        public string Result
        {
            get { return calculation.Result; }
        }

        public string Display
        {
            get { return display; }
            set
            {
                display = value;
                OnPropertyChanged();
            }
        }

        public ICommand SymbolButtonPressCommand { get; }
        public void OnSymbolButtonPressCommandExecute(object button)
        {
            switch (button)
            {
                case "C":
                    Display = "0";
                    FirstOperand = string.Empty;
                    SecondOperand = string.Empty;
                    Operation = string.Empty;
                    LastOperation = string.Empty;
                    break;
                case "DEL":
                    if (display.Length > 1)
                        Display = display.Substring(0, display.Length - 1);
                    else Display = "0";
                    break;
                case ".":
                    if (newDisplayRequired)
                    {
                        Display = "0,";
                    }
                    else
                    {
                        if (!display.Contains("."))
                        {
                            Display = display + ",";
                        }
                    }
                    break;
                default:
                    if (display == "0" || newDisplayRequired)
                        Display = (string)button;
                    else
                        Display = display + (string)button;
                    break;
            }
            newDisplayRequired = false;
        }

        public ICommand OperationButtonPressCommand { get; }
        public void OnOperationButtonPressExecute(object operation)
        {
            try
            {
                if (FirstOperand == string.Empty || LastOperation == "=")
                {
                    FirstOperand = display;
                    LastOperation = operation as string;
                }
                else
                {
                    SecondOperand = display;
                    Operation = lastOperation;
                    calculation.CalculateResult();
                    LastOperation = operation as string;
                    Display = Result;
                    FirstOperand = display;
                }
                newDisplayRequired = true;
            }
            catch (Exception ex)
            {
                Display = $"Ошибка: {ex.Message}";
            }
        }
        public bool CanOperationButtonPressCommandExecuted(object operation)
        {
            if (lastOperation == "/" && (string)operation == "=" && Convert.ToDouble(Display) == Convert.ToDouble(0)) //Тут такая заморочка с конвертицией потому что, если у нуля в знаменателе будет запятая, то string будет разрешать деление.
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public ICommand SingularOperationButtonPressCommand { get; }
        public void OnSingularOperationButtonPressCommandExecute(object operation)
        {
            try
            {
                FirstOperand = Display;
                Operation = (string)operation;
                calculation.CalculateResult();
                LastOperation = "=";
                Display = Result;
                FirstOperand = display;
                newDisplayRequired = true;
            }
            catch (Exception ex)
            {
                Display = $"Ошибка: {ex.Message}";
            }
        }
        public bool CanSingularOperationButtonPressCommandExecuted(object operation)
        {
            if ((string)operation == "x^-2" && Convert.ToDouble(Display) < Convert.ToDouble(0))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
