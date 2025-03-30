using System.Text.Json;
using SalesApp.Models;

namespace SalesApp.Services;

public class DataService
{
    private const string SalesDataFile = "sales_data.json";
    private readonly string _dataPath;

    public DataService()
    {
        _dataPath = Path.Combine(FileSystem.AppDataDirectory, SalesDataFile);
    }

    public async Task SaveSalesDataAsync(SalesData data)
    {
        var allData = await GetAllSalesDataAsync();
        allData.Add(data);
        
        var json = JsonSerializer.Serialize(allData);
        await File.WriteAllTextAsync(_dataPath, json);
    }

    public async Task<List<SalesData>> GetAllSalesDataAsync()
    {
        if (!File.Exists(_dataPath))
            return new List<SalesData>();

        var json = await File.ReadAllTextAsync(_dataPath);
        return JsonSerializer.Deserialize<List<SalesData>>(json) ?? new List<SalesData>();
    }

    public async Task<List<SalesData>> GetSalesDataByPeriodAsync(DateTime start, DateTime end)
    {
        var allData = await GetAllSalesDataAsync();
        return allData.Where(d => d.Date >= start && d.Date <= end).ToList();
    }
}