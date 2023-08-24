using Avalonia.Collections;
using Avalonia.Controls;
using DesktopClient.Models.ListBox;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DesktopClient.Views
{
    internal sealed partial class AvaliableUsersView : UserControl
    {
        public ObservableCollection<ListBoxItemBase> Items => new ObservableCollection<ListBoxItemBase>()
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
