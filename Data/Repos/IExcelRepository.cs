///FIXME:
/// - Добавлена XML-документация
/// - Приведены скобки к стилю Allman
/// - Удалены лишние пустые строки

using lab5.Data;

namespace lab5.Data.Repos
{
    /// <summary>
    /// Определяет операции загрузки и сохранения данных Excel-файла.
    /// </summary>
    public interface IExcelRepository
    {
        /// <summary>
        /// Загружает данные из Excel-файла.
        /// </summary>
        /// <param name="filePath">Путь к Excel-файлу.</param>
        /// <returns>Контекст базы данных.</returns>
        DatabaseContext LoadData(string filePath);

        /// <summary>
        /// Сохраняет данные в Excel-файл.
        /// </summary>
        /// <param name="filePath">Путь для сохранения.</param>
        /// <param name="context">Контекст данных.</param>
        void SaveData(string filePath, DatabaseContext context);
    }
}
