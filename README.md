# Лабораторная работа №5: LINQ-запросы к базе данных
## Выполнил: Гилев Артем Александрович, группа ИТ-10

### Цель работы
Освоение технологии LINQ (Language Integrated Query) в C# для работы с данными, включая чтение Excel-файлов, выполнение сложных запросов и реализацию CRUD-операций.

### Содержание
1. Задание и варианты
2. Структура проекта
3. Работа с Excel-файлами (NPOI)
4. Реализация LINQ-запросов
   - 4.1 Запрос к одной таблице
   - 4.2 Запрос к двум таблицам
   - 4.3 Запросы к трём таблицам (одно значение и перечень)
5. Основные классы и их назначение
6. Примеры работы программы
7. Заключение

---

## Задание

**Общее задание:** Разработать консольное приложение для работы с базой данных из Excel-файла с возможностью выполнения LINQ-запросов.

**Мой вариант (5):** База данных "Европейские футбольные клубы" с таблицами:
- `Страны` - информация о странах-членах UEFA
- `Клубы` - информация о футбольных клубах
- `Достижения` - информация о выигранных медалях и кубках

**Требования к запросам:**
1. 1 запрос с обращением к одной таблице
2. 1 запрос с обращением к двум таблицам
3. 2 запроса с обращением к трём таблицам (один возвращает одно значение, другой - перечень)

---

## Структура проекта

```
Lab5_Variant5/
├── Data/
│   ├── Models/              # Классы сущностей (POCO)
│   │   ├── Country.cs       # Страна (ID, Название)
│   │   ├── Club.cs          # Клуб (ID, Название, ID страны)
│   │   └── Achievement.cs   # Достижения (медали, кубки)
│   │
│   ├── DatabaseContext.cs   # Контейнер данных (списки)
│   │
│   └── Repositories/        # Работа с внешними источниками
│       ├── IExcelRepository.cs  # Интерфейс для Excel
│       └── ExcelRepository.cs   # Реализация с NPOI
│
├── Services/
│   ├── IDatabaseService.cs  # Интерфейс CRUD-операций
│   ├── DatabaseService.cs   # Реализация CRUD
│   │
│   └── Queries/             # Логика запросов
│       ├── IQueryService.cs     # Интерфейс запросов
│       └── QueryService.cs      # Реализация LINQ-запросов
│
├── UI/                      # Пользовательский интерфейс
│   ├── ConsoleMenu.cs       # Меню программы
│   └── ConsoleHelper.cs     # Вспомогательные методы ввода/вывода
│
└── Program.cs               # Точка входа, главная логика
```

**Принцип работы:** Разделение ответственности (SoC). Каждый класс отвечает за свою часть функционала.

---

## Работа с Excel-файлами

### Используемая библиотека: NPOI
Библиотека для чтения/записи Excel-файлов (.xls, .xlsx) без установки Microsoft Office.

**Установка:**
```bash
dotnet add package NPOI
```

**Основные классы NPOI:**
- `HSSFWorkbook` - работа с .xls файлами
- `ISheet` - лист Excel
- `IRow` - строка таблицы
- `ICell` - ячейка

**Загрузка данных из Excel:**
```csharp
using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
{
    IWorkbook workbook = new HSSFWorkbook(fs);
    ISheet sheet = workbook.GetSheet("Страны");
    
    // Чтение строк
    for (int row = 1; row <= sheet.LastRowNum; row++)
    {
        IRow currentRow = sheet.GetRow(row);
        // ... обработка данных
    }
}
```

**Особенности:** 
- Первая строка (0) считается заголовком
- Типы данных определяются автоматически (NumericCellValue, StringCellValue)
- Обработка ошибок при чтении повреждённых файлов

---

## Реализация LINQ-запросов

### 4.1 Запрос к одной таблице: `GetCountriesSortedByName()`

**Задача:** Получить список всех стран, отсортированный по названию.

**Реализация:**
```csharp
public List<Country> GetCountriesSortedByName()
{
    return _context.Countries
        .OrderBy(c => c.Name)
        .ToList();
}
```

**Что происходит:**
1. Берём коллекцию `Countries` из контекста данных
2. Сортируем по полю `Name` (по алфавиту)
3. Преобразуем в список

---

### 4.2 Запрос к двум таблицам: `GetClubsWithCountryNames()`

**Задача:** Получить список клубов с названиями их стран.

**Реализация:**
```csharp
public List<dynamic> GetClubsWithCountryNames()
{
    var query = from club in _context.Clubs
                join country in _context.Countries 
                on club.CountryId equals country.Id
                orderby country.Name, club.Name
                select new
                {
                    ClubId = club.Id,
                    ClubName = club.Name,
                    CountryName = country.Name
                };
    
    return query.ToList<dynamic>();
}
```

**Что происходит:**
1. **JOIN** - соединяем таблицы `Clubs` и `Countries` по полю `CountryId`
2. **SELECT** - выбираем нужные поля из обеих таблиц
3. **ORDER BY** - сортируем сначала по стране, потом по клубу
4. Создаём анонимный тип для результата

**Особенности:**
- Используется `dynamic` для удобства работы с разными типами результатов
- JOIN выполняется по принципу "внешний ключ = первичный ключ"

---

### 4.3.1 Запрос к трём таблицам (одно значение): `GetCountryWithMostGoldMedals()`

**Задача:** Найти страну с наибольшим общим количеством золотых медалей.

**Реализация:**
```csharp
public string GetCountryWithMostGoldMedals()
{
    var result = (from achievement in _context.Achievements
                 join club in _context.Clubs on achievement.ClubId equals club.Id
                 join country in _context.Countries on club.CountryId equals country.Id
                 group achievement by country.Name into g
                 select new
                 {
                     CountryName = g.Key,
                     TotalGold = g.Sum(a => a.З)
                 })
                 .OrderByDescending(x => x.TotalGold)
                 .FirstOrDefault();
    
    return result != null ? $"{result.CountryName}: {result.TotalGold} золотых медалей" : "Нет данных";
}
```

**Что происходит:**
1. **Двойной JOIN** - связываем 3 таблицы через ключи
2. **GROUP BY** - группируем по названию страны
3. **SUM** - суммируем золотые медали в каждой группе
4. **ORDER BY DESC** - сортируем по убыванию суммы
5. **FirstOrDefault** - берём первую (максимальную) запись

**Сложности:**
- Многоступенчатое соединение таблиц
- Агрегация данных (суммирование)
- Работа с группировкой

---

### 4.3.2 Запрос к трём таблицам (перечень): `GetClubsWithGoldMedalsButNoCups()`

**Задача:** Найти клубы, которые имеют золотые медали, но никогда не выигрывали кубок.

**Реализация:**
```csharp
public List<string> GetClubsWithGoldMedalsButNoCups()
{
    var query = from achievement in _context.Achievements
                where achievement.З > 0 && achievement.К == 0
                join club in _context.Clubs on achievement.ClubId equals club.Id
                join country in _context.Countries on club.CountryId equals country.Id
                orderby achievement.З descending
                select $"{club.Name} ({country.Name}): {achievement.З} золотых медалей, 0 кубков";
    
    return query.Distinct().ToList();
}
```

**Что происходит:**
1. **WHERE** - фильтруем достижения (золотые медали есть, кубков нет)
2. **JOIN** - присоединяем информацию о клубах и странах
3. **SELECT** - формируем строку результата
4. **Distinct** - убираем дубликаты
5. **ORDER BY** - сортируем по количеству медалей (убывание)

**Особенности:**
- Комплексное условие фильтрации
- Форматирование строки результата
- Удаление дубликатов через `Distinct()`

---

### Дополнительный запрос (пример из задания): `GetCountryIdOfChampionWithoutCups()`

**Задача из варианта:** Определить клубы, которые побеждали в чемпионате, но ни разу не выиграли национальный кубок. Из них выбрать клуб с наибольшим количеством побед. Если таких несколько - выбрать наибольший ID страны.

**Реализация:**
```csharp
public int GetCountryIdOfChampionWithoutCups()
{
    // Клубы с золотыми медалями, но без кубков
    var clubsWithoutCups = from achievement in _context.Achievements
                           where achievement.З > 0 && achievement.К == 0
                           join club in _context.Clubs on achievement.ClubId equals club.Id
                           group new { achievement, club } by club.Id into g
                           select new
                           {
                               ClubId = g.Key,
                               Club = g.First().club,
                               GoldMedals = g.Sum(x => x.achievement.З)
                           };
    
    // Находим максимальное количество медалей
    var maxGold = clubsWithoutCups.Max(c => c.GoldMedals);
    
    // Все клубы с максимальным количеством медалей
    var topClubs = clubsWithoutCups.Where(c => c.GoldMedals == maxGold);
    
    // Возвращаем максимальный ID страны среди них
    return topClubs.Max(c => c.Club.CountryId);
}
```

**Логика:**
1. Фильтр: З > 0 и К == 0
2. Группировка по клубу для суммирования медалей
3. Поиск максимального значения
4. Выбор из нескольких вариантов по условию

---

## Основные классы и их назначение

### 1. Модели данных (Data/Models/)
Базовые классы, представляющие сущности:

```csharp
public class Country
{
    public int Id { get; set; }      // Первичный ключ
    public string Name { get; set; } // Название страны
}
```

### 2. DatabaseContext
Контейнер для хранения данных в памяти:

```csharp
public class DatabaseContext
{
    public List<Country> Countries { get; set; } = new();
    public List<Club> Clubs { get; set; } = new();
    public List<Achievement> Achievements { get; set; } = new();
}
```

### 3. ExcelRepository
Отвечает за чтение/запись Excel-файлов:

```csharp
public DatabaseContext LoadData(string filePath)
{
    // 1. Открытие файла
    // 2. Чтение каждого листа
    // 3. Преобразование строк в объекты
    // 4. Заполнение контекста
}
```

### 4. QueryService
Содержит все LINQ-запросы. Каждый метод - отдельный запрос с чёткой целью.

### 5. ConsoleMenu
Управление пользовательским интерфейсом:

```csharp
public static void ShowMainMenu()
{
    Console.Clear();
    Console.WriteLine("=== СИСТЕМА УПРАВЛЕНИЯ ===");
    Console.WriteLine("1. Загрузить данные");
    Console.WriteLine("2. Выполнить запросы");
    // ... и т.д.
}
```

---

## Примеры работы программы

### Загрузка данных
```
=== СИСТЕМА УПРАВЛЕНИЯ ФУТБОЛЬНЫМИ КЛУБАМИ ===
1. Загрузить данные из Excel
2. Просмотреть все данные
3. Управление данными (CRUD)
4. Выполнить запросы
5. Сохранить изменения в Excel
6. Тестовые данные (демо)
0. Выход

Выберите пункт: 1
Введите путь к файлу [LR5-var5.xls]: 

✓ Данные успешно загружены из LR5-var5.xls
Загружено: 55 стран, 210 клубов, 850 достижений
```

### Выполнение запроса
<img width="1096" height="284" alt="image" src="https://github.com/user-attachments/assets/5c5a5105-0d34-434d-94b1-08a187f6d5e5" />
<img width="1353" height="426" alt="image" src="https://github.com/user-attachments/assets/9c774f7e-e768-4dd1-82e3-3f78647fdcbc" />
<img width="422" height="450" alt="image" src="https://github.com/user-attachments/assets/f2b00150-81af-43a4-9888-b25e89d658a9" />
<img width="621" height="690" alt="image" src="https://github.com/user-attachments/assets/c2e03f1c-1934-427e-838b-69c13250db83" />
<img width="617" height="253" alt="image" src="https://github.com/user-attachments/assets/46411dd3-8f65-46f8-bd01-8ea81d511354" />
<img width="622" height="881" alt="image" src="https://github.com/user-attachments/assets/01be5e8c-c2f6-40b5-b74b-29b41a15b5f7" />
<img width="629" height="274" alt="image" src="https://github.com/user-attachments/assets/56bc9b49-d460-4325-94b7-b681a31f28c9" />

---

## Заключение

### Что было сделано:
1.  Реализовано чтение данных из Excel-файла с помощью библиотеки NPOI
2.  Создана объектная модель данных (Country, Club, Achievement)
3.  Реализованы 4 LINQ-запроса разной сложности:
   - Простой запрос к одной таблице (сортировка)
   - Запрос с соединением двух таблиц (JOIN)
   - Сложный запрос к трём таблицам с группировкой
   - Запрос к трём таблицам с фильтрацией и сортировкой
4.  Реализован дополнительный запрос по условию варианта
5.  Создан дружественный консольный интерфейс с меню
6.  Реализованы CRUD-операции для управления данными
7.  Добавлена обработка ошибок на всех этапах работы

### Что было изучено:
- **LINQ (Language Integrated Query)** - технология запросов в C#
- **Методы расширения LINQ**: `Where()`, `Select()`, `OrderBy()`, `Join()`, `GroupBy()`, `Sum()`
- **Работа с внешними данными**: чтение/запись Excel-файлов
- **Архитектура приложений**: разделение на слои (Data, Services, UI)
- **Обработка исключений** при работе с файлами и данными

### Трудности и их решение:
1. **Проблема**: Разные форматы данных в Excel (числа, строки, пустые ячейки)
   **Решение**: Добавлена проверка типа ячейки и значения по умолчанию

2. **Проблема**: Сложные JOIN-запросы с несколькими условиями
   **Решение**: Поэтапная разработка запросов, от простого к сложному

3. **Проблема**: Обработка больших объёмов данных в памяти
   **Решение**: Использование ленивой загрузки (IEnumerable) где возможно

### Особенности реализации:
- Чистая архитектура с чётким разделением ответственности
- Использование интерфейсов для возможности тестирования
- Юзер-френдли интерфейс с подсказками
