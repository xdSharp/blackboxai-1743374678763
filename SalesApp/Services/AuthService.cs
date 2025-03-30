using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace SalesApp.Services;

public class AuthService
{
    private const string UsersFile = "users.json";
    private readonly string _filePath;
    private List<User> _users = new();

    public bool IsAuthenticated { get; private set; }
    public string CurrentUser { get; private set; }

    public AuthService()
    {
        _filePath = Path.Combine(FileSystem.AppDataDirectory, UsersFile);
        LoadUsers();
    }

    public bool Register(string email, string password)
    {
        if (_users.Any(u => u.Email == email))
            return false;

        var user = new User
        {
            Email = email,
            PasswordHash = HashPassword(password)
        };

        _users.Add(user);
        SaveUsers();
        return true;
    }

    public bool Login(string email, string password)
    {
        var user = _users.FirstOrDefault(u => u.Email == email);
        if (user == null || user.PasswordHash != HashPassword(password))
            return false;

        IsAuthenticated = true;
        CurrentUser = email;
        return true;
    }

    public void Logout()
    {
        IsAuthenticated = false;
        CurrentUser = null;
    }

    private void LoadUsers()
    {
        if (!File.Exists(_filePath))
            return;

        var json = File.ReadAllText(_filePath);
        _users = JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();
    }

    private void SaveUsers()
    {
        var json = JsonSerializer.Serialize(_users);
        File.WriteAllText(_filePath, json);
    }

    private static string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(hashedBytes);
    }

    private class User
    {
        public string Email { get; set; }
        public string PasswordHash { get; set; }
    }
}