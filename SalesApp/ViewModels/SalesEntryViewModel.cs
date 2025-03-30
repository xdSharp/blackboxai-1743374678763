using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using SalesApp.Models;
using SalesApp.Services;

namespace SalesApp.ViewModels;

public class SalesEntryViewModel : INotifyPropertyChanged
{
    private readonly DataService _dataService;
    
    public SalesData NewSale { get; } = new()
    {
        Date = DateTime.Now,
        ProductName = string.Empty,
        Amount = 0,
        Quantity = 1
    };

    public ICommand SaveCommand { get; }
    public ICommand CancelCommand { get; }

    public SalesEntryViewModel(DataService dataService)
    {
        _dataService = dataService;
        SaveCommand = new Command(async () => await SaveSale());
        CancelCommand = new Command(Cancel);
    }

    private async Task SaveSale()
    {
        if (string.IsNullOrWhiteSpace(NewSale.ProductName) || NewSale.Amount <= 0)
        {
            await Shell.Current.DisplayAlert("Error", "Please enter valid product and amount", "OK");
            return;
        }

        await _dataService.SaveSalesDataAsync(NewSale);
        await Shell.Current.GoToAsync("..");
    }

    private void Cancel() => Shell.Current.GoToAsync("..");

    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}