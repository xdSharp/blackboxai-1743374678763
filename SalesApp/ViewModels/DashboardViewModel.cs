using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using SalesApp.Models;
using SalesApp.Services;

namespace SalesApp.ViewModels;

public class DashboardViewModel : INotifyPropertyChanged
{
    private readonly DataService _dataService;
    private DateTime _selectedDate = DateTime.Today;
    
    public ObservableCollection<SalesData> RecentSales { get; } = new();
    public decimal TotalRevenue { get; private set; }
    public int TotalTransactions { get; private set; }
    
    public DateTime SelectedDate
    {
        get => _selectedDate;
        set
        {
            _selectedDate = value;
            OnPropertyChanged();
            LoadDashboardData();
        }
    }

    public ICommand RefreshCommand { get; }
    public ICommand NavigateToSalesEntryCommand { get; }

    public DashboardViewModel(DataService dataService)
    {
        _dataService = dataService;
        RefreshCommand = new Command(async () => await LoadDashboardData());
        NavigateToSalesEntryCommand = new Command(NavigateToSalesEntry);
    }

    public async Task LoadDashboardData()
    {
        var sales = await _dataService.GetSalesDataByPeriodAsync(
            SelectedDate.AddDays(-7), 
            SelectedDate);
            
        RecentSales.Clear();
        foreach (var sale in sales.OrderByDescending(s => s.Date))
        {
            RecentSales.Add(sale);
        }

        TotalRevenue = sales.Sum(s => s.Amount);
        TotalTransactions = sales.Count;
        
        OnPropertyChanged(nameof(TotalRevenue));
        OnPropertyChanged(nameof(TotalTransactions));
    }

    private void NavigateToSalesEntry() => 
        Shell.Current.GoToAsync(nameof(SalesEntryPage));

    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}