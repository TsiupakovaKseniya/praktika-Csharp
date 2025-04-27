using System;
using System.Collections.Generic;
using System.Linq;

    class Program
    {
        // Класс Car реализует интерфейсы IComparable и IComparer для сравнения объектов автомобиля
        public class Car : IComparable<Car>, IComparer<Car>
        {
            // Свойства автомобиля
            public string Brand { get; set; } // Марка автомобиля
            public string Owner { get; set; } // Владелец автомобиля
            public int YearOfPurchase { get; set; } // Год приобретения
            public int Mileage { get; set; } // Пробег автомобиля

            // Конструктор для инициализации свойств автомобиля
            public Car(string brand, string owner, int yearOfPurchase, int mileage)
            {
                Brand = brand;
                Owner = owner;
                YearOfPurchase = yearOfPurchase;
                Mileage = mileage;
            }

            // Реализация метода CompareTo для сравнения по пробегу
            public int CompareTo(Car other) => Mileage.CompareTo(other.Mileage);

            // Реализация метода Compare из интерфейса IComparer
            int IComparer<Car>.Compare(Car x, Car y)
            {
                // Сравниваем автомобили по пробегу с использованием обычной логики
                if (x.Mileage < y.Mileage) return -1; // x меньше y
                if (x.Mileage > y.Mileage) return 1; // x больше y

                return 0; // x равен y
            }
        }

        static void Main(string[] args)
        {
            // Создаем список автомобилей
            List<Car> cars = new List<Car>
            {
                new Car("Toyota", "Иванов", 2010, 150000),
                new Car("Ford", "Петров", 2005, 200000),
                new Car("BMW", "Сидоров", 2015, 50000),
                new Car("Audi", "Кузнецов", 2000, 300000),
                new Car("Nissan", "Смирнов", 2008, 120000)
            };

            // Выводим информацию о всех машинах
            Console.WriteLine("Машины");
            foreach (var car in cars)
            {
                Console.WriteLine($"Марка: {car.Brand}, Владелец: {car.Owner}, Год приобретения: {car.YearOfPurchase}, Пробег: {car.Mileage}");
            }

            // Считываем год для фильтрации автомобилей
            int filterYear = int.Parse(Console.ReadLine());

            // Фильтруем автомобили по году приобретения
            var filteredCars = cars
                .Where(car => car.YearOfPurchase < filterYear) // Сравниваем с введенным годом
                .ToList();

            // Сортируем отфильтрованные автомобили по пробегу
            filteredCars.Sort();

            // Выводим список отфильтрованных и отсортированных автомобилей
            Console.WriteLine("Автомобили, выпущенные ранее " + filterYear + " года, отсортированные по пробегу:");
            foreach (var car in filteredCars)
            {
                Console.WriteLine($"Марка: {car.Brand}, Владелец: {car.Owner}, Год приобретения: {car.YearOfPurchase}, Пробег: {car.Mileage}");
            }

            // Ожидание ввода пользователя перед завершением программы
            Console.ReadLine();
        }
    }

