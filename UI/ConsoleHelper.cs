using System;

namespace lab5.UI
{
    public static class ConsoleHelper
    {
        public static void PressAnyKey()
        {
            Console.WriteLine("\nНажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }
        
        public static string ReadString(string prompt, string defaultValue = "")
        {
            Console.Write(prompt);
            if (!string.IsNullOrEmpty(defaultValue))
                Console.Write($" [{defaultValue}]: ");
            else
                Console.Write(": ");
            
            string input = Console.ReadLine()?.Trim();
            return string.IsNullOrEmpty(input) ? defaultValue : input;
        }
        
        public static int ReadInt(string prompt, int defaultValue = 0)
        {
            while (true)
            {
                Console.Write(prompt);
                if (defaultValue != 0)
                    Console.Write($" [{defaultValue}]: ");
                else
                    Console.Write(": ");
                
                string input = Console.ReadLine()?.Trim();
                
                if (string.IsNullOrEmpty(input) && defaultValue != 0)
                    return defaultValue;
                
                if (int.TryParse(input, out int result))
                    return result;
                
                Console.WriteLine("Ошибка: введите целое число!");
            }
        }
        
        public static bool ReadYesNo(string prompt, bool defaultValue = false)
        {
            Console.Write(prompt);
            Console.Write(defaultValue ? " [Y/n]: " : " [y/N]: ");
            
            string input = Console.ReadLine()?.Trim().ToLower();
            
            if (string.IsNullOrEmpty(input))
                return defaultValue;
            
            return input == "y" || input == "да";
        }
        
        public static void ShowError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"ОШИБКА: {message}");
            Console.ResetColor();
        }
        
        public static void ShowSuccess(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"✓ {message}");
            Console.ResetColor();
        }
        
        public static void ShowWarning(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"⚠ {message}");
            Console.ResetColor();
        }
        
        public static void ShowTitle(string title)
        {
            Console.WriteLine($"\n=== {title.ToUpper()} ===");
        }
    }
}