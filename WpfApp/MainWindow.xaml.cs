using Clipping2D.Clipping;
using Clipping2D.Polygon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using Point = System.Windows.Point;

namespace WpfApp
{
    public partial class MainWindow : Window
    {
        private DispatcherTimer timer;
        private double _crossCenterX = 400;
        private double _crossCenterY = 400;
        private double _crossWidth = 200;
        private double _crossHeight = 200;
        private int[,] M = new int[4, 2] { { 100, 300 }, { 400, 300 }, { 400, 450 }, { 100, 450 } };

        private float[] r = new float[31];
        private float[] s = new float[31];
        private int[,] M1 = new int[4, 2] { { 0, 0 }, { 0, 0 }, { 0, 0 }, { 0, 0 } };
        private int pr = 1;
        private int j;
        private float dx, dy;
        private float[,] dh = new float[4, 2];
        private int k;

        private Polygon2D _trapeze;
        private Polygon2D _rectangle;

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ////DrawCross();

            timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(100)
            };

            timer.Tick += Timer_Tick;

            Radio1.IsChecked = false;
            Radio2.IsChecked = true;
        }

        private void DrawCross()
        {
            Path _crossClipWindow = CreateCross(_crossCenterX, _crossCenterY);

            MainCanvas.Children.Add(_crossClipWindow);
        }

        private Path CreateCross(double centerX, double centerY)
        {
            PathGeometry pathGeometry = new PathGeometry();

            PathFigure pathFigure = new PathFigure
            {
                IsClosed = true
            };

            double innerWidth = _crossWidth / 4;
            double innerHeight = _crossHeight / 4;

            Point topMiddleLeft1 = new Point(centerX - innerWidth, centerY - innerHeight);
            Point topMiddleRight1 = new Point(centerX + innerWidth, centerY - innerHeight);
            Point topLeft2 = new Point(centerX - _crossWidth / 2, centerY - _crossHeight);
            Point topRight2 = new Point(centerX + _crossWidth / 2, centerY - _crossHeight);
            Point rightTop1 = new Point(centerX + _crossWidth, centerY - _crossHeight / 2);
            Point rightBottom2 = new Point(centerX + _crossWidth, centerY + _crossHeight / 2);
            Point bottomMiddleRight1 = new Point(centerX + innerWidth, centerY + innerHeight);
            Point bottomMiddleLeft1 = new Point(centerX - innerWidth, centerY + innerHeight);
            Point bottomRight2 = new Point(centerX + _crossWidth / 2, centerY + _crossHeight);
            Point bottomLeft2 = new Point(centerX - _crossWidth / 2, centerY + _crossHeight);
            Point leftTop2 = new Point(centerX - _crossWidth, centerY - _crossHeight / 2);
            Point leftBottom1 = new Point(centerX - _crossWidth, centerY + _crossHeight / 2);

            pathFigure.StartPoint = topMiddleLeft1;
            pathFigure.Segments.Add(new LineSegment(topLeft2, true));
            pathFigure.Segments.Add(new LineSegment(topRight2, true));
            pathFigure.Segments.Add(new LineSegment(topMiddleRight1, true));
            pathFigure.Segments.Add(new LineSegment(rightTop1, true));
            pathFigure.Segments.Add(new LineSegment(rightBottom2, true));
            pathFigure.Segments.Add(new LineSegment(bottomMiddleRight1, true));
            pathFigure.Segments.Add(new LineSegment(bottomRight2, true));
            pathFigure.Segments.Add(new LineSegment(bottomLeft2, true));
            pathFigure.Segments.Add(new LineSegment(bottomMiddleLeft1, true));
            pathFigure.Segments.Add(new LineSegment(leftBottom1, true));
            pathFigure.Segments.Add(new LineSegment(leftTop2, true));

            pathGeometry.Figures.Add(pathFigure);

            Path crossPath = new Path
            {
                Stroke = Brushes.Black,
                StrokeThickness = 2,
                Data = pathGeometry
            };

            return crossPath;
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            float[] centerX = new float[4];
            float[] centerY = new float[4];

            Draw(M[0, 0], M[1, 0], M[2, 0], M[3, 0], M[0, 1], M[1, 1], M[2, 1], M[3, 1]);

            for (int i = 0; i < 4; i++)
            {
                M1[i, 0] = M[i, 0];
                M1[i, 1] = M[i, 1];
            }

            Random rnd = new Random();
            for (int i = 1; i <= 30; i++)
            {
                r[i] = 4 * (float)rnd.NextDouble() - 2;
                s[i] = 4 * (float)rnd.NextDouble() - 2;
            }
            pr = 1;
            j = 0;
            dx = 2;
            dy = 1;

            for (int i = 0; i < 4; i++)
            {
                centerX[i] = M[i, 0];
                centerY[i] = M[i, 1];
            }
            float cx = (centerX[0] + centerX[1] + centerX[2] + centerX[3]) / 4;
            float cy = (centerY[0] + centerY[1] + centerY[2] + centerY[3]) / 4;

            for (int i = 0; i < 4; i++)
            {
                dh[i, 0] = (cx - M[i, 0]) / 30;
                dh[i, 1] = (cy - M[i, 1]) / 30;
            }
            StartButton.IsEnabled = false;
            ResetButton.IsEnabled = true;

            timer.Start();
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            pr = 0;
            MainCanvas.Children.Clear();
            Draw(M[0, 0], M[1, 0], M[2, 0], M[3, 0], M[0, 1], M[1, 1], M[2, 1], M[3, 1]);
            StartButton.IsEnabled = true;
            ResetButton.IsEnabled = false;
            timer.Stop();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        //private void SomeHow()
        //{

        //    var c12 = new Point(leftTop2.X, leftTop2.Y);

        //    var Points = new List<Point>
        //    {
        //        c1, c2, c3, c4, c5, c6, c7, c8, c9, c10, c11, c12,
        //    };

        //    //var p1 = new Point(300, 200);
        //    //var p2 = new Point(600, 200);
        //    //var p3 = new Point(600, 350);
        //    //var p4 = new Point(300, 350);
        //    var p1 = new Point(300, 200);
        //    var p2 = new Point(600, 200);
        //    var p3 = new Point(600, 350);
        //    var p4 = new Point(300, 350);

        //    var pol = new Polygon(Points);
        //    var cvadr = new List<Point>
        //    {
        //        c3, c4, c5, c6
        //    };

        //    var pol1 = new Polygon(cvadr);
        //    pol.CyrusBeckClip(new List<Segment> { new Segment(c5, c6) });
        //}
        public void AnotherTry(int l)
        {
            Polygon2D rec = GetRectangle(l);

            Polygon2D cross = GetCross();

            rec.ClipByPolygon(cross, ClippingType.External);
            DrawPolygone(rec);
            DrawPolygone(cross);
        }

        private Polygon2D GetRectangle(int l)
        {
            for (int i = 0; i < 4; i++)
            {
                M1[i, 0] = (int)(M[i, 0] + l * dh[i, 0] + r[l] * dx);
                M1[i, 1] = (int)(M[i, 1] + l * dh[i, 1] + s[l] * dy);
            }
            var pointList = new List<Point>
            {
                new Point(M1[0, 0], M1[0, 1]),
                new Point(M1[1, 0], M1[1, 1]),
                new Point(M1[2, 0], M1[2, 1]),
                new Point(M1[3, 0], M1[3, 1]),
                new Point(M1[0, 0], M1[0, 1]),
            };

            var rec = new Polygon2D(pointList);
            return rec;
        }

        private Polygon2D GetCross()
        {
            double innerWidth = _crossWidth / 4;
            double innerHeight = _crossHeight / 4;

            Point topMiddleLeft1 = new Point(_crossCenterX - innerWidth, _crossCenterY - innerHeight);
            Point topLeft2 = new Point(_crossCenterX - _crossWidth / 2, _crossCenterY - _crossHeight);
            Point topRight2 = new Point(_crossCenterX + _crossWidth / 2, _crossCenterY - _crossHeight);
            Point topMiddleRight1 = new Point(_crossCenterX + innerWidth, _crossCenterY - innerHeight);
            Point rightTop1 = new Point(_crossCenterX + _crossWidth, _crossCenterY - _crossHeight / 2);
            Point rightBottom2 = new Point(_crossCenterX + _crossWidth, _crossCenterY + _crossHeight / 2);
            Point bottomMiddleRight1 = new Point(_crossCenterX + innerWidth, _crossCenterY + innerHeight);
            Point bottomRight2 = new Point(_crossCenterX + _crossWidth / 2, _crossCenterY + _crossHeight);
            Point bottomLeft2 = new Point(_crossCenterX - _crossWidth / 2, _crossCenterY + _crossHeight);
            Point bottomMiddleLeft1 = new Point(_crossCenterX - innerWidth, _crossCenterY + innerHeight);
            Point leftBottom1 = new Point(_crossCenterX - _crossWidth, _crossCenterY + _crossHeight / 2);
            Point leftTop2 = new Point(_crossCenterX - _crossWidth, _crossCenterY - _crossHeight / 2);

            var ches = new List<Point>
            {
                topMiddleLeft1,
                topLeft2,
                topRight2,
                topMiddleRight1,
                rightTop1,
                rightBottom2,
                bottomMiddleRight1,
                bottomRight2,
                bottomLeft2,
                bottomMiddleLeft1,
                leftBottom1,
                leftTop2,
            };
            var line = new Polygon2D(ches);
            return line;
        }

        private void DrawPolygone(Polygon2D pol)
        {
            foreach (var item in pol.Edges)
            {
                var t = item.VisibleParts.Select(x => new List<Point> { x.Start, x.End });
                foreach (var l in t)
                {
                    Draw(l);
                }
            }
        }

        private void Draw(IEnumerable<Point> points)
        {
            PointCollection rectanglePointCollection = new PointCollection(points);
            Polyline rectangleLine = new Polyline
            {
                Stroke = Brushes.Black,
                StrokeThickness = 1
            };

            rectangleLine.Points = rectanglePointCollection;
            MainCanvas.Children.Add(rectangleLine);
        }

        private void PerformClipping()
        {
            _trapeze.ResetClipping();
            _trapeze.ClipByPolygon(_rectangle, ClippingType.Inside);
        }

        private void Draw1(IEnumerable<Point> ps)
        {
            var pol = new Polygon(ps);

            PointCollection rectanglePointCollection = new PointCollection(ps);
            Polyline rectangleLine = new Polyline
            {
                Stroke = Brushes.Black,
                StrokeThickness = 1
            };

            rectangleLine.Points = rectanglePointCollection;
            MainCanvas.Children.Add(rectangleLine);

            double innerWidth = _crossWidth / 4;
            double innerHeight = _crossHeight / 4;

            Point topMiddleLeft1 = new Point(_crossCenterX - innerWidth, _crossCenterY - innerHeight);
            Point topMiddleRight1 = new Point(_crossCenterX + innerWidth, _crossCenterY - innerHeight);
            Point topLeft2 = new Point(_crossCenterX - _crossWidth / 2, _crossCenterY - _crossHeight);
            Point topRight2 = new Point(_crossCenterX + _crossWidth / 2, _crossCenterY - _crossHeight);

            var cuttedLines = pol.CyrusBeckClip(new List<Segment>
            {
                new Segment(topMiddleLeft1, topLeft2),
                //new Segment(topLeft2, topRight2),
                //new Segment(topRight2, topMiddleRight1),
            });

            PointCollection myPointCollection = new PointCollection();
            foreach (var item in cuttedLines)
            {
                myPointCollection.Add(item.A);
                myPointCollection.Add(item.B);
            }


            var myPolygon = new System.Windows.Shapes.Polygon();
            myPolygon.Stroke = System.Windows.Media.Brushes.Black;

            myPolygon.Points = myPointCollection;
            MainCanvas.Children.Add(myPolygon);
        }

        private void Figure1(int l)
        {
            ////DrawCross();
            for (int i = 0; i < 4; i++)
            {
                M1[i, 0] = (int)(M[i, 0] + l * dh[i, 0] + r[l] * dx);
                M1[i, 1] = (int)(M[i, 1] + l * dh[i, 1] + s[l] * dy);
            }

            var pointList = new List<Point>
            {
                new Point(M1[0, 0], M1[0, 1]),
                new Point(M1[1, 0], M1[1, 1]),
                new Point(M1[2, 0], M1[2, 1]),
                new Point(M1[3, 0], M1[3, 1]),
                new Point(M1[0, 0], M1[0, 1]),
            };
            Draw1(pointList);
        }

        private void Draw(int x1, int x2, int x3, int x4, int y1, int y2, int y3, int y4)
        {
            Polyline polyline = new Polyline
            {
                Stroke = Brushes.Black,
                StrokeThickness = 1
            };

            polyline.Points.Add(new Point(x1, y1));
            polyline.Points.Add(new Point(x2, y2));
            polyline.Points.Add(new Point(x3, y3));
            polyline.Points.Add(new Point(x4, y4));
            polyline.Points.Add(new Point(x1, y1));

            MainCanvas.Children.Add(polyline);
        }

        private void Figure(int l)
        {
            DrawCross();
            for (int i = 0; i < 4; i++)
            {
                M1[i, 0] = (int)(M[i, 0] + l * dh[i, 0] + r[l] * dx);
                M1[i, 1] = (int)(M[i, 1] + l * dh[i, 1] + s[l] * dy);
            }
            Draw(M1[0, 0], M1[1, 0], M1[2, 0], M1[3, 0], M1[0, 1], M1[1, 1], M1[2, 1], M1[3, 1]);
        }

        private void HSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            pr = 0;
            j = (int)HSlider.Value;
            MainCanvas.Children.Clear();


            AnotherTry(1);
            if (Radio1.IsChecked == true)
            {
                for (k = 0; k <= 30; k++)
                {
                    AnotherTry(k);
                }
            }
            else
            {
                AnotherTry(j);
            }

            //if (Radio1.IsChecked == true)
            //{
            //    for (k = 0; k <= 30; k++)
            //    {
            //        Figure(k);
            //    }
            //}
            //else
            //{
            //    Figure(j);
            //}
        }

        private void HSlider_ValueChanged1(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            pr = 0;
            j = (int)HSlider.Value;
            MainCanvas.Children.Clear();


            Figure1(j);

            //if (Radio1.IsChecked == true)
            //{
            //    for (k = 0; k <= 30; k++)
            //    {
            //        Figure(k);
            //    }
            //}
            //else
            //{
            //    Figure(j);
            //}
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (pr == 1)
            {
                HSlider.Value = j;
                j++;
                if (j > 30) pr = 0;
            }
        }

        private void Radio1_Click(object sender, RoutedEventArgs e)
        {
            MainCanvas.Children.Clear();
            if (Radio1.IsChecked == true)
            {
                for (int k = 0; k <= 30; k++)
                {
                    Figure(k);
                }
            }
            else
            {
                Figure(j);
            }
        }

        private void Radio2_Click(object sender, RoutedEventArgs e)
        {
            MainCanvas.Children.Clear();
            if (Radio1.IsChecked == true)
            {
                for (int k = 0; k <= 30; k++)
                {
                    Figure(k);
                }
            }
            else
            {
                Figure(j);
            }
        }

        public struct Segment
        {
            public readonly Point A, B;

            public Segment(Point a, Point b)
            {
                A = a;
                B = b;
            }

            public bool OnLeft(Point p)
            {
                var ab = B.Sub(A);
                var ap = p.Sub(A);
                return ab.Cross(ap) >= 0;
            }

            public Point Normal
            {
                get
                {
                    return new Point(B.Y - A.Y, A.X - B.X);
                }
            }

            public Point Direction
            {
                get
                {
                    return new Point(B.X - A.X, B.Y - A.Y);
                }
            }

            public double IntersectionParameter(Segment that)
            {
                var segment = this;
                var edge = that;

                var segmentToEdge = edge.A.Sub(segment.A);
                var segmentDir = segment.Direction;
                var edgeDir = edge.Direction;

                var t = edgeDir.Cross(segmentToEdge) / edgeDir.Cross(segmentDir);

                if (double.IsNaN(t))
                {
                    t = 0;
                }

                return t;
            }

            public Segment Morph(double tA, double tB)
            {
                var d = Direction;
                return new Segment(A.Add(d.Mul(tA)), A.Add(d.Mul(tB)));
            }
        }

        public class Polygon : List<Point>
        {
            public Polygon()
                : base()
            { }

            public Polygon(int capacity)
                : base(capacity)
            { }

            public Polygon(IEnumerable<Point> collection)
                : base(collection)
            { }

            public bool IsConvex
            {
                get
                {
                    if (Count >= 3)
                    {
                        for (int a = Count - 2, b = Count - 1, c = 0; c < Count; a = b, b = c, ++c)
                        {
                            if (!new Segment(this[a], this[b]).OnLeft(this[c]))
                            {
                                return false;
                            }
                        }
                    }
                    return true;
                }
            }

            public IEnumerable<Segment> Edges
            {
                get
                {
                    if (Count >= 2)
                    {
                        for (int a = Count - 1, b = 0; b < Count; a = b, ++b)
                        {
                            yield return new Segment(this[a], this[b]);
                        }
                    }
                }
            }

            private bool CyrusBeckClip(ref Segment subject)
            {
                var subjDir = subject.Direction;
                var tA = 0.0;
                var tB = 1.0;
                foreach (var edge in Edges)
                {
                    switch (Math.Sign(edge.Normal.Dot(subjDir)))
                    {
                        case -1:
                            {
                                var t = subject.IntersectionParameter(edge);
                                if (t > tA)
                                {
                                    tA = t;
                                }
                                break;
                            }
                        case +1:
                            {
                                var t = subject.IntersectionParameter(edge);
                                if (t < tB)
                                {
                                    tB = t;
                                }
                                break;
                            }
                        case 0:
                            {
                                if (!edge.OnLeft(subject.A))
                                {
                                    return false;
                                }
                                break;
                            }
                    }
                }
                //if (tA > tB)
                //{
                //    return false;
                //}
                subject = subject.Morph(tA, tB);
                return true;
            }

            public List<Segment> CyrusBeckClip(List<Segment> subjects)
            {
                if (!IsConvex)
                {
                    Reverse();
                    if (!IsConvex)
                    {
                        throw new InvalidOperationException("Clip polygon must be convex.");
                    }
                }

                var clippedSubjects = new List<Segment>();
                foreach (var subject in subjects)
                {
                    var clippedSubject = subject;
                    if (CyrusBeckClip(ref clippedSubject))
                    {
                        clippedSubjects.Add(clippedSubject);
                    }
                }
                return clippedSubjects;
            }

        }

        public void DrawPolygonPoint()
        {
            // Create points that define polygon.
            Point point1 = new Point(50, 50);
            Point point2 = new Point(100, 25);
            Point point3 = new Point(200, 5);
            Point point4 = new Point(250, 50);
            Point point5 = new Point(300, 100);
            Point point6 = new Point(350, 200);
            Point point7 = new Point(250, 250);

            Point[] curvePoints =
                        {
                point1,
                point2,
                point3,
                point4,
                point5,
                point6,
                point7
            };
            PointCollection myPointCollection = new PointCollection(curvePoints.AsEnumerable<Point>());

            var myPolygon = new System.Windows.Shapes.Polygon();
            myPolygon.Stroke = System.Windows.Media.Brushes.Black;

            myPolygon.Points = myPointCollection;
            // Draw polygon to screen.
            MainCanvas.Children.Add(myPolygon);
        }
    }

    public static class PointExtensions
    {
        public static Point Add(this Point a, Point b)
        {
            return new Point(a.X + b.X, a.Y + b.Y);
        }

        public static Point Sub(this Point a, Point b)
        {
            return new Point(a.X - b.X, a.Y - b.Y);
        }

        public static Point Mul(this Point a, double b)
        {
            return new Point(a.X * b, a.Y * b);
        }

        public static double Dot(this Point a, Point b)
        {
            return a.X * b.X + a.Y * b.Y;
        }

        public static double Cross(this Point a, Point b)
        {
            return a.X * b.Y - a.Y * b.X;
        }
    }
}