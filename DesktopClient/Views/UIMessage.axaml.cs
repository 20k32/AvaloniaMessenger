using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Shapes;
using Avalonia.Media;

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
                        _border.CornerRadius = new CornerRadius(4, 0, 4, 4);

                        var path = new Path();

                        path.Fill = Brushes.Aqua;

                        path.Data = new PathGeometry()
                        {
                            Figures = new PathFigures()
                            {
                                new PathFigure()
                                {
                                    StartPoint = new Point(10, 0),
                                    Segments = new PathSegments()
                                    {
                                        new LineSegment()
                                        {
                                            Point = new Point(0, 5),
                                        },
                                        new LineSegment()
                                        {
                                            Point = new Point(10, 10)
                                        }
                                    }
                                }
                            }
                        };

                        _canvas.Children.Add(path);
                        Grid.SetColumn(_border, 1);
                        Grid.SetColumn(_canvas, 0);
                    } break;

                // the message arrow indicates right -- (...)>
                case 1:
                    {
                        var path = new Path();

                        path.Fill = Brushes.Red;

                        path.Data = new PathGeometry()
                        {
                            Figures = new PathFigures()
                            {
                                new PathFigure()
                                {
                                    StartPoint = new Point(0, 0),
                                    Segments = new PathSegments()
                                    {
                                        new LineSegment()
                                        {
                                            Point = new Point(10, 5),
                                        },
                                        new LineSegment()
                                        {
                                            Point = new Point(0, 10)
                                        }
                                    }
                                }
                            }
                        };

                        _canvas.Children.Add(path);
                        _border.CornerRadius = new CornerRadius(4, 4, 0, 4);

                        Grid.SetColumn(_border, 0);
                        Grid.SetColumn(_canvas, 1);
                    } break;
            }
        }
    }
}
