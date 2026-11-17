using System;
using System.Linq;

namespace SmartHomeApp
{
    public class SmartDevice
    {
        // Приватні поля
        private string _serialNumber;
        private string _deviceName;
        private DeviceType _type;
        private double _powerUsage;
        private bool _isActive;

        // Автовластивість зі значенням за замовчуванням
        public DateTime InstallationDate { get; set; } = DateTime.Now;

        // Властивість із різним рівнем доступу
        public int ErrorCount { get; private set; } = 0;

        // Властивості з валідацією та генерацією виключень
        public string SerialNumber
        {
            get => _serialNumber;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentNullException(nameof(value), "Error: відсутнє значення!");
                if (!value.All(char.IsDigit))
                    throw new FormatException("Error: серійний номер має містити тільки цифри!");
                if (value.Length != 6)
                    throw new ArgumentException("Error: серійний номер має складатися з 6 цифр!");

                _serialNumber = value;
            }
        }

        public string DeviceName
        {
            get => _deviceName;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentNullException(nameof(value), "Error: відсутнє значення!");
                if (value.Length < 3 || value.Length > 20)
                    throw new ArgumentException("Error: довжина назви має бути від 3 до 20 символів!");
                if (!value.All(c => char.IsLetterOrDigit(c) || char.IsWhiteSpace(c)))
                    throw new FormatException("Error: назва має містити тільки букви, цифри або пробіли!");

                _deviceName = value;
            }
        }

        public DeviceType Type
        {
            get => _type;
            set
            {
                if (!Enum.IsDefined(typeof(DeviceType), value))
                    throw new ArgumentException("Error: некоректний тип пристрою!");

                _type = value;
            }
        }

        public double PowerUsage
        {
            get => _powerUsage;
            set
            {
                if (value < 0 || value > 500)
                    throw new ArgumentOutOfRangeException(nameof(value), "Error: значення потужності має бути у діапазоні [0..500] Вт!");

                _powerUsage = value;
            }
        }

        public bool IsActive
        {
            get => _isActive;
            set => _isActive = value;
        }

        // Обчислювальна властивість
        public string StatusSummary => $"[{_type}] {_deviceName} - {(_isActive ? "Увімкнено" : "Вимкнено")} ({_powerUsage} W)";

        // [НОВЕ в Lab-3]: ПЕРЕВАНТАЖЕНІ КОНСТРУКТОРИ З ЛАНЦЮГОВИМ ВИКЛИКОМ (: this)

        // 1. Конструктор без параметрів (викликає конструктор з 2 параметрами)
        public SmartDevice() : this("000000", "DefaultDevice")
        {
            Console.WriteLine("-> Використано конструктор №1: SmartDevice() без параметрів.");
        }

        // 2. Конструктор з 2 параметрами (викликає конструктор з 4 параметрами)
        public SmartDevice(string serialNumber, string deviceName) : this(serialNumber, deviceName, DeviceType.SmartLight, 10.0)
        {
            Console.WriteLine("-> Використано конструктор №2: SmartDevice(serialNumber, deviceName).");
        }

        // 3. Конструктор з 4 параметрами (викликає головний конструктор з 5 параметрами)
        public SmartDevice(string serialNumber, string deviceName, DeviceType type, double powerUsage)
            : this(serialNumber, deviceName, type, powerUsage, false)
        {
            Console.WriteLine("-> Використано конструктор №3: SmartDevice(serialNumber, deviceName, type, powerUsage).");
        }

        // 4. Головний конструктор з 5 параметрами
        public SmartDevice(string serialNumber, string deviceName, DeviceType type, double powerUsage, bool isActive)
        {
            SerialNumber = serialNumber;
            DeviceName = deviceName;
            Type = type;
            PowerUsage = powerUsage;
            IsActive = isActive;
            Console.WriteLine("-> Використано конструктор №4: SmartDevice(serialNumber, deviceName, type, powerUsage, isActive).");
        }

        // =========================================================================

        // Приватний метод для приховування деталей реалізації
        private string FormatLogMessage(string action)
        {
            return $"[{DateTime.Now:HH:mm:ss}] [Пристрій '{_deviceName}']: {action}";
        }

        public void TogglePower()
        {
            _isActive = !_isActive;
            string status = _isActive ? "Увімкнено" : "Вимкнено";
            Console.WriteLine(FormatLogMessage($"Стан змінено на '{status}'"));
        }

        // [НОВЕ в Lab-3]: ПЕРЕВАНТАЖЕННЯ МЕТОДУ (OVERLOADING) CalibratePower

        // Варіант 1: Встановлення нового значення потужності (double)
        public void CalibratePower(double newUsage)
        {
            PowerUsage = newUsage;
            Console.WriteLine(FormatLogMessage($"[Overload 1] Потужність встановлено на {_powerUsage} Вт"));
        }

        // Варіант 2: Коригування потужності на певну дельту/відсоток (double, bool)
        public void CalibratePower(double delta, bool isPercentage)
        {
            double calculated = isPercentage
                ? _powerUsage + (_powerUsage * delta / 100.0)
                : _powerUsage + delta;

            PowerUsage = Math.Round(calculated, 2);
            string mode = isPercentage ? $"{delta}%" : $"{delta} Вт";
            Console.WriteLine(FormatLogMessage($"[Overload 2] Потужність скориговано на {mode}. Поточна: {_powerUsage} Вт"));
        }

        // Варіант 3: Скидання до базової потужності за типом пристрою (без параметрів)
        public void CalibratePower()
        {
            double defaultPower = _type switch
            {
                DeviceType.SmartLight => 12.0,
                DeviceType.Thermostat => 5.0,
                DeviceType.SecurityCamera => 18.0,
                DeviceType.SmartLock => 3.5,
                _ => 10.0
            };
            PowerUsage = defaultPower;
            Console.WriteLine(FormatLogMessage($"[Overload 3] Скинуто до заводської потужності ({_powerUsage} Вт)"));
        }

        // =========================================================================

        public string GetDiagnosticsReport()
        {
            string log = FormatLogMessage($"Діагностика. Встановлено: {InstallationDate:dd.MM.yyyy}. Помилок: {ErrorCount}");
            return $"{log}\nСтатус: {StatusSummary}";
        }
    }
}