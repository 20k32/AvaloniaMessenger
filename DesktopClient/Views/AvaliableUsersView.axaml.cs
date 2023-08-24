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
