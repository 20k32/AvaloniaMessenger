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

        public static CornerRadius RightRoundedRadius = new(4, 4, 0, 4);
        public static CornerRadius LeftRoundedRadius = new(4, 4, 4, 0);

        public static Path DrawRightArrow(IBrush fill, IBrush stroke, double strokeThickness) =>
            new Path()
            {
                Data = _rightArrowGeometry,
                Fill = fill,
                Stroke = stroke,
                StrokeThickness = strokeThickness,
            };


        public static Path DrawLeftArrow(IBrush fill, IBrush stroke, double strokeThickness) =>
            new Path()
            {
                Data = _leftArrowGeometry,
                Fill = fill,
                Stroke = stroke,
                StrokeThickness = strokeThickness,
            };
    }
}
