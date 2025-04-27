using System;

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Введите последовательность чисел без пробелов: ");
            string input = Console.ReadLine(); // Считываем строку с числовой последовательностью

            string result = ExcludeDigits(input); // Вызываем метод для исключения цифр
            Console.WriteLine($"Результат: {result}"); // Выводим результат
        }

        // Метод для исключения цифр 1 и 3 из строки
        static string ExcludeDigits(string input)
        {
            string output = ""; // Переменная для хранения результата

            foreach (char digit in input) // Проходим по каждому символу в строке
            {
                if (digit != '1' && digit != '3') // Проверяем, что символ не равен '1' и '3'
                {
                    output += digit; // Добавляем символ к результату
                }
            }

            return output; // Возвращаем результат
        }
    }

