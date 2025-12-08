using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.IO;
using lab5.Data;
using lab5.Data.Models;

namespace lab5.Data.Repos
{
    public class ExcelRepository : IExcelRepository
    {
        public DatabaseContext LoadData(string filePath)
        {
            var context = new DatabaseContext();
            
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"Файл {filePath} не найден");
            
            using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                IWorkbook workbook = new HSSFWorkbook(fs);
                
                LoadCountries(workbook, context);
                LoadClubs(workbook, context);
                LoadAchievements(workbook, context);
            }
            
            return context;
        }
        
        private void LoadCountries(IWorkbook workbook, DatabaseContext context)
        {
            try
            {
                ISheet sheet = workbook.GetSheet("Страны") ?? workbook.GetSheetAt(0);
                if (sheet == null) return;
                
                for (int row = 1; row <= sheet.LastRowNum; row++)
                {
                    IRow currentRow = sheet.GetRow(row);
                    if (currentRow == null || currentRow.GetCell(0) == null) continue;
                    
                    context.Countries.Add(new Country
                    {
                        Id = (int)currentRow.GetCell(0).NumericCellValue,
                        Name = currentRow.GetCell(1)?.StringCellValue?.Trim() ?? $"Country_{row}"
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка загрузки стран: {ex.Message}");
            }
        }
        
        private void LoadClubs(IWorkbook workbook, DatabaseContext context)
        {
            try
            {
                ISheet sheet = workbook.GetSheet("Клубы") ?? workbook.GetSheetAt(1);
                if (sheet == null) return;
                
                for (int row = 1; row <= sheet.LastRowNum; row++)
                {
                    IRow currentRow = sheet.GetRow(row);
                    if (currentRow == null || currentRow.GetCell(0) == null) continue;
                    
                    context.Clubs.Add(new Club
                    {
                        Id = (int)currentRow.GetCell(0).NumericCellValue,
                        Name = currentRow.GetCell(1)?.StringCellValue?.Trim() ?? $"Club_{row}",
                        CountryId = (int)currentRow.GetCell(2).NumericCellValue
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка загрузки клубов: {ex.Message}");
            }
        }
        
        private void LoadAchievements(IWorkbook workbook, DatabaseContext context)
        {
            try
            {
                ISheet sheet = workbook.GetSheet("Достижения") ?? workbook.GetSheetAt(2);
                if (sheet == null) return;
                
                for (int row = 1; row <= sheet.LastRowNum; row++)
                {
                    IRow currentRow = sheet.GetRow(row);
                    if (currentRow == null || currentRow.GetCell(0) == null) continue;
                    
                    context.Achievements.Add(new Achievement
                    {
                        Id = (int)currentRow.GetCell(0).NumericCellValue,
                        ClubId = (int)currentRow.GetCell(1).NumericCellValue,
                        G = GetCellValue(currentRow.GetCell(2)),
                        S = GetCellValue(currentRow.GetCell(3)),
                        B = GetCellValue(currentRow.GetCell(4)),
                        C = GetCellValue(currentRow.GetCell(5)),
                        FC = GetCellValue(currentRow.GetCell(6)),
                        LC = GetCellValue(currentRow.GetCell(7)),
                        FLC = GetCellValue(currentRow.GetCell(8)),
                        LE = GetCellValue(currentRow.GetCell(9)),
                        FLE = GetCellValue(currentRow.GetCell(10)),
                        COC = GetCellValue(currentRow.GetCell(11)),
                        FCOC = GetCellValue(currentRow.GetCell(12)),
                        LK = GetCellValue(currentRow.GetCell(13)),
                        FLK = GetCellValue(currentRow.GetCell(14))
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка загрузки достижений: {ex.Message}");
            }
        }
        
        private int GetCellValue(ICell cell)
        {
            if (cell == null) return 0;
            if (cell.CellType == CellType.Numeric) return (int)cell.NumericCellValue;
            if (cell.CellType == CellType.String && int.TryParse(cell.StringCellValue, out int result)) return result;
            return 0;
        }
        
        public void SaveData(string filePath, DatabaseContext context)
        {
            IWorkbook workbook = new HSSFWorkbook();
            
            SaveToSheet(workbook, "Страны", context.Countries);
            SaveToSheet(workbook, "Клубы", context.Clubs);
            SaveToSheet(workbook, "Достижения", context.Achievements);
            
            using (var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                workbook.Write(fs);
            }
        }
        
        private void SaveToSheet<T>(IWorkbook workbook, string sheetName, List<T> items)
        {
            ISheet sheet = workbook.CreateSheet(sheetName);
            
            if (items.Count > 0)
            {
                var properties = typeof(T).GetProperties();
                IRow headerRow = sheet.CreateRow(0);
                
                for (int i = 0; i < properties.Length; i++)
                {
                    headerRow.CreateCell(i).SetCellValue(properties[i].Name);
                }
                
                for (int rowIdx = 0; rowIdx < items.Count; rowIdx++)
                {
                    IRow row = sheet.CreateRow(rowIdx + 1);
                    var item = items[rowIdx];
                    
                    for (int colIdx = 0; colIdx < properties.Length; colIdx++)
                    {
                        var value = properties[colIdx].GetValue(item);
                        if (value != null)
                        {
                            if (value is int intValue)
                                row.CreateCell(colIdx).SetCellValue(intValue);
                            else if (value is double doubleValue)
                                row.CreateCell(colIdx).SetCellValue(doubleValue);
                            else
                                row.CreateCell(colIdx).SetCellValue(value.ToString());
                        }
                    }
                }
            }
        }
    }
}