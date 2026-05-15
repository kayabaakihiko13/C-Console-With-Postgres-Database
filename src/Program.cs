using System;
using Npgsql;
using CSharpDatabase;
using Users.Repositories;
using Users.Models;
using CSharpDatabase.Utils;

async Task Main(string[] args)
{
    Console.WriteLine("--- Aplikasi Database C# ---");
    bool isRunning =true;
    while (isRunning)
    {
        ShowMenu();
        string? choice = Console.ReadLine();
        switch (choice)
        {
             case "1":
                await RegisterUser();
                break;
            case "2":
                await LoginUser();
                break;
            case "3":
                isRunning = false;
                Console.WriteLine("Terima kasih telah menggunakan aplikasi!");
                break;
            default:
                Console.WriteLine("Pilihan tidak valid. Silakan coba lagi.\n");
                break;
        }
    }
}

void ShowMenu()
{
    Console.WriteLine("=== Menu ===");
    Console.WriteLine("1. Register");
    Console.WriteLine("2. Login");
    Console.WriteLine("3. Exit");
    Console.Write("Pilih: ");
}

async Task RegisterUser()
{
    Console.WriteLine("--- Register User ---");
    Console.Write("Masukkan firstname: ");
    string firstname = Console.ReadLine() ?? "";
    
    Console.Write("Masukkan lastname: ");
    string lastname = Console.ReadLine() ?? "";
    
    Console.Write("Masukkan username: ");
    string username = Console.ReadLine() ?? "";
    
    Console.Write("Masukkan email: ");
    string email = Console.ReadLine() ?? "";
    
    Console.Write("Masukkan password: ");
    string password = Console.ReadLine() ?? "";

    // logic flow
    if (string.IsNullOrWhiteSpace(firstname) || string.IsNullOrWhiteSpace(lastname) ||
        string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(email) ||
        string.IsNullOrWhiteSpace(password))
    {
        Console.WriteLine("Semua field harus diisi!\n");
        return;
    }
    
    try
    {
        var input = new RegisterInput(firstname, lastname, username, email, password);
        bool success = await AuthRepository.RegisterAsync(input);
        
        if (success)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Registrasi berhasil!\n");
            Console.ResetColor();
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Registrasi gagal!\n");
            Console.ResetColor();
        }
    }
    catch (Exception ex)
    {
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine($"Error: {ex.Message}\n");
        Console.ResetColor();
    }

}
async Task LoginUser()
{
    Console.WriteLine("\n--- Login ---");
    Console.Write("Masukkan username/email: ");
    string input = Console.ReadLine() ?? "";
    
    Console.Write("Masukkan password: ");
    string password = Console.ReadLine() ?? "";
    
    if (string.IsNullOrWhiteSpace(input) || string.IsNullOrWhiteSpace(password))
    {
        Console.WriteLine("Username/email dan password harus diisi!\n");
        return;
    }
    
    try
    {
        var user = await AuthRepository.LoginAsync(input, password);
        
        if (user != null)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n=== Login Berhasil! ===");
            Console.WriteLine($"ID: {user.UserId}");
            Console.WriteLine($"Nama: {user.Firstname} {user.Lastname}");
            Console.WriteLine($"Username: {user.Username}");
            Console.WriteLine($"Email: {user.Email}");
            Console.WriteLine($"Tanggal Daftar: {user.CreateAt}\n");
            Console.ResetColor();
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Username/email atau password salah!\n");
            Console.ResetColor();
        }
    }
    catch (Exception ex)
    {
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine($"Error: {ex.Message}\n");
        Console.ResetColor();
    }
}

await Main(args);
