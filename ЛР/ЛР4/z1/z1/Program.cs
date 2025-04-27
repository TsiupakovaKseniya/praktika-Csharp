using System;
using System.Collections.Generic;
using System.Linq;

    internal class Program
    {
        static void Main(string[] args)
        {
            // Создаем экземпляр класса ArrayAdder
            ArrayAdder a = new ArrayAdder();

            // Выводим текущее состояние массива
            a.ShowArrayState();

            // Запрашиваем элемент для вывода и суммирования элементов после него
            Console.WriteLine("Введите элемент для вывода и суммирования элементов после него");
            int el = int.Parse(Console.ReadLine());

            // Показываем элементы после введенного элемента
            a.ShowElementsAfter(el);

            // Выводим сумму элементов после введенного элемента
            Console.WriteLine($"Сумма последовательности элементов: {a.SumElementsAfter(el)}");

            Console.ReadLine();
        }
    }

    class ArrayAdder
    {
        private int[] arr; // Массив для хранения уникальных целых чисел

        // Индексатор для доступа к элементам массива
        public int this[int i]
        {
            get
            {
                // Проверяем, не выходит ли индекс за границы массива
                if (arr.Length <= i || i < 0)
                    throw new Exception("Данное число выходит за границы индексов массива");

                return arr[i];
            }
            set
            {
                // Проверяем, если значение уже существует в массиве
                if (arr.Contains(value))
                    throw new Exception("Данное значение уже имеется");

                arr[i] = value; // Устанавливаем значение по индексу
            }
        }

        // Конструктор без параметров, инициализирует массив случайными уникальными числами от 1 до 10
        public ArrayAdder()
        {
            Random rnd = new Random();
            arr = new int[10]; // Инициализация массива размером 10

            HashSet<int> set = new HashSet<int>();
            // Генерируем уникальные случайные числа
            while (set.Count < 10)
                set.Add(rnd.Next(1, 11));

            arr = set.ToArray(); // Преобразуем HashSet в массив
        }

        // Конструктор с параметром для создания массива заданного размера
        public ArrayAdder(int n)
        {
            arr = new int[n];
        }

        // Метод для вывода элемента по индексу
        public void ShowElement(int i) => Console.WriteLine(this[i]);

        // Метод для отображения текущего состояния массива
        public void ShowArrayState() => Console.WriteLine(string.Join(" ", arr));

        // Метод для вывода элементов массива, находящихся после указанного элемента
        public void ShowElementsAfter(int el)
        {
            string res = "";
            int i = GetElementIndex(el) + 1; // Получаем индекс элемента и переходим к следующему

            for (int j = i; j < arr.Length; j++)
                res += $"{arr[j]} "; // Собираем элементы в строку

            Console.WriteLine(res); // Выводим результат
        }

        // Метод для суммирования элементов массива, находящихся после указанного элемента
        public int SumElementsAfter(int el)
        {
            int i = GetElementIndex(el) + 1; // Получаем индекс элемента и переходим к следующему
            int sum = 0;

            for (int j = i; j < arr.Length; j++)
                sum += arr[j]; // Суммируем элементы

            return sum; // Возвращаем сумму
        }

        // Метод для получения индекса указанного элемента в массиве
        private int GetElementIndex(int el)
        {
            for (int i = 0; i < arr.Length; i++)
                if (arr[i] == el)
                    return i; // Возвращаем индекс, если элемент найден

            throw new Exception("Массив не имеет данного элемента"); // Исключение, если элемент не найден
        }
    }
