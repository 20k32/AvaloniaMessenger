using Avalonia;
using Avalonia.Controls.Shapes;
using Avalonia.Media;

namespace DesktopClient.Models.Drawing
{
    internal static class ArrowDrawingHelper
    {
        private static Geometry _rightArrowGeometry = 
            new PathGeometry()
            {
                Figures = new PathFigures()
                        {
                            new PathFigure()
                            {
                                StartPoint = new Point(0, 0),

                                Segments = new PathSegments()
                                {
                                    new QuadraticBezierSegment()
                                    {
                                        Point1 = new(5, 8),
                                        Point2 = new(10, 10)
                                    },
                                    new LineSegment()
                                    {
                                        Point = new Point(0, 10),
                                    },
                                }
                            }
                        }
            };

        private static Geometry _leftArrowGeometry = 
            new PathGeometry()
            {
                Figures = new PathFigures()
                        {
                            new PathFigure()
                            {
                                StartPoint = new Point(10, 0),
                                Segments = new PathSegments()
                                {
                                    new QuadraticBezierSegment()
                                    {
                                        Point1 = new(6, 7),
                                        Point2 = new(0, 10)
                                    },
                                    new LineSegment()
                                    {
                                        Point = new Point(10, 10),
                                    },
                                }
                            }
                        }
            };

        public static CornerRadius RightRoundedRadius = new(10, 10, 0, 10);
        public static CornerRadius LeftRoundedRadius = new(10, 10, 10, 0);
        public static Color YourMessageStrokeAndFillColor;
        public static Color FriendsMessageStrokeAndFillColor;
        public static SolidColorBrush YourMessageColorBrush = new(YourMessageStrokeAndFillColor);
        public static SolidColorBrush FriendsMessageColorBrush = new(FriendsMessageStrokeAndFillColor);

        public static Path DrawRightArrow() =>
            new Path()
            {
                Data = _rightArrowGeometry,
                Fill = YourMessageColorBrush,
                Stroke = YourMessageColorBrush,
            };


        public static Path DrawLeftArrow() =>
            new Path()
            {
                Data = _leftArrowGeometry,
                Fill = FriendsMessageColorBrush,
                Stroke = FriendsMessageColorBrush,
            };
    }
}
