using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApplicationASPMVC.Models;

namespace WebApplicationASPMVC.Controllers
{
    public class HomeController : Controller
    {        
            [HttpGet]
        public IActionResult Index() => View();

        [HttpGet("Sum")]
        public IActionResult SumAction(double a, double b) => Content($"Input a - {a}\nInput b - {b}\nResult: {Sum(a, b)}");
        [HttpGet("Substract")]
        public IActionResult SubstractAction(double a, double b) => Content($"Input a - {a}\nInput b - {b}\nResult: {Substract(a, b)}");
        [HttpGet("Divide")]
        public IActionResult DivideAction(double a, double b) => Content($"Input a - {a}\nInput b - {b}\nResult: {Divide(a, b)}");
        [HttpGet("multiple")]
        public IActionResult MultipleAction(double a, double b) => Content($"Input a - {a}\nInput b - {b}\nResult: {Multiply(a, b)}");
        [HttpGet("Power")]
        public IActionResult PowerAction(double a, double b) => Content($"Input a - {a}\nInput b - {b}\nResult: {Power(a, b)}");
        [HttpGet("Root")]
        public IActionResult RootAction(double a) => Content($"Input a - {a}\nResult: {Root(a)}");
        [HttpGet("Calc")]
        public IActionResult CalculateAction(string str) => Content($"Input a - {str}\nResult: {Calculate(str)}");

        [HttpPost]
        public string Index(Dictionary<string, string> items)
        {
            string result = "";            
            foreach (var item in items)
            {
                result = $"{result} {item.Key} - {item.Value}; ";
            }
            return result;
        }

        public IActionResult Indexes()
        {
            return View();
        }
        public IActionResult History()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        
        private static string Calculate(string expression)
        {
            if (!int.TryParse(expression, out int _parsResult))
            {
                bool error = false;

                bool dots = false;
                bool afterOperator = false;
                bool minus = false;
                bool sideOperation = false;

                double leftOperand = 0;
                int symbCode = 0;
                double rightOperand = 0;

                int leftPoint = 0;
                int rightPoint = 0;

                int _tmpSymbCode = 0;
                double _tmpInt = 0;

                string result = "";

                foreach (char c in expression)
                {
                    if (c != ' ')
                    {
                        result += c.ToString();
                    }
                }
                if (result.Contains('('))
                {
                    int leftParentheses = result.IndexOf('(');
                    int rightParentheses = -1;

                    for (int i = result.Length - 1; i > 0; i--)
                    {
                        if (result[i] == ')')
                        {
                            rightParentheses = i;
                        }
                    }
                    if(rightParentheses != -1)
                    {
                        result = $"{result[0..leftParentheses]}{
                            Calculate(result[(leftParentheses + 1)..rightParentheses])}{
                            result[(rightParentheses + 1)..]}";
                    }
                    else
                    {
                        error = true;
                    }

                }                                          
                
                for (int i = 0; i < result.Length; i++)
                {
                    // if it int
                    if (double.TryParse(result[i].ToString(), out double _tmpPartOfInt))
                    {
                        afterOperator = false;
                        if (_tmpInt != 0)
                        {
                            if (dots)
                            {
                                if (minus)
                                {
                                    _tmpInt = _tmpInt - (_tmpPartOfInt / 10);
                                }
                                else
                                {
                                    _tmpInt = _tmpInt + (_tmpPartOfInt / 10);
                                }                                
                            }
                            else
                            {
                                if (minus)
                                {
                                    _tmpInt = _tmpInt * 10 -_tmpPartOfInt;
                                }
                                else
                                {
                                    _tmpInt = _tmpInt * 10 + _tmpPartOfInt;
                                }                                
                            }
                        }
                        else
                        {
                            _tmpInt = _tmpPartOfInt;
                            if (sideOperation) rightPoint = i;
                            else leftPoint = i;                            
                        }
                        if (sideOperation) rightOperand = _tmpInt;
                        else leftOperand = _tmpInt;
                        if (minus) _tmpInt = -_tmpInt;
                    
                    } 
                    // if it symb
                    else
                    {
                        switch (result[i])
                        {
                            case '.':
                                dots = true;
                                break;
                            case '+':
                                _tmpSymbCode = 1;                                
                                break;
                            case '-':
                                if (afterOperator) minus = true;
                                else _tmpSymbCode = 2;                                
                                break;
                            case '*':
                                _tmpSymbCode = 3;                                
                                break;
                            case '/':
                                _tmpSymbCode = 4;                                
                                break;
                            case '^':
                                _tmpSymbCode = 5;                                
                                break;                            
                            default:
                                error = true;
                                break;
                        }
                        if (symbCode == 0)
                        {
                            symbCode = _tmpSymbCode;
                            _tmpInt = 0;
                            sideOperation = true;
                        } else if (_tmpSymbCode > symbCode)
                        {                            
                            leftOperand = rightOperand;
                            leftPoint = rightPoint;

                            symbCode = _tmpSymbCode;
                            _tmpInt = 0;
                            sideOperation = true;
                        }
                        afterOperator = true;
                        if (_tmpSymbCode != 2) minus = false;
                    }
                    if (error) return $"The simbhol \"{result[i]}\" is incorrect";                   
                    
                }
                rightPoint += rightOperand.ToString().Length - 1;

                switch (symbCode)
                {
                    case 1:
                        result = Calculate($"{result[0..leftPoint]}{
                            Dlgt(Sum, leftOperand, rightOperand)}{
                            result[(rightPoint + 1)..]}");
                        break;
                    case 2:
                        result = Calculate($"{result[0..leftPoint]}{
                            Dlgt(Substract, leftOperand, rightOperand)}{
                            result[(rightPoint + 1)..]}");  
                        break;
                    case 3:
                        result = Calculate($"{result[0..leftPoint]}{
                            Dlgt(Multiply, leftOperand, rightOperand)}{
                            result[(rightPoint + 1)..]}");
                        break;
                    case 4:
                        result = Calculate($"{result[0..leftPoint]}{
                            Dlgt(Divide, leftOperand, rightOperand)}{
                            result[(rightPoint + 1)..]}");                        
                        break;
                    case 5:
                        result = Calculate($"{result[0..leftPoint]}{
                            Dlgt(Power, leftOperand, rightOperand)}{
                            result[(rightPoint + 1)..]}");
                        break;
                    default: error = true; break;
                }
                return result;
            } 
            else return _parsResult.ToString();
        }        

        delegate double MyDel(double x, double y);
        static double Dlgt(MyDel d, double x, double y)
        {
            return d(x, y);
        }
        static double Sum(double a, double b)
        {
            return a + b;
        }
        static double Substract(double a, double b)
        {
            return a - b;
        }
        static double Multiply(double a, double b)
        {
            return a * b;
        }
        static double Divide(double a, double b)
        {
            return a / b;
        }
        static double Power(double a, double b)
        {
            return double.Pow(a, b);
            //¬озвращает указанное число, возведенное в указанную степень.
        }
        static double Root(double a)
        {
            return double.ReciprocalSqrtEstimate(a);
            // ¬озвращает оценку обратного квадратного корн€ указанного числа.
        }
    }
}
