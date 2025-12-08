namespace lab5.Data.Repos
{
    public interface IExcelRepository
    {
        DatabaseContext LoadData(string filePath);
        void SaveData(string filePath, DatabaseContext context);
    }
}