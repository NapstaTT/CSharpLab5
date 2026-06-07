///FIXME:
/// - Удалены неиспользуемые using
/// - Приведены скобки к стилю Allman (Allman Style)
/// - Переименованы локальные переменные в camelCase
/// - Добавлена XML-документация ко всем публичным методам
/// - Логические блоки разделены пустыми строками
/// - Упрощена структура меню
/// - Guard-clauses для проверки загруженности данных
/// - Улучшена читаемость и структура файла

using System;
using System.IO;
using lab5.Data;
using lab5.Data.Repos;
using lab5.Services;
using lab5.Services.Queries;
using lab5.UI;

namespace lab5
{
    /// <summary>
    /// Главный класс приложения. Управляет запуском, меню и обработкой пользовательских действий.
    /// </summary>
    internal class Program
    {
        private static DatabaseContext _context = new DatabaseContext();
        private static IExcelRepository _excelRepository;
        private static IDatabaseService _dbService;
        private static IQueryService _queryService;

        /// <summary>
        /// Точка входа в приложение.
        /// </summary>
        public static void Main(string[] args)
        {
            Console.Title = "Футбольные клубы Европы - LINQ Запросы";
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            InitializeServices();

            var exit = false;

            while (!exit)
            {
                try
                {
                    ConsoleMenu.ShowMainMenu();
                    var choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1":
                            LoadData();
                            break;

                        case "2":
                            ViewAllData();
                            break;

                        case "3":
                            ManageData();
                            break;

                        case "4":
                            ExecuteQueries();
                            break;

                        case "5":
                            SaveData();
                            break;

                        case "6":
                            AddDemoData();
                            break;

                        case "0":
                            exit = true;
                            break;

                        default:
                            Console.WriteLine("Неверный пункт меню!");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    ConsoleHelper.ShowError(ex.Message);
                    ConsoleHelper.PressAnyKey();
                }
            }

            Console.WriteLine("\nПрограмма завершена. Нажмите любую клавишу...");
            Console.ReadKey();
        }

        /// <summary>
        /// Инициализирует сервисы, работающие с данными.
        /// </summary>
        private static void InitializeServices()
        {
            _excelRepository = new ExcelRepository();
            _dbService = new DatabaseService(_context);
            _queryService = new QueryService(_context);
        }

        /// <summary>
        /// Загружает данные из Excel-файла.
        /// </summary>
        private static void LoadData()
        {
            ConsoleHelper.ShowTitle("Загрузка данных из Excel");

            var defaultPath = Path.Combine(Directory.GetCurrentDirectory(), "LR5-var5.xls");
            var path = ConsoleHelper.ReadString("Введите путь к файлу", defaultPath);

            if (!File.Exists(path))
            {
                ConsoleHelper.ShowError($"Файл не найден: {path}");
                ConsoleHelper.ShowWarning("Поместите файл LR5-var5.xls в папку с программой");
                ConsoleHelper.PressAnyKey();
                return;
            }

            try
            {
                _context = _excelRepository.LoadData(path);
                InitializeServices();

                ConsoleHelper.ShowSuccess($"Данные успешно загружены из {Path.GetFileName(path)}");
                Console.WriteLine($"Загружено: {_context.Countries.Count} стран, {_context.Clubs.Count} клубов, {_context.Achievements.Count} достижений");
            }
            catch (Exception ex)
            {
                ConsoleHelper.ShowError($"Ошибка загрузки: {ex.Message}");
            }

            ConsoleHelper.PressAnyKey();
        }

        /// <summary>
        /// Отображает все данные в консоли.
        /// </summary>
        private static void ViewAllData()
        {
            ConsoleHelper.ShowTitle("Просмотр всех данных");

            if (_context.Countries.Count == 0)
            {
                ConsoleHelper.ShowWarning("Данные не загружены! Сначала загрузите данные из Excel.");
                ConsoleHelper.PressAnyKey();
                return;
            }

            _dbService.ViewAll();
            ConsoleHelper.PressAnyKey();
        }

        /// <summary>
        /// Управление данными (CRUD).
        /// </summary>
        private static void ManageData()
        {
            if (_context.Countries.Count == 0)
            {
                ConsoleHelper.ShowWarning("Сначала загрузите данные из Excel!");
                ConsoleHelper.PressAnyKey();
                return;
            }

            var back = false;

            while (!back)
            {
                ConsoleMenu.ShowCrudMenu();
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddCountry();
                        break;

                    case "2":
                        AddClub();
                        break;

                    case "3":
                        AddAchievement();
                        break;

                    case "4":
                        RemoveCountry();
                        break;

                    case "5":
                        RemoveClub();
                        break;

                    case "6":
                        RemoveAchievement();
                        break;

                    case "7":
                        FindCountry();
                        break;

                    case "8":
                        FindClub();
                        break;

                    case "9":
                        FindAchievement();
                        break;

                    case "0":
                        back = true;
                        break;

                    default:
                        Console.WriteLine("Неверный пункт!");
                        break;
                }

                if (choice != "0")
                {
                    ConsoleHelper.PressAnyKey();
                }
            }
        }

        /// <summary>
        /// Добавляет новую страну.
        /// </summary>
        private static void AddCountry()
        {
            ConsoleHelper.ShowTitle("Добавление новой страны");

            var id = ConsoleHelper.ReadInt("Введите ID страны");
            var name = ConsoleHelper.ReadString("Введите название страны");

            var country = new Data.Models.Country
            {
                Id = id,
                Name = name
            };

            _dbService.AddCountry(country);
        }

        /// <summary>
        /// Добавляет новый клуб.
        /// </summary>
        private static void AddClub()
        {
            ConsoleHelper.ShowTitle("Добавление нового клуба");

            var id = ConsoleHelper.ReadInt("Введите ID клуба");
            var name = ConsoleHelper.ReadString("Введите название клуба");
            var countryId = ConsoleHelper.ReadInt("Введите ID страны");

            var club = new Data.Models.Club
            {
                Id = id,
                Name = name,
                CountryId = countryId
            };

            _dbService.AddClub(club);
        }

        /// <summary>
        /// Добавляет новое достижение.
        /// </summary>
        private static void AddAchievement()
        {
            ConsoleHelper.ShowTitle("Добавление достижения");

            var id = ConsoleHelper.ReadInt("Введите ID достижения");
            var clubId = ConsoleHelper.ReadInt("Введите ID клуба");
            var gold = ConsoleHelper.ReadInt("Количество золотых медалей", 0);
            var silver = ConsoleHelper.ReadInt("Количество серебряных медалей", 0);
            var bronze = ConsoleHelper.ReadInt("Количество бронзовых медалей", 0);
            var cups = ConsoleHelper.ReadInt("Количество выигранных кубков", 0);

            var achievement = new Data.Models.Achievement
            {
                Id = id,
                ClubId = clubId,
                G = gold,
                S = silver,
                B = bronze,
                C = cups
            };

            _dbService.AddAchievement(achievement);
        }

        private static void RemoveCountry() => RemoveItem("страну", id => _dbService.RemoveCountry(id));
        private static void RemoveClub() => RemoveItem("клуб", id => _dbService.RemoveClub(id));
        private static void RemoveAchievement() => RemoveItem("достижение", id => _dbService.RemoveAchievement(id));

        /// <summary>
        /// Универсальный метод удаления сущности.
        /// </summary>
        private static void RemoveItem(string itemName, Action<int> removeAction)
        {
            ConsoleHelper.ShowTitle($"Удаление: {itemName}");

            var id = ConsoleHelper.ReadInt($"Введите ID {itemName} для удаления");

            if (ConsoleHelper.ReadYesNo($"Вы уверены, что хотите удалить {itemName} с ID {id}?"))
            {
                removeAction(id);
                ConsoleHelper.ShowSuccess($"{itemName} удалено");
            }
            else
            {
                Console.WriteLine("Удаление отменено");
            }
        }

        private static void FindCountry()
        {
            ConsoleHelper.ShowTitle("Поиск страны");

            var id = ConsoleHelper.ReadInt("Введите ID страны");
            var country = _dbService.GetCountryById(id);

            Console.WriteLine(country != null
                ? $"Найдена: {country}"
                : $"Страна с ID {id} не найдена");
        }

        private static void FindClub()
        {
            ConsoleHelper.ShowTitle("Поиск клуба");

            var id = ConsoleHelper.ReadInt("Введите ID клуба");
            var club = _dbService.GetClubById(id);

            Console.WriteLine(club != null
                ? $"Найден: {club}"
                : $"Клуб с ID {id} не найден");
        }

        private static void FindAchievement()
        {
            ConsoleHelper.ShowTitle("Поиск достижения");

            var id = ConsoleHelper.ReadInt("Введите ID достижения");
            var achievement = _dbService.GetAchievementById(id);

            Console.WriteLine(achievement != null
                ? $"Найдено: {achievement}"
                : $"Достижение с ID {id} не найдено");
        }

        /// <summary>
        /// Выполняет LINQ-запросы.
        /// </summary>
        private static void ExecuteQueries()
        {
            if (_context.Countries.Count == 0)
            {
                ConsoleHelper.ShowWarning("Сначала загрузите данные из Excel!");
                ConsoleHelper.PressAnyKey();
                return;
            }

            var back = false;

            while (!back)
            {
                ConsoleMenu.ShowQueriesMenu();
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ExecuteQuery("Страны по алфавиту", () =>
                        {
                            var countries = _queryService.GetCountriesSortedByName();
                            countries.ForEach(c => Console.WriteLine(c));
                        });
                        break;

                    case "2":
                        ExecuteQuery("Клубы с названиями стран", () =>
                        {
                            var results = _queryService.GetClubsWithCountryNames();
                            foreach (dynamic item in results)
                            {
                                Console.WriteLine($"{item.ClubName} - {item.CountryName}");
                            }
                        });
                        break;

                    case "3":
                        ExecuteQuery("Страна с наибольшим количеством золотых медалей", () =>
                        {
                            Console.WriteLine(_queryService.GetCountryWithMostGoldMedals());
                        });
                        break;

                    case "4":
                        ExecuteQuery("Клубы с золотыми медалями, но без кубков", () =>
                        {
                            var clubs = _queryService.GetClubsWithGoldMedalsButNoCups();
                            clubs.ForEach(Console.WriteLine);
                        });
                        break;

                    case "5":
                        ExecuteQuery("Пример из задания", () =>
                        {
                            var countryId = _queryService.GetCountryIdOfChampionWithoutCups();
                            var country = _dbService.GetCountryById(countryId);

                            Console.WriteLine($"ID страны: {countryId}");

                            if (country != null)
                            {
                                Console.WriteLine($"Название страны: {country.Name}");
                            }
                        });
                        break;

                    case "0":
                        back = true;
                        break;

                    default:
                        Console.WriteLine("Неверный пункт!");
                        break;
                }

                if (choice != "0")
                {
                    ConsoleHelper.PressAnyKey();
                }
            }
        }

        /// <summary>
        /// Обёртка для выполнения запроса с выводом заголовка.
        /// </summary>
        private static void ExecuteQuery(string queryName, Action queryAction)
        {
            ConsoleHelper.ShowTitle(queryName);
            Console.WriteLine($"Выполнение запроса: {queryName}");
            Console.WriteLine(new string('-', 50));

            try
            {
                queryAction();
            }
            catch (Exception ex)
            {
                ConsoleHelper.ShowError($"Ошибка выполнения запроса: {ex.Message}");
            }
        }

        /// <summary>
        /// Сохраняет данные в Excel-файл.
        /// </summary>
        private static void SaveData()
        {
            ConsoleMenu.ShowSaveMenu();
            var choice = Console.ReadLine();

            string path;

            switch (choice)
            {
                case "1":
                    path = Path.Combine(Directory.GetCurrentDirectory(), "LR5-var5-modified.xls");
                    break;

                case "2":
                    Console.Write("Введите имя нового файла: ");
                    var fileName = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(fileName))
                    {
                        fileName = "LR5-var5-modified.xls";
                    }

                    path = Path.Combine(Directory.GetCurrentDirectory(), fileName);
                    break;

                case "0":
                    return;

                default:
                    Console.WriteLine("Неверный пункт!");
                    ConsoleHelper.PressAnyKey();
                    return;
            }

            try
            {
                _excelRepository.SaveData(path, _context);
                ConsoleHelper.ShowSuccess($"Данные сохранены в файл: {Path.GetFileName(path)}");
            }
            catch (Exception ex)
            {
                ConsoleHelper.ShowError($"Ошибка сохранения: {ex.Message}");
            }

            ConsoleHelper.PressAnyKey();
        }

        /// <summary>
        /// Добавляет тестовые данные.
        /// </summary>
        private static void AddDemoData()
        {
            ConsoleHelper.ShowTitle("Добавление тестовых данных");

            var testCountries = new[]
            {
                new Data.Models.Country { Id = 999, Name = "Тестовая страна 1" },
                new Data.Models.Country { Id = 998, Name = "Тестовая страна 2" }
            };

            foreach (var country in testCountries)
            {
                if (!_dbService.CountryExists(country.Id))
                {
                    _context.Countries.Add(country);
                    Console.WriteLine($"Добавлена тестовая страна: {country.Name}");
                }
            }

            var testClub = new Data.Models.Club
            {
                Id = 999,
                Name = "Тестовый клуб",
                CountryId = 999
            };

            if (!_dbService.ClubExists(testClub.Id))
            {
                _context.Clubs.Add(testClub);
                Console.WriteLine($"Добавлен тестовый клуб: {testClub.Name}");
            }

            ConsoleHelper.ShowSuccess("Тестовые данные добавлены. Можно тестировать запросы.");
            ConsoleHelper.PressAnyKey();
        }
    }
}
