using System;

internal class Program
{
    static void Main(string[] args)
    {
        // Создание экземпляров тетрадей
        Notebook notebook1 = new Notebook("В клетку", 12);  // Обычная тетрадь
        CoveredNotebook notebook2 = new CoveredNotebook("В линейку", 24, "Картон"); // Тетрадь с обложкой

        // Вывод информации о тетрадях
        Console.WriteLine(notebook1);
        Console.WriteLine(notebook2);

        // Ожидание ввода пользователя перед завершением программы
        Console.ReadLine();
    }
}

class Notebook
{
    private string name; // Название тетради
    public string Name => name; // Свойство для получения названия тетради
    private int k; // Количество листов
    public int Sheets => k; // Свойство для получения количества листов
    public virtual int Price => k * 15; // Свойство для расчета цены тетради

    // Конструктор для инициализации тетради с именем и количеством листов
    public Notebook(string name, int k)
    {
        this.name = name;
        this.k = k;
    }

    // Переопределение метода ToString для представления информации о тетради
    public override string ToString() => $"Информация о тетради\n\tНазвание: {name}\n\tКол-во листов: {k}\n\tЦена: {Price}";
}

class CoveredNotebook : Notebook
{
    // Переопределение цены для тетради с обложкой, добавляя 50 единиц к базовой цене
    public override int Price => base.Price + 50;
    private string coverMaterial; // Материал обложки

    // Конструктор для инициализации тетради с обложкой
    public CoveredNotebook(string name, int k, string material) : base(name, k)
    {
        coverMaterial = material; // Присвоение материала обложки
    }

    // Переопределение метода ToString для добавления информации о материале обложки
    public override string ToString() => base.ToString() + $"\n\tМатериал обложки: {coverMaterial}";
}
