using System;

namespace lab5.UI
{
    public static class ConsoleMenu
    {
        public static void ShowMainMenu()
        {
            Console.Clear();
            Console.WriteLine("=== СИСТЕМА УПРАВЛЕНИЯ ФУТБОЛЬНЫМИ КЛУБАМИ ===");
            Console.WriteLine("1. Загрузить данные из Excel");
            Console.WriteLine("2. Просмотреть все данные");
            Console.WriteLine("3. Управление данными (CRUD)");
            Console.WriteLine("4. Выполнить запросы");
            Console.WriteLine("5. Сохранить изменения в Excel");
            Console.WriteLine("6. Тестовые данные (демо)");
            Console.WriteLine("0. Выход");
            Console.Write("\nВыберите пункт: ");
        }
        
        public static void ShowCrudMenu()
        {
            Console.Clear();
            Console.WriteLine("=== УПРАВЛЕНИЕ ДАННЫМИ ===");
            Console.WriteLine("1. Добавить страну");
            Console.WriteLine("2. Добавить клуб");
            Console.WriteLine("3. Добавить достижение");
            Console.WriteLine("4. Удалить страну");
            Console.WriteLine("5. Удалить клуб");
            Console.WriteLine("6. Удалить достижение");
            Console.WriteLine("7. Найти страну по ID");
            Console.WriteLine("8. Найти клуб по ID");
            Console.WriteLine("9. Найти достижение по ID");
            Console.WriteLine("0. Назад");
            Console.Write("\nВыберите пункт: ");
        }
        
        public static void ShowQueriesMenu()
        {
            Console.Clear();
            Console.WriteLine("=== LINQ ЗАПРОСЫ ===");
            Console.WriteLine("1. Страны по алфавиту (1 таблица)");
            Console.WriteLine("2. Клубы с названиями стран (2 таблицы)");
            Console.WriteLine("3. Страна с наибольшим количеством золотых медалей (3 таблицы, одно значение)");
            Console.WriteLine("4. Клубы с золотыми медалями, но без кубков (3 таблицы, перечень)");
            Console.WriteLine("5. Пример из задания: ID страны клуба-чемпиона без кубков");
            Console.WriteLine("0. Назад");
            Console.Write("\nВыберите запрос: ");
        }
        
        public static void ShowSaveMenu()
        {
            Console.Clear();
            Console.WriteLine("=== СОХРАНЕНИЕ ДАННЫХ ===");
            Console.WriteLine("1. Сохранить в исходный файл");
            Console.WriteLine("2. Сохранить как новый файл");
            Console.WriteLine("0. Назад");
            Console.Write("\nВыберите пункт: ");
        }
    }
}