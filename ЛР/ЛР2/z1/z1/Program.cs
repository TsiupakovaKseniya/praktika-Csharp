using System;

    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Введите символ: ");
            char inputChar = Console.ReadKey().KeyChar; // Считываем символ с клавиатуры
            Console.WriteLine(); // Переход на новую строку

            // Проверяем, к какому алфавиту принадлежит символ
            if ((inputChar >= 'A' && inputChar <= 'Z') || (inputChar >= 'a' && inputChar <= 'z'))
            {
                Console.WriteLine($"Символ '{inputChar}' принадлежит латинскому алфавиту.");
            }
            else if ((inputChar >= 'А' && inputChar <= 'Я') || (inputChar >= 'а' && inputChar <= 'я'))
            {
                Console.WriteLine($"Символ '{inputChar}' принадлежит русскому алфавиту.");
            }
            else
            {
                Console.WriteLine($"Символ '{inputChar}' не принадлежит ни латинскому, ни русскому алфавиту.");
            }
        }
    }

