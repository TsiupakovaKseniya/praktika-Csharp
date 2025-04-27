using System;
using System.Linq;

internal class Program
{
    static void Main(string[] args)
    {
        // Создаем массив объектов LivingPlace, включая города и деревни
        LivingPlace[] livingPlaces = new LivingPlace[]
        {
            new City(2_000_000, 25000), // Город с 2 миллионами населения и площадью 25000
            new Village(2, 1000, 5), // Деревня с 2 домами, площадью 1000 и средним числом людей в доме 5
            new City(1_000_000, 10000), // Город с 1 миллион населения и площадью 10000
            new Village(20, 10000, 7), // Деревня с 20 домами, площадью 10000 и 7 людей в каждом доме
            new Village(5, 10, 100) // Деревня с 5 домами, площадью 10 и 100 людей в каждом доме
        };

        // Вывод плотности населения для каждого объекта LivingPlace
        foreach (var lp in livingPlaces)
            Console.WriteLine($"Плотность населения: {lp.GetPopulationDensity()}");

        // Находим и выводим максимальную плотность населения среди всех объектов LivingPlace
        Console.WriteLine(livingPlaces.Select(x => x.GetPopulationDensity()).Max());

        // Ожидание ввода пользователя перед завершением программы
        Console.ReadLine();
    }
}

abstract class LivingPlace
{
    protected int h; // Хранит количество домов или населения
    protected double square; // Хранит площадь

    // Конструктор для инициализации базовых данных
    protected LivingPlace(int h, double square)
    {
        this.h = h;
        this.square = square;
    }

    // Абстрактный метод для вычисления плотности населения, должен быть реализован в дочерних классах
    public virtual double GetPopulationDensity() { throw new NotImplementedException(); }
}

class Village : LivingPlace
{
    public int Homes => h; // Свойство для получения количества домов
    public readonly double AveragePeopleInHouse; // Среднее число людей в доме

    // Конструктор для инициализации деревни
    public Village(int homesCount, double square, double averagePeopleInHouse) : base(homesCount, square)
    {
        AveragePeopleInHouse = averagePeopleInHouse;
    }

    // Переопределение метода для расчета плотности населения в деревне
    public override double GetPopulationDensity() => (Homes * AveragePeopleInHouse) / square;
}

class City : LivingPlace
{
    public int Population => h; // Свойство для получения населения в городе

    // Конструктор для инициализации города
    public City(int population, double square) : base(population, square) { }

    // Переопределение метода для расчета плотности населения в городе
    public override double GetPopulationDensity() => Population / square;
}
