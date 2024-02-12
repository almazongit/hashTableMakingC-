using System;
using System.Collections;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Xml.Linq;
class Program
{
    // Функция хэширования - сумма цифр элемента
    static int HashFunction(int value)
    {
        // Суммируем цифры числа
        int sum = 0;
        value %= 1000;
        while (value != 0)
        {
            sum += value % 10;
            value /= 10;
        }

        // Возвращаем остаток от деления суммы на t
        return sum;
    }


    static void Main()
    {
        Random rd = new Random();
        int m = 53;
        Console.WriteLine($"Количество элементов: {m}");

        int n = 5;
        Console.WriteLine($"Размерность: {n} ");

        int min = 0, max = (int)Math.Pow(10, n);
        if (n != 1)
            min = (int)Math.Pow(10, n - 1);

        int t = m * 3 / 2;


        int[] elements = new int[m];
        int[] hashTable = new int[t];

        // Инициализация хэш-таблицы значениями -1
        for (int i = 0; i < t; ++i)
            hashTable[i] = -1;

        // Генерация случайных уникальных ключей
        for (int i = 0; i < m; ++i)
        {
            elements[i] = rd.Next(min, max);
            for (int j = 0; j < i; j++)
            {
                // Проверка на уникальность ключа
                if (elements[j] == elements[i])
                {
                    elements[i] = rd.Next(min, max);
                    j = -1;
                }
            }
        }


        Console.WriteLine("Входные элементы:");
        for (int i = 0; i < m; ++i)
        {

            if (i % 10 == 0)
                Console.WriteLine();
            Console.Write($"{elements[i] + " "}");
        }
        Console.WriteLine("\n");

        int allCount = 0;

        // Хэширование ключей и обработка коллизий методом квадратичного опробовывания
        for (int i = 0; i < m; ++i)
        {
            int deg = 1;
            int fl = 0; //Счетчик для неудачных попыток

            if (hashTable[HashFunction(elements[i])] == -1)
            {
                //Присвоение хэш-таблице, если ячейка пуста
                hashTable[HashFunction(elements[i])] = elements[i];
                allCount++;
            }
            else
            {
                allCount++;

                while (fl < t)
                {
                    int index = (HashFunction(elements[i]) + deg * deg) % t;

                    if (hashTable[index] == -1)
                    {
                        // Квадратичное пробирование
                        hashTable[index] = elements[i];
                        deg = 1;
                        fl = 0;
                        allCount++;
                        break;
                    }
                    else
                    {
                        deg++;
                        fl++;
                        allCount++;
                    }

                    if (fl >= 30)
                    {
                        // Переключение на линейное опробирование после 30 неудачных попыток
                        deg = 1;
                        while (hashTable[index] != -1)
                        {
                            index = (HashFunction(elements[i]) + deg) % t;
                            deg++;
                            fl++;
                            allCount++;
                        }
                        hashTable[index] = elements[i];
                        break;
                    }
                }
            }
        }

        Console.WriteLine("Хэш-таблица:");
        for (int i = 0; i < t; ++i)
        {
            if (i % 5 == 0)
                Console.WriteLine();

            Console.Write($"{i})\t");

            if (hashTable[i] == -1)
                Console.Write("-----\t");
            else
                Console.Write($"{hashTable[i]}\t");
        }

        Console.WriteLine($"\n\nКоэффициент заполнения таблицы: {(float)m / t:F3}");
        Console.WriteLine($"Текущий размер таблицы: {t}");
        Console.WriteLine($"Общее число проб: {allCount - 1}");
        Console.WriteLine($"Среднее число проб, необходимых для размещения некоторого ключа в таблице: {(double)(allCount - 1) / m:F3}");
    }
}



