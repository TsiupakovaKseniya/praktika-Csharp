using System;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Введите вещественное число x:");
            double x = double.Parse(Console.ReadLine());

            Console.WriteLine("Введите вещественное число y:");
            double y = double.Parse(Console.ReadLine());

            Console.WriteLine("Введите вещественное число z:");
            double z = double.Parse(Console.ReadLine());

            double a = Math.Pow(1 + y, 1.0 / 3) * (x + y * (x * x + 4)) /
                       (Math.Exp(-x -2) + 1.0 / (x * x + 4));

            double b = (1 + Math.Cos(y - 2)) / (Math.Pow(x, 4) / 2 + Math.Pow(Math.Sin(z), 4));

            Console.WriteLine("a: {0:F2}", a);
            Console.WriteLine("b: {0:F2}", b);
        }
    }
}