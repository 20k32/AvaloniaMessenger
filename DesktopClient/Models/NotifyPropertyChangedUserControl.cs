using System.ComponentModel;
using System.Runtime.CompilerServices;
using Avalonia.Controls;

namespace DesktopClient.Models;

public class NotifyPropertyChangedUserControl : UserControl, INotifyPropertyChanged
{
    public new event PropertyChangedEventHandler? PropertyChanged;
    public void OnPropertyChanged([CallerMemberName]string prop = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}