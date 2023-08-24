using Avalonia.Controls;
using DesktopClient.Models.ListBox;
using System.Collections.ObjectModel;

namespace DesktopClient.Views
{
    public sealed partial class AvaliableUsersView : UserControl
    {
        internal ObservableCollection<ListBoxItemBase> Items => new()
            {
               new ListBoxItemCategory("Пользователи в подписках:"),
               new ListBoxItemUser("Друг 1"),
               new ListBoxItemCategory("Пользователи глобально:"),
               new ListBoxItemUser("Пользователь 1"),
               new ListBoxItemCategory("Найденные сообщения"),
               new ListBoxItemMessage("Сообщение 1")
            };

        public AvaliableUsersView()
        {
            InitializeComponent();
        }
    }
}
