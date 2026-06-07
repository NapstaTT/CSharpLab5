///FIXME:
/// - Приведены скобки к стилю Allman
/// - Добавлена XML-документация
/// - Переименованы локальные переменные в camelCase
/// - Удалены лишние пустые строки
/// - Улучшена читаемость и структура методов

using System;

namespace lab5.UI
{
    /// <summary>
    /// Вспомогательные методы для взаимодействия с консолью.
    /// </summary>
    public static class ConsoleHelper
    {
        /// <summary>
        /// Ожидает нажатия любой клавиши пользователем.
        /// </summary>
        public static void PressAnyKey()
        {
            Console.WriteLine("\nНажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }

        /// <summary>
        /// Считывает строку с консоли с возможностью указать значение по умолчанию.
        /// </summary>
        public static string ReadString(string prompt, string defaultValue = "")
        {
            Console.Write(prompt);

            if (!string.IsNullOrEmpty(defaultValue))
            {
                Console.Write($" [{defaultValue}]: ");
            }
            else
            {
                Console.Write(": ");
            }

            var input = Console.ReadLine()?.Trim();
            return string.IsNullOrEmpty(input) ? defaultValue : input;
        }

        /// <summary>
        /// Считывает целое число с консоли.
        /// </summary>
        public static int ReadInt(string prompt, int defaultValue = 0)
        {
            while (true)
            {
                Console.Write(prompt);

                if (defaultValue != 0)
                {
                    Console.Write($" [{defaultValue}]: ");
                }
                else
                {
                    Console.Write(": ");
                }

                var input = Console.ReadLine()?.Trim();

                if (string.IsNullOrEmpty(input) && defaultValue != 0)
                {
                    return defaultValue;
                }

                if (int.TryParse(input, out var result))
                {
                    return result;
                }

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Ошибка: введите целое число!");
                Console.ResetColor();
            }
        }

        /// <summary>
        /// Считывает ответ Да/Нет.
        /// </summary>
        public static bool ReadYesNo(string prompt, bool defaultValue = false)
        {
            Console.Write(prompt);
            Console.Write(defaultValue ? " [Y/n]: " : " [y/N]: ");

            var input = Console.ReadLine()?.Trim().ToLower();

            if (string.IsNullOrEmpty(input))
            {
                return defaultValue;
            }

            return input == "y" || input == "да";
        }

        /// <summary>
        /// Выводит сообщение об ошибке красным цветом.
        /// </summary>
        public static void ShowError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"ОШИБКА: {message}");
            Console.ResetColor();
        }

        /// <summary>
        /// Выводит сообщение об успешном выполнении зелёным цветом.
        /// </summary>
        public static void ShowSuccess(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"✓ {message}");
            Console.ResetColor();
        }

        /// <summary>
        /// Выводит предупреждение жёлтым цветом.
        /// </summary>
        public static void ShowWarning(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"⚠ {message}");
            Console.ResetColor();
        }

        /// <summary>
        /// Выводит заголовок раздела.
        /// </summary>
        public static void ShowTitle(string title)
        {
            Console.WriteLine($"\n=== {title.ToUpper()} ===");
        }
    }
}
