using System;

internal class Program
{
    static void Main(string[] args)
    {
        // Создаем две последовательности строк
        StringSequence firstSeq = new StringSequence(new string[] { "hey", "bye" });
        StringSequence secondSeq = new StringSequence(new string[] { "hello", "bye" });

        // Сравниваем две последовательности и выводим результат
        // Сравнение выполняется с использованием перегруженных операторов == и !=
        Console.WriteLine($"{firstSeq == secondSeq} {firstSeq != secondSeq}");

        // Присваиваем вторую последовательность первой
        firstSeq.Seq = secondSeq.Seq;

        // Сравниваем последовательности снова после изменения
        Console.WriteLine($"{firstSeq == secondSeq} {firstSeq != secondSeq}");

        // Ожидаем нажатия клавиши перед выходом
        Console.ReadLine();
    }
}

class StringSequence
{
    private string[] arr; // Массив для хранения последовательности строк

    // Свойство для доступа к массиву строк
    public string[] Seq
    {
        get => arr; // Возвращаем массив строк
        set => arr = value; // Устанавливаем новый массив строк
    }

    // Конструктор, принимающий массив строк в качестве параметра
    public StringSequence(string[] arr)
    {
        this.arr = arr; // Инициализируем массив строк
    }

    // Пустой конструктор
    public StringSequence() { }

    // Перегрузка оператора == для сравнения двух объектов StringSequence
    public static bool operator ==(StringSequence left, StringSequence right)
    {
        // Если один из объектов равен null, проверяем равен ли другой объект null
        if (ReferenceEquals(left, right)) return true;
        if (ReferenceEquals(left, null) || ReferenceEquals(right, null)) return false;

        // Если длины массивов различаются, возвращаем false
        if (left.arr.Length != right.arr.Length)
            return false;

        // Сравниваем элементы массивов поэлементно
        for (int i = 0; i < left.arr.Length; i++)
            if (left.arr[i] != right.arr[i])
                return false; // Если хоть один элемент не равен, возвращаем false

        return true; // Все элементы равны, возвращаем true
    }

    // Перегрузка оператора != для сравнения двух объектов StringSequence
    public static bool operator !=(StringSequence left, StringSequence right) => !(left == right);
}
