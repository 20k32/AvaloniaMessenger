using Avalonia.Controls;
using DesktopClient.Models.Messages;
using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Styling;
using Microsoft.CodeAnalysis.Operations;

namespace DesktopClient.Views
{
    public sealed partial class ChatView : UserControl
    {
        #region Messages

        private ObservableCollection<ChatMessage> _messages = null!;

        public static readonly DirectProperty<ChatView, ObservableCollection<ChatMessage>> MessagesProperty =
            AvaloniaProperty.RegisterDirect<ChatView, ObservableCollection<ChatMessage>>(
                nameof(Messages),
                getter => getter.Messages,
                (setter, value) => setter.Messages = value);

        public ObservableCollection<ChatMessage> Messages
        {
            get => _messages;
            set => SetAndRaise(MessagesProperty, ref _messages, value);
        }

        #endregion
        
        public ChatView()
        {
            InitializeComponent();
        }
    }
    
}
