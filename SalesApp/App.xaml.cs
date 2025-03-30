using SalesApp.Pages;
using SalesApp.Services;

namespace SalesApp;

public partial class App : Application
{
    private readonly AuthService _authService;

    public App(AuthService authService)
    {
        _authService = authService;
        InitializeComponent();
        SetupMainPage();
    }

    private void SetupMainPage()
    {
        var isAuthenticated = _authService.IsAuthenticated();
        MainPage = isAuthenticated 
            ? new AppShell() 
            : new NavigationPage(new LoginPage());
    }

    protected override void OnStart()
    {
        base.OnStart();
        // Additional startup logic
    }
}
