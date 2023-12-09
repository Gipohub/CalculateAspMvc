using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApplicationASPMVC.Models;

namespace WebApplicationASPMVC.Controllers
{
    public class HomeController : Controller
    {
        //private readonly ILogger<HomeController> _logger;

        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}

        
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
        public IActionResult RootAction(double a, double b) => Content($"Input a - {a}\nResult: {Root(a)}");
        [HttpGet("Calc")]
        public IActionResult CalculateAction(string str) => Content($"Input a - {str}\nResult: {Calculate(str)}");

        [HttpPost]
        public string Index(Dictionary<string, string> items)
        {

            string result = "";
            result = Calculate(result);
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
                string result = expression;
                bool error = false;
                int priority = 0;
                int symb = 0;
                bool dots = false;
                
                bool sideOperation = false;

                double leftOperand = 0;
                int operation = 0;
                double rightOperand = 0;

                int leftPoint = 0;
                int rightPoint = 0;

                double _tmpInt = 0;
                int _operationCount = 0;

                bool completeStep = false;
                //if (expression == null || expression == "") return operand;


                for (int i = 0; i < result.Length; i++)
                {
                    // if it int
                    if (double.TryParse(result[i].ToString(), out double _tmpParsInt))
                    {                        
                        if (_tmpInt != 0)
                        {
                            if (dots)
                            {
                                _tmpInt = _tmpInt + (_tmpParsInt / 10);
                            }
                            else
                            {
                                _tmpInt = _tmpInt * 10 + _tmpParsInt;
                            }
                        }
                        else
                        {
                            _tmpInt = _tmpParsInt;
                            if (sideOperation) rightPoint = i;
                            else leftPoint = i;
                            
                        }
                        if (sideOperation) rightOperand = _tmpInt;
                        else leftOperand = _tmpInt;
                        
                    
                    } // if it symb
                    else
                    {
                        if (result[i] != '.')
                        {
                            int _tempSymb = 0;
                            switch (result[i])
                            {                                
                                case '+':
                                    symb = 1;
                                    if (sideOperation)
                                    {
                                        result = $"{result[0..(leftPoint - 1)]}{Calculate(result[(leftPoint - 1)..i])}{result[i..]}";
                                        sideOperation = false;
                                        i = -1;
                                        completeStep = true;
                                        _tmpInt = 0;
                                    }
                                    else
                                    {
                                        sideOperation = true;
                                        priority = 1;
                                        _tmpInt = 0;
                                    }
                                    break;
                                case '-':
                                    symb = 2;
                                    if (sideOperation)
                                    {
                                        result = $"{result[0..(leftPoint - 1)]}{Calculate(result[(leftPoint - 1)..i])}{result[i..]}";
                                        sideOperation = false;
                                        i = -1;
                                        completeStep = true;
                                        _tmpInt = 0;
                                    }
                                    else
                                    {
                                        sideOperation = true;
                                        priority = 1;
                                        _tmpInt = 0;
                                    }
                                    break;
                                case '*':
                                    symb = 3;
                                    if (sideOperation)
                                    {
                                        if(priority < 2)
                                        {
                                            leftOperand = rightOperand; 
                                            leftPoint = i;
                                            priority = 2;
                                            _tmpInt = 0;
                                        }
                                        else
                                        {
                                            result = $"{result[0..(leftPoint - 1)]}{Calculate(result[(leftPoint - 1)..i])}{result[i..]}";
                                            sideOperation = false;
                                            i = -1;
                                            completeStep = true;
                                            _tmpInt = 0;

                                        }                                        
                                    }
                                    else
                                    {
                                        sideOperation = true;
                                        priority = 2;
                                        _tmpInt = 0;
                                    }
                                    break;
                                case '/':
                                    symb = 4;
                                    if (sideOperation)
                                    {
                                        if (priority < 2)
                                        {
                                            leftOperand = rightOperand;
                                            leftPoint = i;
                                            priority = 2;
                                            _tmpInt = 0;
                                        }
                                        else
                                        {
                                            result = $"{result[0..(leftPoint - 1)]}{Calculate(result[(leftPoint - 1)..i])}{result[i..]}";
                                            sideOperation = false;
                                            i = -1;
                                            completeStep = true;
                                            _tmpInt = 0;
                                        }
                                    }
                                    else
                                    {
                                        sideOperation = true;
                                        priority = 2;
                                        _tmpInt = 0;
                                    }
                                    break;
                                case '^':
                                    symb = 5;
                                    if (sideOperation)
                                    {
                                        if (priority < 2)
                                        {
                                            leftOperand = rightOperand;
                                            leftPoint= i - 1;
                                            priority = 2;
                                            _tmpInt = 0;
                                        }
                                        else
                                        {
                                            result = $"{result[0..(leftPoint - 1)]}{Calculate(result[(leftPoint - 1)..i])}{result[i..]}";
                                            sideOperation = false;
                                            i = -1;
                                            completeStep = true;
                                            _tmpInt = 0;
                                        }
                                    }
                                    else
                                    {
                                        sideOperation = true;
                                        priority = 1;
                                        _tmpInt = 0;
                                    }
                                    break;
                                case '(':
                                    symb = 6;
                                    leftOperand = 0;
                                    rightOperand = 0;
                                    leftPoint = i;
                                    sideOperation = false;
                                    _tmpInt = 0;
                                    break;
                                case ')':
                                    symb = 7;                                            
                                    result = $"{result[0..(leftPoint - 1)]}{Calculate(result[leftPoint..i])}{result[(i + 1)..]}";
                                    sideOperation = false;
                                    i = -1;
                                    completeStep = true;
                                    _tmpInt = 0;
                                    break;
                                default:
                                    error = true;
                                    break;
                            }
                        }
                        else
                        {
                            dots = true;
                        }
                        
                    }
                    if (error) return $"The simbhol \"{result[i]}\" is incorrect";


                    if (i == result.Length - 1)
                    {
                        switch (symb)
                        {
                            case 1:
                               result =  DelegateAlgorithm(Sum, leftOperand, rightOperand).ToString();
                                break; 
                            case 2:
                               result = DelegateAlgorithm(Substract, leftOperand, rightOperand).ToString();
                                break; 
                            case 3:
                                result = DelegateAlgorithm(Multiply, leftOperand, rightOperand).ToString();
                                break; 
                            case 4:
                                result = DelegateAlgorithm(Divide, leftOperand, rightOperand).ToString();
                                break; 
                            case 5:
                                result = DelegateAlgorithm(Power, leftOperand, rightOperand).ToString();
                                break; 
                                default : error = true; break;
                        }
                        
                    }
                    //if (completeStep) i = expression.Length - 1;
                }
                return result;
            } 
            else return _parsResult.ToString();
        }        

        delegate double MyDel(double x, double y);
        static double DelegateAlgorithm(MyDel d, double x, double y)
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
            return double.Pow(a, b);//¬озвращает указанное число, возведенное в указанную степень.
            //return double.Exp(b);//	¬озвращает e, возведенное в указанную степень.
        }

        static double Root(double a)
        {
            return double.ReciprocalSqrtEstimate(a);// ¬озвращает оценку обратного квадратного корн€ указанного числа.
        }
    }
}
