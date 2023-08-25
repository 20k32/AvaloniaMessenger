using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using Avalonia.VisualTree;
using DesktopClient.Models.Drawing;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DesktopClient.Views
{
    public class UIMessage : TemplatedControl, INotifyPropertyChanged
    {
        private const string PART_CANVAS_NAME = "PART_CANVAS";
        private const string PART_BORDER_NAME = "PART_BORDER";

        private Canvas _canvas = null!;
        private Border _border = null!;

        #region INotifyPropertyChanged
        
        public new event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName]string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            DataContext = this;

            base.OnApplyTemplate(e);

            _canvas = (Canvas)e.NameScope.Find(PART_CANVAS_NAME)!;
            _border = (Border)e.NameScope.Find(PART_BORDER_NAME)!;

            switch (Grid.GetColumn(this))
            {
                // the message arrow indicates left -- <(...)
                case 0:
                {
                    var path = ArrowDrawingHelper.DrawLeftArrow();

                    _border.CornerRadius = ArrowDrawingHelper.LeftRoundedRadius;
                    _canvas.Children.Add(path);

                    Grid.SetColumn(_border, 1);
                    Grid.SetColumn(_canvas, 0);
                }
                break;

                // the message arrow indicates right -- (...)>
                case 1:
                {
                    var path = ArrowDrawingHelper.DrawRightArrow();

                    _border.CornerRadius = ArrowDrawingHelper.RightRoundedRadius;
                    _canvas.Children.Add(path);

                    Grid.SetColumn(_border, 0);
                    Grid.SetColumn(_canvas, 1);
                }
                break;
            }
        }

        #region MessageBackground

        public static readonly DirectProperty<UIMessage, Color> BackgroundColorProperty =
            AvaloniaProperty.RegisterDirect<UIMessage, Color>(
                nameof(BackgroundColor),
                getter => getter.BackgroundColor,
                (setter, value) => setter.BackgroundColor = value);

        public Color BackgroundColor
        {
            get => ArrowDrawingHelper.MessageStrokeAndFillColor;
            set
            {
                SetAndRaise(BackgroundColorProperty, ref ArrowDrawingHelper.MessageStrokeAndFillColor, value);
                BorderBackgroundColor = new SolidColorBrush(BackgroundColor);
            }
        }

        public SolidColorBrush BorderBackgroundColor
        {
            get => ArrowDrawingHelper.MessageColorBrush;
            set
            {
                ArrowDrawingHelper.MessageColorBrush = value;
                OnPropertyChanged();
            }
        }

        #endregion
    }
}
