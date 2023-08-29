using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using Avalonia.VisualTree;
using DesktopClient.Models.Drawing;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Avalonia.Layout;
using Avalonia.LogicalTree;

namespace DesktopClient.Views
{
    public class Message : TemplatedControl, INotifyPropertyChanged
    {
        private const string PART_CANVAS_NAME = "PART_CANVAS";
        private const string PART_BORDER_NAME = "PART_BORDER";
        
        private static ItemsControl ParentControl = null!;
        
        private Canvas _canvas = null!;
        private Border _border = null!;


        #region INotifyPropertyChanged

        public new event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            
        }

        #endregion
        
        #region MaxDesiredWidth

        private double _maxDesiredWidth;

        public static readonly DirectProperty<Message, double> MaxDesiredWidthProperty =
            AvaloniaProperty.RegisterDirect<Message, double>(
                nameof(MaxDesiredWidth), 
                o => o.MaxDesiredWidth, 
                (o, v) => o.MaxDesiredWidth = v);

        public double MaxDesiredWidth
        {
            get => _maxDesiredWidth;
            set => SetAndRaise(MaxDesiredWidthProperty, ref _maxDesiredWidth, value - 100);
        }

        #endregion
        
        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);

            ParentControl = Parent.FindLogicalAncestorOfType<ItemsControl>()!;
            _canvas = (Canvas)e.NameScope.Find(PART_CANVAS_NAME)!;
            _border = (Border)e.NameScope.Find(PART_BORDER_NAME)!;
            
            switch (HorizontalAlignment)
            {
                // the message arrow indicates left -- <(...)
                // so the message is from your friend
                case HorizontalAlignment.Left:
                {
                    var path = ArrowDrawingHelper.DrawLeftArrow();

                    _border.Background = ArrowDrawingHelper.FriendsMessageColorBrush;
                    _border.CornerRadius = ArrowDrawingHelper.LeftRoundedRadius;
                    _canvas.Children.Add(path);

                    Grid.SetColumn(_border, 1);
                    Grid.SetColumn(_canvas, 0);
                }
                    break;

                // the message arrow indicates right -- (...)>
                // so the message is yours
                case HorizontalAlignment.Right:
                {
                    var path = ArrowDrawingHelper.DrawRightArrow();
                    _border.Background = ArrowDrawingHelper.YourMessageColorBrush;
                    _border.CornerRadius = ArrowDrawingHelper.RightRoundedRadius;
                    _canvas.Children.Add(path);

                    Grid.SetColumn(_border, 0);
                    Grid.SetColumn(_canvas, 1);
                }
                    break;
            }
        }

        #region YourMessageBackground

        public static readonly DirectProperty<Message, Color> YourMessageBackgroundColorProperty =
            AvaloniaProperty.RegisterDirect<Message, Color>(
                nameof(YourMessageBackgroundColor),
                getter => getter.YourMessageBackgroundColor,
                (setter, value) => setter.YourMessageBackgroundColor = value);

        public Color YourMessageBackgroundColor
        {
            get => ArrowDrawingHelper.YourMessageStrokeAndFillColor;
            set
            {
                SetAndRaise(YourMessageBackgroundColorProperty, ref ArrowDrawingHelper.YourMessageStrokeAndFillColor,
                    value);
                ArrowDrawingHelper.YourMessageColorBrush = new SolidColorBrush(YourMessageBackgroundColor);
            }
        }

        #endregion

        #region FriendsMessageBackground

        public static readonly DirectProperty<Message, Color> FriendsMessageBackgroundColorProperty =
            AvaloniaProperty.RegisterDirect<Message, Color>(
                nameof(FriendsMessageBackgroundColor),
                getter => getter.FriendsMessageBackgroundColor,
                (setter, value) => setter.FriendsMessageBackgroundColor = value);

        public Color FriendsMessageBackgroundColor
        {
            get => ArrowDrawingHelper.FriendsMessageStrokeAndFillColor;
            set
            {
                SetAndRaise(YourMessageBackgroundColorProperty, ref ArrowDrawingHelper.FriendsMessageStrokeAndFillColor,
                    value);
                ArrowDrawingHelper.FriendsMessageColorBrush = new SolidColorBrush(FriendsMessageBackgroundColor);
            }
        }

        #endregion

        #region Text

        public static readonly DirectProperty<Message, string> TextProperty =
            AvaloniaProperty.RegisterDirect<Message, string>(
                nameof(Text),
                getter => getter.Text,
                (setter, value) => setter.Text = value);

        private string _text = null!;

        public string Text
        {
            get => _text;
            set => SetAndRaise(TextProperty, ref _text, value);
        }

        #endregion
    }
}