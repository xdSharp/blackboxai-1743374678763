namespace SalesApp;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        
        // Register routes
        Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
        Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage));
        Routing.RegisterRoute(nameof(SalesEntryPage), typeof(SalesEntryPage));
    }
}
