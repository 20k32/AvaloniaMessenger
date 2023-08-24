using Avalonia.Controls;
using DesktopClient.Models.ListBox;
using System.Collections.ObjectModel;

namespace DesktopClient.Views
{
    public sealed partial class AvaliableUsersView : UserControl
    {
        internal ObservableCollection<ListBoxItemBase> Items => new()
            {
               new ListBoxItemCategory("������������ � ���������:"),
               new ListBoxItemUser("���� 1"),
               new ListBoxItemCategory("������������ ���������:"),
               new ListBoxItemUser("������������ 1"),
               new ListBoxItemCategory("��������� ���������"),
               new ListBoxItemMessage("��������� 1")
            };

        public AvaliableUsersView()
        {
            InitializeComponent();
        }
    }
}
