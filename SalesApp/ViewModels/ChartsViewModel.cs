using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using CommunityToolkit.Maui.Core;
using SalesApp.Models;
using SalesApp.Services;

namespace SalesApp.ViewModels;

public class ChartsViewModel : INotifyPropertyChanged
{
    private readonly DataService _dataService;
    private DateTime _startDate = DateTime.Today.AddDays(-30);
    private DateTime _endDate = DateTime.Today;
    
    public ObservableCollection<ISeries> Series { get; } = new();
    public Axis[] XAxes { get; } = { new DateTimeAxis() };
    public string ChartTitle { get; private set; } = "Sales Performance";
    
    public DateTime StartDate
    {
        get => _startDate;
        set
        {
            _startDate = value;
            OnPropertyChanged();
            LoadChartData();
        }
    }

    public DateTime EndDate
    {
        get => _endDate;
        set
        {
            _endDate = value;
            OnPropertyChanged();
            LoadChartData();
        }
    }

    public ICommand RefreshCommand { get; }

    public ChartsViewModel(DataService dataService)
    {
        _dataService = dataService;
        RefreshCommand = new Command(async () => await LoadChartData());
    }

    public async Task LoadChartData()
    {
        var sales = await _dataService.GetSalesDataByPeriodAsync(StartDate, EndDate);
        var dailySales = sales
            .GroupBy(s => s.Date.Date)
            .Select(g => new { Date = g.Key, Amount = g.Sum(s => s.Amount) })
            .OrderBy(x => x.Date);

        Series.Clear();
        Series.Add(new LineSeries<double>
        {
            Values = dailySales.Select(x => (double)x.Amount).ToList(),
            Fill = new SolidColorPaint(new Color(78, 175, 80, 100)),
            Stroke = new SolidColorPaint(new Color(78, 175, 80), 3),
            PointGeometry = new CircleGeometry(5)
        });

        ChartTitle = $"Sales Performance ({StartDate:dd MMM} - {EndDate:dd MMM})";
        OnPropertyChanged(nameof(ChartTitle));
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}