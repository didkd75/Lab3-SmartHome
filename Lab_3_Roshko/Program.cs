using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartHomeApp
{
    internal class Program
    {
        private static List<SmartDevice> devices = new List<SmartDevice>();
        private static int maxCapacity = 0;

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            while (true)
            {
                Console.Write("Введіть максимальну кількість пристроїв N (N > 0): ");
                string input = Console.ReadLine();
                if (int.TryParse(input, out maxCapacity) && maxCapacity > 0)
                {
                    break;
                }
                Console.WriteLine("Error: має бути ціле додатне число!");
            }

            bool exitProgram = false;
            while (!exitProgram)
            {
                Console.WriteLine("\n=== МЕНЮ КЕРУВАННЯ РОЗУМНИМ ДОМОМ (Lab-3) ===");
                Console.WriteLine("1 – Додати об'єкт");
                Console.WriteLine("2 – Переглянути всі об'єкти");
                Console.WriteLine("3 – Знайти об'єкт");
                Console.WriteLine("4 – Продемонструвати поведінку");
                Console.WriteLine("5 – Видалити об'єкт");
                Console.WriteLine("0 – Вийти з програми");
                Console.Write("Оберіть пункт меню: ");

                string choice = Console.ReadLine();
                Console.WriteLine();

                switch (choice)
                {
                    case "1": AddDeviceMenu(); break;
                    case "2": ShowAllDevices(); break;
                    case "3": SearchDeviceMenu(); break;
                    case "4": DemonstrateBehaviorMenu(); break;
                    case "5": DeleteDeviceMenu(); break;
                    case "0":
                        exitProgram = true;
                        Console.WriteLine("Програму успішно завершено. До побачення!");
                        break;
                    default:
                        Console.WriteLine("Некоректний вибір! Спробуйте ще раз.");
                        break;
                }
            }
        }

        // Допоміжні методи безпечного зчитування з валідацією через тимчасовий об'єкт
        private static string ReadValidSerial()
        {
            SmartDevice temp = new SmartDevice();
            Console.Clear(); // Очищення логів створення тимчасового об'єкта
            while (true)
            {
                Console.Write("Введіть серійний номер (6 цифр): ");
                try
                {
                    temp.SerialNumber = Console.ReadLine();
                    return temp.SerialNumber;
                }
                catch (Exception ex) { Console.WriteLine(ex.Message); }
            }
        }

        private static string ReadValidName()
        {
            SmartDevice temp = new SmartDevice();
            while (true)
            {
                Console.Write("Введіть назву пристрою (3-20 символів): ");
                try
                {
                    temp.DeviceName = Console.ReadLine();
                    return temp.DeviceName;
                }
                catch (Exception ex) { Console.WriteLine(ex.Message); }
            }
        }

        private static DeviceType ReadValidType()
        {
            SmartDevice temp = new SmartDevice();
            while (true)
            {
                Console.WriteLine("Оберіть тип: 1 - Thermostat, 2 - SmartLight, 3 - SecurityCamera, 4 - SmartLock");
                Console.Write("Ваш вибір: ");
                try
                {
                    if (int.TryParse(Console.ReadLine(), out int val))
                    {
                        temp.Type = (DeviceType)val;
                        return temp.Type;
                    }
                    throw new FormatException("Error: введене значення має бути числом!");
                }
                catch (Exception ex) { Console.WriteLine(ex.Message); }
            }
        }

        private static double ReadValidPower()
        {
            SmartDevice temp = new SmartDevice();
            while (true)
            {
                Console.Write("Введіть потужність Вт (0..500): ");
                try
                {
                    if (double.TryParse(Console.ReadLine(), out double val))
                    {
                        temp.PowerUsage = val;
                        return temp.PowerUsage;
                    }
                    throw new FormatException("Error: некоректний формат числа!");
                }
                catch (Exception ex) { Console.WriteLine(ex.Message); }
            }
        }

        // Пункт 1: Вибір конструктора при створенні об'єкта
        private static void AddDeviceMenu()
        {
            if (devices.Count >= maxCapacity)
            {
                Console.WriteLine($"Error: досягнуто ліміт пристроїв N = {maxCapacity}. Додавання неможливе!");
                return;
            }

            Console.WriteLine("Оберіть спосіб / конструктор створення об'єкта:");
            Console.WriteLine("1 – Конструктор без параметрів: SmartDevice()");
            Console.WriteLine("2 – Конструктор з 2 параметрами: SmartDevice(serialNumber, deviceName)");
            Console.WriteLine("3 – Конструктор з 4 параметрами: SmartDevice(serialNumber, deviceName, type, powerUsage)");
            Console.WriteLine("4 – Конструктор з 5 параметрами: SmartDevice(serialNumber, deviceName, type, powerUsage, isActive)");
            Console.WriteLine("5 – Автоматична генерація випадкового об'єкта");
            Console.Write("Ваш вибір: ");
            string choice = Console.ReadLine();
            Console.WriteLine();

            try
            {
                SmartDevice newDevice = null;

                switch (choice)
                {
                    case "1":
                        newDevice = new SmartDevice();
                        break;

                    case "2":
                        string sn2 = ReadValidSerial();
                        string name2 = ReadValidName();
                        Console.WriteLine();
                        newDevice = new SmartDevice(sn2, name2);
                        break;

                    case "3":
                        string sn3 = ReadValidSerial();
                        string name3 = ReadValidName();
                        DeviceType type3 = ReadValidType();
                        double power3 = ReadValidPower();
                        Console.WriteLine();
                        newDevice = new SmartDevice(sn3, name3, type3, power3);
                        break;

                    case "4":
                        string sn4 = ReadValidSerial();
                        string name4 = ReadValidName();
                        DeviceType type4 = ReadValidType();
                        double power4 = ReadValidPower();
                        Console.Write("Пристрій увімкнений? (1 - Так, 0 - Ні): ");
                        bool active4 = Console.ReadLine() == "1";
                        Console.WriteLine();
                        newDevice = new SmartDevice(sn4, name4, type4, power4, active4);
                        break;

                    case "5":
                        Random rnd = new Random();
                        string autoSn = rnd.Next(100000, 999999).ToString();
                        string autoName = "SmartNode" + rnd.Next(10, 99);
                        DeviceType autoType = (DeviceType)rnd.Next(1, 5);
                        double autoPower = Math.Round(rnd.NextDouble() * 150, 1);
                        bool autoActive = rnd.Next(0, 2) == 1;
                        newDevice = new SmartDevice(autoSn, autoName, autoType, autoPower, autoActive);
                        break;

                    default:
                        Console.WriteLine("Некоректний вибір конструктора!");
                        return;
                }

                devices.Add(newDevice);
                Console.WriteLine("Об'єкт успішно створено та додано до колекції!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка створення об'єкта: {ex.Message}");
            }
        }

        private static void ShowAllDevices()
        {
            PrintTable(devices);
        }

        private static void PrintTable(List<SmartDevice> list)
        {
            if (list == null || list.Count == 0)
            {
                Console.WriteLine("Жодного об'єкта не знайдено.");
                return;
            }

            Console.WriteLine(new string('-', 88));
            Console.WriteLine($"| {"№",-3} | {"Серійний номер",-14} | {"Назва пристрою",-16} | {"Тип",-14} | {"Потужність",-10} | {"Стан",-12} |");
            Console.WriteLine(new string('-', 88));

            for (int i = 0; i < list.Count; i++)
            {
                int index = devices.IndexOf(list[i]) + 1;
                string state = list[i].IsActive ? "Увімкнено" : "Вимкнено";
                Console.WriteLine($"| {index,-3} | {list[i].SerialNumber,-14} | {list[i].DeviceName,-16} | {list[i].Type,-14} | {list[i].PowerUsage + "W",-10} | {state,-12} |");
            }
            Console.WriteLine(new string('-', 88));
        }

        private static void SearchDeviceMenu()
        {
            if (devices.Count == 0)
            {
                Console.WriteLine("Жодного об'єкта немає в системі.");
                return;
            }

            Console.WriteLine("Оберіть характеристику для пошуку:");
            Console.WriteLine("1 – За серійним номером");
            Console.WriteLine("2 – За типом пристрою");
            Console.Write("Ваш вибір: ");
            string choice = Console.ReadLine();

            List<SmartDevice> results = new List<SmartDevice>();

            if (choice == "1")
            {
                Console.Write("Введіть серійний номер для пошуку: ");
                string sn = Console.ReadLine();
                results = devices.Where(d => d.SerialNumber.Equals(sn, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            else if (choice == "2")
            {
                Console.Write("Введіть назву типу (Thermostat, SmartLight, SecurityCamera, SmartLock): ");
                string typeStr = Console.ReadLine();
                results = devices.Where(d => d.Type.ToString().Equals(typeStr, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            PrintTable(results);
        }

        // Пункт 4: Демонстрація роботи всіх перевантажених версій методу CalibratePower
        private static void DemonstrateBehaviorMenu()
        {
            if (devices.Count == 0)
            {
                Console.WriteLine("Жодного об'єкта немає для демонстрації.");
                return;
            }

            ShowAllDevices();
            Console.Write("Введіть № пристрою у таблиці: ");
            if (int.TryParse(Console.ReadLine(), out int index) && index >= 1 && index <= devices.Count)
            {
                SmartDevice dev = devices[index - 1];
                Console.WriteLine($"\n--- Демонстрація поведінки об'єкта '{dev.DeviceName}' ---");

                dev.TogglePower();

                // 1. Виклик CalibratePower(double newUsage)
                while (true)
                {
                    Console.Write("\n[Перевантаження 1] Введіть нову точну потужність (0..500 Вт): ");
                    try
                    {
                        if (double.TryParse(Console.ReadLine(), out double p))
                        {
                            dev.CalibratePower(p);
                            break;
                        }
                        throw new FormatException("Error: некоректне число!");
                    }
                    catch (Exception ex) { Console.WriteLine(ex.Message); }
                }

                // 2. Виклик CalibratePower(double delta, bool isPercentage)
                while (true)
                {
                    Console.Write("\n[Перевантаження 2] Введіть зміну потужності у відсотках (напр. 10 або -20): ");
                    try
                    {
                        if (double.TryParse(Console.ReadLine(), out double percent))
                        {
                            dev.CalibratePower(percent, true);
                            break;
                        }
                        throw new FormatException("Error: некоректне число!");
                    }
                    catch (Exception ex) { Console.WriteLine(ex.Message); }
                }

                // 3. Виклик CalibratePower() без параметрів
                Console.WriteLine("\n[Перевантаження 3] Скидання потужності до заводського значення:");
                dev.CalibratePower();

                Console.WriteLine("\n" + dev.GetDiagnosticsReport());
            }
            else
            {
                Console.WriteLine("Error: некоректний порядковий номер!");
            }
        }

        private static void DeleteDeviceMenu()
        {
            if (devices.Count == 0)
            {
                Console.WriteLine("Жодного об'єкта немає в системі.");
                return;
            }

            Console.WriteLine("1 – За порядковим номером");
            Console.WriteLine("2 – За характеристикою (серійний номер)");
            Console.Write("Ваш вибір: ");
            string choice = Console.ReadLine();

            if (choice == "1")
            {
                Console.Write("Введіть порядковий номер: ");
                if (int.TryParse(Console.ReadLine(), out int index) && index >= 1 && index <= devices.Count)
                {
                    devices.RemoveAt(index - 1);
                    Console.WriteLine("Об'єкт успішно видалено!");
                }
                else
                {
                    Console.WriteLine("Error: некоректне значення порядкового номера!");
                }
            }
            else if (choice == "2")
            {
                Console.Write("Введіть серійний номер: ");
                string sn = Console.ReadLine();
                int count = devices.RemoveAll(d => d.SerialNumber.Equals(sn, StringComparison.OrdinalIgnoreCase));

                if (count > 0)
                    Console.WriteLine($"Успішно видалено об'єктів: {count}");
                else
                    Console.WriteLine("Відсутність об'єктів для видалення із заданою характеристикою.");
            }
        }
    }
}