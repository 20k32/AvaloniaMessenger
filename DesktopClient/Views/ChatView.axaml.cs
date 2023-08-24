using Avalonia.Controls;
using DesktopClient.Models.Messages;
using System.Collections.ObjectModel;

namespace DesktopClient.Views
{
    public sealed partial class ChatView : UserControl
    {
        internal ObservableCollection<Message> Messages => new()
        {
            new("Hi", true),
            new("hello", false),
            new("how are you?", true),
            new("i am fine", false)
        };

        public ChatView()
        {
            InitializeComponent();
        }
    }
}
