using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using DesktopClient.Models.Auth;
using Shared.Databases.DTOs;

namespace DesktopClient.Views;

public partial class LoginWindow : Window
{
    private AuthorizationControllerAccessor _accessor;
    public LoginWindow()
    {
        InitializeComponent();
    }

    public LoginWindow(AuthorizationControllerAccessor accessor) : this()
    {
        _accessor = accessor;
    }

    private async void SignInButton_OnClick(object? sender, RoutedEventArgs e)
    {
        ErrorTextBlock.IsVisible = false;
        
        var fullName = FullNameTextBox.Text;
        var username = UserNameTextBox.Text;
        var password = PasswordTextBox.Text;
        
        bool validationResult = ValidateCredential(fullName)
                                && ValidateCredential(username)
                                && ValidateCredential(password);

        if (!validationResult)
        {
            Close(null!); 
        }

        if (!(username!.StartsWith("@")))
        { 
            if (username.Length == 22)
            {
                username = $"@{username[1..]}";
            }
            else
            {
                username = $"@{username}";
            }
        }

        var probablyNewUser = new UsersDbUserEntry(username!, password!, fullName!);
        
        try
        {
            var user = await _accessor.RequestAuthFor(probablyNewUser);
            if (user is not null)
            {
                Close(user);
            }
            else
            {
                ErrorTextBlock.IsVisible = true;
            }
        }
        catch
        { }
        
    }

    private bool ValidateCredential(string? credential)
    {
        if (string.IsNullOrEmpty(credential))
        {
            return false;
        }

        if (credential.Length > 22
            || credential.Length < 3)
        {
            return false;
        }

        return true;
    }
}