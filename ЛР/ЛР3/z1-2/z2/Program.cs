using System;

    public class Vector
    {
        // Свойство для вычисления длины вектора
        public double Length { get => Math.Sqrt(x * x + y * y + z * z); }

        // поля для координат вектора
        private double x, y, z;

     
        public Vector() { }

        // Конструктор с параметрами для установки значений координат
        public Vector(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        // Оператор сложения векторов
        public static Vector operator +(Vector a, Vector b) => new Vector(a.x + b.x, a.y + b.y, a.z + b.z);

        // Оператор вычитания векторов
        public static Vector operator -(Vector a, Vector b) => new Vector(a.x - b.x, a.y - b.y, a.z - b.z);

        // Оператор скалярного произведения векторов
        public static double operator *(Vector a, Vector b) => a.x * b.x + a.y * b.y + a.z * b.z;

        // Переопределение метода для отображения вектора в виде строки
        public override string ToString() => $"({x}, {y}, {z})";
    }

    class Program
    {
        static void Main()
        {
            // Чтение координат для первого вектора
            Vector vector1 = new Vector(ReadDouble(), ReadDouble(), ReadDouble());
            // Чтение координат для второго вектора
            Vector vector2 = new Vector(ReadDouble(), ReadDouble(), ReadDouble());

            // Вычисление суммы векторов
            Vector sum = vector1 + vector2;
            // Вычисление разности векторов
            Vector difference = vector1 - vector2;
            // Вычисление скалярного произведения векторов
            double dotProduct = vector1 * vector2;

            // Вывод векторов и их атрибутов на экран
            Console.WriteLine($"Vector 1: {vector1}");
            Console.WriteLine($"Vector 2: {vector2}");
            Console.WriteLine($"Length of Vector 1: {vector1.Length}");
            Console.WriteLine($"Length of Vector 2: {vector2.Length}");

            Console.WriteLine($"Sum: {sum}");
            Console.WriteLine($"Difference: {difference}");
            Console.WriteLine($"Dot Product: {dotProduct}");

            // Создание нулевого вектора и вывод его на экран
            Vector vector = new Vector();
            Console.WriteLine($"zero vector: {vector}");

            // Создание двух пустых векторов
            Vector v1 = new Vector();
            Vector v2 = new Vector();

            // Сложение двух пустых векторов
            Vector v3 = v1 + v2;

            // Ожидание ввода пользователя перед завершением программы
            Console.ReadLine();
        }

        // Метод для чтения вещественных чисел из консоли с обработкой ошибок
        private static double ReadDouble()
        {
            while (true)
            {
                try
                {
                    // Чтение и парсинг ввода пользователя
                    return double.Parse(Console.ReadLine());
                }
                catch (Exception)
                {
                    // Сообщение об ошибке, если ввод некорректен
                    Console.WriteLine("Введены некорректные данные, введите вещественное число");
                }
            }
        }
    }

