using lab5.Data;
using lab5.Data.Repos;
using lab5.Services;
using lab5.Services.Queries;
using lab5.UI;
using System;
using System.IO;

namespace lab5
{
    class Program
    {
        private static DatabaseContext _context = new DatabaseContext();
        private static IExcelRepository _excelRepository;
        private static IDatabaseService _dbService;
        private static IQueryService _queryService;
        
        static void Main(string[] args)
        {
            Console.Title = "Футбольные клубы Европы - LINQ Запросы";
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            
            InitializeServices();
            
            bool exit = false;
            while (!exit)
            {
                try
                {
                    ConsoleMenu.ShowMainMenu();
                    string choice = Console.ReadLine();
                    
                    switch (choice)
                    {
                        case "1": LoadData(); break;
                        case "2": ViewAllData(); break;
                        case "3": ManageData(); break;
                        case "4": ExecuteQueries(); break;
                        case "5": SaveData(); break;
                        case "6": AddDemoData(); break;
                        case "0": exit = true; break;
                        default: Console.WriteLine("Неверный пункт меню!"); break;
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
        
        static void InitializeServices()
        {
            _excelRepository = new ExcelRepository();
            _dbService = new DatabaseService(_context);
            _queryService = new QueryService(_context);
        }
        
        static void LoadData()
        {
            ConsoleHelper.ShowTitle("Загрузка данных из Excel");
            
            string defaultPath = Path.Combine(Directory.GetCurrentDirectory(), "LR5-var5.xls");
            string path = ConsoleHelper.ReadString("Введите путь к файлу", defaultPath);
            
            if (!File.Exists(path))
            {
                ConsoleHelper.ShowError($"Файл не найден: {path}");
                ConsoleHelper.ShowWarning("Попробуйте поместить файл LR5-var5.xls в папку с программой");
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
        
        static void ViewAllData()
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
        
        static void ManageData()
        {
            if (_context.Countries.Count == 0)
            {
                ConsoleHelper.ShowWarning("Сначала загрузите данные из Excel!");
                ConsoleHelper.PressAnyKey();
                return;
            }
            
            bool back = false;
            while (!back)
            {
                ConsoleMenu.ShowCrudMenu();
                string choice = Console.ReadLine();
                
                switch (choice)
                {
                    case "1": AddCountry(); break;
                    case "2": AddClub(); break;
                    case "3": AddAchievement(); break;
                    case "4": RemoveCountry(); break;
                    case "5": RemoveClub(); break;
                    case "6": RemoveAchievement(); break;
                    case "7": FindCountry(); break;
                    case "8": FindClub(); break;
                    case "9": FindAchievement(); break;
                    case "0": back = true; break;
                    default: Console.WriteLine("Неверный пункт!"); break;
                }
                
                if (choice != "0")
                    ConsoleHelper.PressAnyKey();
            }
        }
        
        static void AddCountry()
        {
            ConsoleHelper.ShowTitle("Добавление новой страны");
            
            int id = ConsoleHelper.ReadInt("Введите ID страны");
            string name = ConsoleHelper.ReadString("Введите название страны");
            
            var country = new Data.Models.Country { Id = id, Name = name };
            _dbService.AddCountry(country);
        }
        
        static void AddClub()
        {
            ConsoleHelper.ShowTitle("Добавление нового клуба");
            
            int id = ConsoleHelper.ReadInt("Введите ID клуба");
            string name = ConsoleHelper.ReadString("Введите название клуба");
            int countryId = ConsoleHelper.ReadInt("Введите ID страны");
            
            var club = new Data.Models.Club { Id = id, Name = name, CountryId = countryId };
            _dbService.AddClub(club);
        }
        
        static void AddAchievement()
        {
            ConsoleHelper.ShowTitle("Добавление достижения");
            
            int id = ConsoleHelper.ReadInt("Введите ID достижения");
            int clubId = ConsoleHelper.ReadInt("Введите ID клуба");
            int gold = ConsoleHelper.ReadInt("Количество золотых медалей", 0);
            int silver = ConsoleHelper.ReadInt("Количество серебряных медалей", 0);
            int bronze = ConsoleHelper.ReadInt("Количество бронзовых медалей", 0);
            int cups = ConsoleHelper.ReadInt("Количество выигранных кубков", 0);
            
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
        
        static void RemoveCountry() => RemoveItem("страну", id => _dbService.RemoveCountry(id));
        static void RemoveClub() => RemoveItem("клуб", id => _dbService.RemoveClub(id));
        static void RemoveAchievement() => RemoveItem("достижение", id => _dbService.RemoveAchievement(id));
        
        static void RemoveItem(string itemName, Action<int> removeAction)
        {
            ConsoleHelper.ShowTitle($"Удаление {itemName}");
            int id = ConsoleHelper.ReadInt($"Введите ID {itemName} для удаления");
            
            if (ConsoleHelper.ReadYesNo($"Вы уверены, что хотите удалить {itemName} с ID {id}?"))
            {
                removeAction(id);
                ConsoleHelper.ShowSuccess($"{itemName} удален(о)");
            }
            else
            {
                Console.WriteLine("Удаление отменено");
            }
        }
        
        static void FindCountry()
        {
            ConsoleHelper.ShowTitle("Поиск страны");
            int id = ConsoleHelper.ReadInt("Введите ID страны");
            var country = _dbService.GetCountryById(id);
            
            if (country != null)
                Console.WriteLine($"Найдена: {country}");
            else
                Console.WriteLine($"Страна с ID {id} не найдена");
        }
        
        static void FindClub()
        {
            ConsoleHelper.ShowTitle("Поиск клуба");
            int id = ConsoleHelper.ReadInt("Введите ID клуба");
            var club = _dbService.GetClubById(id);
            
            if (club != null)
                Console.WriteLine($"Найден: {club}");
            else
                Console.WriteLine($"Клуб с ID {id} не найден");
        }
        
        static void FindAchievement()
        {
            ConsoleHelper.ShowTitle("Поиск достижения");
            int id = ConsoleHelper.ReadInt("Введите ID достижения");
            var achievement = _dbService.GetAchievementById(id);
            
            if (achievement != null)
                Console.WriteLine($"Найдено: {achievement}");
            else
                Console.WriteLine($"Достижение с ID {id} не найдено");
        }
        
        static void ExecuteQueries()
        {
            if (_context.Countries.Count == 0)
            {
                ConsoleHelper.ShowWarning("Сначала загрузите данные из Excel!");
                ConsoleHelper.PressAnyKey();
                return;
            }
            
            bool back = false;
            while (!back)
            {
                ConsoleMenu.ShowQueriesMenu();
                string choice = Console.ReadLine();
                
                switch (choice)
                {
                    case "1":
                        ExecuteQuery("Страны по алфавиту", 
                            () => _queryService.GetCountriesSortedByName().ForEach(c => Console.WriteLine(c)));
                        break;
                    case "2":
                        ExecuteQuery("Клубы с названиями стран", () =>
                        {
                            var results = _queryService.GetClubsWithCountryNames();
                            foreach (dynamic item in results)
                                Console.WriteLine($"{item.ClubName} - {item.CountryName}");
                        });
                        break;
                    case "3":
                        ExecuteQuery("Страна с наибольшим количеством золотых медалей", 
                            () => Console.WriteLine(_queryService.GetCountryWithMostGoldMedals()));
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
                            int countryId = _queryService.GetCountryIdOfChampionWithoutCups();
                            var country = _dbService.GetCountryById(countryId);
                            Console.WriteLine($"ID страны: {countryId}");
                            if (country != null)
                                Console.WriteLine($"Название страны: {country.Name}");
                        });
                        break;
                    case "0": back = true; break;
                    default: Console.WriteLine("Неверный пункт!"); break;
                }
                
                if (choice != "0" && choice != null)
                    ConsoleHelper.PressAnyKey();
            }
        }
        
        static void ExecuteQuery(string queryName, Action queryAction)
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
        
        static void SaveData()
        {
            ConsoleMenu.ShowSaveMenu();
            string choice = Console.ReadLine();
            
            string path;
            switch (choice)
            {
                case "1":
                    path = Path.Combine(Directory.GetCurrentDirectory(), "LR5-var5-modified.xls");
                    break;
                case "2":
                    Console.Write("Введите имя нового файла: ");
                    string fileName = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(fileName))
                        fileName = "LR5-var5-modified.xls";
                    path = Path.Combine(Directory.GetCurrentDirectory(), fileName);
                    break;
                case "0": return;
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
        
        static void AddDemoData()
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