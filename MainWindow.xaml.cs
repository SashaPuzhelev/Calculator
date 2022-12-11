using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace Calculator
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            foreach (UIElement element in LayoutRoot.Children)
            {
                if (element is Button button)
                {
                    button.Click += Button_Click;
                }
            }
        }
        private static bool CheckDivisionByZero(string str)
        {
            float value = float.Parse(str);
            return float.IsInfinity(value) || float.IsNaN(value);
        }
        private static string CalculateStringExpression(string str)
        {
            try
            {
                str = new DataTable().Compute(str.Replace(",", "."), null).ToString();
                if (CheckDivisionByZero(str))
                {
                    MessageBox.Show("Деление на 0", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    return str;
                }
            }
            catch (SyntaxErrorException)
            {
                MessageBox.Show("Синтаксическая ошибка", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (DivideByZeroException)
            {
                MessageBox.Show("Деление на 0", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (EvaluateException)
            {
                MessageBox.Show("Синтаксическая ошибка", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            str = "";
            return str;
        }
        private static string GetCorrectResult(string textBlock, string buttonsText)
        {
            if (buttonsText == "CLEAR")
            {
                textBlock = "";
            }
            else if ((buttonsText == "-" || buttonsText == "+" || buttonsText == "/" || buttonsText == "*")
                && (textBlock.EndsWith("-") || textBlock.EndsWith("+") || textBlock.EndsWith("*") || textBlock.EndsWith("/")))
            {
                textBlock = textBlock.Remove(textBlock.Length - 1, 1) + buttonsText;
            }
            else if (buttonsText == "=")
            {
                textBlock = CalculateStringExpression(textBlock);
            }
            else
            {
                textBlock += buttonsText;
            }
            return textBlock;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string buttonsText = (string)((Button)e.OriginalSource).Content;
            textBlock.Text = GetCorrectResult(textBlock.Text, buttonsText);
        }
    }
}
