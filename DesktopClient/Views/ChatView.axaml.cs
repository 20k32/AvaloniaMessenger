using Avalonia.Controls;
using DesktopClient.Models.Messages;
using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Data;
using Avalonia.Data.Core;
using Avalonia.LogicalTree;
using Avalonia.Markup.Xaml.MarkupExtensions;
using Avalonia.Markup.Xaml.MarkupExtensions.CompiledBindings;
using Avalonia.Styling;
using CommunityToolkit.Mvvm.Input;
using Microsoft.CodeAnalysis.Operations;

namespace DesktopClient.Views
{
    public sealed partial class ChatView : UserControl
    {
        private static Control FocusableElement = null!;
        private const string FOCUSABLE_ELEMENT_NAME = "FOCUSABLE_ELEMENT";
        
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

        #region SendMessageCommand

        private IRelayCommand<string> _sendMessgeCommand;

        public static readonly DirectProperty<ChatView, IRelayCommand<string>> SendMessageCommandProperty = 
            AvaloniaProperty.RegisterDirect<ChatView, IRelayCommand<string>>(
                nameof(SendMessageCommand),
                getter => getter.SendMessageCommand,
                (setter, value) => setter.SendMessageCommand = value);

        public IRelayCommand<string> SendMessageCommand
        {
            get => _sendMessgeCommand;
            set => SetAndRaise(SendMessageCommandProperty, ref _sendMessgeCommand, value);
        }

        #endregion

        #region SendMessageCommandParameter

        private string _sendMessageCommandParameter;

        public static readonly DirectProperty<ChatView, string> SendMessageCommandParameterProperty =
            AvaloniaProperty.RegisterDirect<ChatView, string>(
                nameof(SendMessageCommandParameter), 
                o => o.SendMessageCommandParameter, 
                (o, v) => o.SendMessageCommandParameter = v);

        public string SendMessageCommandParameter
        {
            get => _sendMessageCommandParameter;
            set => SetAndRaise(SendMessageCommandParameterProperty, ref _sendMessageCommandParameter, value);
        }

        #endregion
        
        #region MessageText

        private string _messageText;

        public static readonly DirectProperty<ChatView, string> MessageTextProperty =
            AvaloniaProperty.RegisterDirect<ChatView, string>(
                nameof(MessageText),
                getter => getter.MessageText, 
                (getter, value) => getter.MessageText = value);

        public string MessageText
        {
            get => _messageText;
            set => SetAndRaise(MessageTextProperty , ref _messageText, value);
        }
        
        #endregion
        
        public ChatView()
        {
            InitializeComponent();
            FocusableElement = this.FindControl<TextBox>(FOCUSABLE_ELEMENT_NAME)!;
        }

        public static void SetFocusToFocusableElement()
        {
            FocusableElement.Focus();
        }
    }
    
}
