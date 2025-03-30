using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using SalesApp.Services;
using SalesApp.Pages;

namespace SalesApp.ViewModels;

public class AuthViewModel : INotifyPropertyChanged
{
    private readonly AuthService _authService;
    
    public string Email { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
    
    public ICommand LoginCommand { get; }
    public ICommand RegisterCommand { get; }
    public ICommand NavigateToRegisterCommand { get; }
    public ICommand NavigateToLoginCommand { get; }

    public AuthViewModel(AuthService authService)
    {
        _authService = authService;
        
        LoginCommand = new Command(async () => await Login());
        RegisterCommand = new Command(async () => await Register());
        NavigateToRegisterCommand = new Command(NavigateToRegister);
        NavigateToLoginCommand = new Command(NavigateToLogin);
    }

    private async Task Login()
    {
        if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
        {
            await Shell.Current.DisplayAlert("Error", "Please enter both email and password", "OK");
            return;
        }

        if (_authService.Login(Email, Password))
        {
            await Shell.Current.GoToAsync("//Dashboard");
        }
        else
        {
            await Shell.Current.DisplayAlert("Error", "Invalid login attempt", "OK");
        }
    }

    private async Task Register()
    {
        if (Password != ConfirmPassword)
        {
            await Shell.Current.DisplayAlert("Error", "Passwords don't match", "OK");
            return;
        }

        if (_authService.Register(Email, Password))
        {
            await Shell.Current.DisplayAlert("Success", "Registration successful", "OK");
            await Shell.Current.GoToAsync("//Login");
        }
        else
        {
            await Shell.Current.DisplayAlert("Error", "Registration failed", "OK");
        }
    }

    private void NavigateToRegister() => Shell.Current.GoToAsync(nameof(RegisterPage));
    private void NavigateToLogin() => Shell.Current.GoToAsync(nameof(LoginPage));

    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}