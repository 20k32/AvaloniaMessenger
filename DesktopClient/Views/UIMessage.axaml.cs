using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using Avalonia.VisualTree;
using DesktopClient.Models.Drawing;

namespace DesktopClient.Views
{
    public class UIMessage : TemplatedControl
    {
        private const string PART_CANVAS_NAME = "PART_CANVAS";
        private const string PART_BORDER_NAME = "PART_BORDER";
        
        private Canvas _canvas = null!;
        private Border _border = null!;

        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);

            _canvas = (Canvas)e.NameScope.Find(PART_CANVAS_NAME)!;
            _border = (Border)e.NameScope.Find(PART_BORDER_NAME)!;

            switch (Grid.GetColumn(this))
            {
                // the message arrow indicates left -- <(...)
                case 0:
                    {
                        var path = ArrowDrawingHelper.DrawLeftArrow(Brushes.AliceBlue, Brushes.AliceBlue, 1);

                        _border.CornerRadius = ArrowDrawingHelper.LeftRoundedRadius;
                        _canvas.Children.Add(path);

                        Grid.SetColumn(_border, 1);
                        Grid.SetColumn(_canvas, 0);
                    } break;

                // the message arrow indicates right -- (...)>
                case 1:
                    {
                        var path = ArrowDrawingHelper.DrawRightArrow(Brushes.AliceBlue, Brushes.AliceBlue, 1);

                        _border.CornerRadius = ArrowDrawingHelper.RightRoundedRadius;
                        _canvas.Children.Add(path);

                        Grid.SetColumn(_border, 0);
                        Grid.SetColumn(_canvas, 1);
                    } break;
            }
        }
    }
}
