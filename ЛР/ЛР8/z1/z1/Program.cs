using System;

    internal class Program
    {
        static void Main(string[] args)
        {
            // Создаем экземпляр TestClass, который реализует интерфейсы Iy, Ix и Iz
            Iy iy = new TestClass();

            // Вызываем метод F0 из интерфейса Iy с параметром 2
            iy.F0(2);

            // Приводим iy к интерфейсу Iz и вызываем метод F1
            ((Iz)iy).F1();

            // Ожидание ввода пользователя перед завершением программы
            Console.ReadLine();
        }
    }

    // Определение интерфейса Ix с двумя методами
    interface Ix
    {
        void IxF0(int n); // Метод IxF0 принимает целочисленный параметр
        void IxF1(); // Метод IxF1 без параметров
    }

    // Определение интерфейса Iy с двумя методами
    interface Iy
    {
        void F0(int n); // Метод F0 принимает целочисленный параметр
        void F1(); // Метод F1 без параметров
    }

    // Определение интерфейса Iz с двумя методами
    interface Iz
    {
        void F0(int n); // Метод F0 принимает целочисленный параметр
        void F1(); // Метод F1 без параметров
    }

    // Класс TestClass, реализующий три интерфейса: Ix, Iy и Iz
    class TestClass : Ix, Iy, Iz
    {
        // Реализация метода F0 интерфейса Iz, выводит 7 * w - 2
        void Iz.F0(int w) => Console.WriteLine(7 * w - 2);

        // Реализация метода F1 интерфейса Iz, выводит -2
        void Iz.F1() => Console.WriteLine(-2);

        // Реализация метода F0 интерфейса Iy, выводит w в кубе
        public void F0(int w) => Console.WriteLine(Math.Pow(w, 3));

        // Реализация метода F1 интерфейса Iy, выводит 0
        public void F1() => Console.WriteLine(0);

        // Реализация метода IxF0, выводит w + 5
        public void IxF0(int w) => Console.WriteLine(w + 5);

        // Реализация метода IxF1, выводит 5
        public void IxF1() => Console.WriteLine(5);
    }

