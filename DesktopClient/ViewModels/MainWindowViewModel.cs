using System.Collections.ObjectModel;
using DesktopClient.Models.ListBox;

namespace DesktopClient.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public ObservableCollection<ListBoxItemBase> Items => new()
    {
        new ListBoxItemCategory("asfasdf:"),
        new ListBoxItemUser("Друг 1"),
        new ListBoxItemCategory("Пользователи глобально:"),
        new ListBoxItemUser("Пользователь 1"),
        new ListBoxItemCategory("Найденные сообщения"),
        new ListBoxItemMessage("Сообщение 1")
    };
    
    public string Greeting => "Welcome to Avalonia!";
}
