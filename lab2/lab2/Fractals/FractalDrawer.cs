using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Drawing;

namespace lab2.Fractals
{
    class DurersStar
    {
        private readonly Canvas _canvas;
        private Brush _brushForThickness;
        private int _thickness;
        private int _coordX;
        private int _coordY;
        private int _radius = 130;
        private double _angle = Math.PI / 2;
        public DurersStar(Canvas canvas, Point center, Brush brushForThickness, int thickness, int deth)
        {
            _canvas = canvas;
            _brushForThickness = brushForThickness;
            _thickness = thickness;
            _coordX = center.X;
            _coordY = center.Y;
            DrawStar(_coordX, _coordY, _radius, _angle, deth);
        }


        private void DrawLine(Point start, Point end, Brush color, int thickness)
        {
            Line line = new Line();

            line.StrokeThickness = thickness;
            line.Stroke = color;

            line.X1 = start.X;
            line.Y1 = start.Y;

            line.X2 = end.X;
            line.Y2 = end.Y;

            _canvas.Children.Add(line);
        }

        //(coordXForFirstPentagon, coordYForFirstPentagon) - (х, у) координаты центра первого пятиугольника
        // radius - радиус описанной окружности (типа размер пятиугольников)
        // d - количество уровней рекурсии
        // angle - угол поворота (для правильной отрисовки)

        private void DrawPentagon(double coordXForFirstPentagon, double coordYForFirstPentagon, double radius, double angle)
        {
            double[] x1 = new double[6];
            double[] y1 = new double[6];

            for (int i = 0; i <= 5; i++)
            {
                x1[i] = radius * Math.Cos(angle + i * Math.PI * 2 / 5);
                y1[i] = radius * Math.Sin(angle + i * Math.PI * 2 / 5);
            }

            for (int i = 0; i <= 4; i++)
            {
                Point pStart = new Point((int)Math.Round(coordXForFirstPentagon + x1[i]), (int)Math.Round(coordYForFirstPentagon + y1[i]));
                Point pEnd = new Point((int)Math.Round(coordXForFirstPentagon + x1[i + 1]), (int)Math.Round(coordYForFirstPentagon + y1[i + 1]));
                DrawLine(pStart, pEnd, _brushForThickness, 2);
            }
        }

        private void DrawStar(double coordXForFirstPentagon, double coordYForFirstPentagon, double radius, double angle, int d)
        {
            double h = 2 * radius * Math.Cos(Math.PI / 5); //смещение от центра для рисования следующего пятиугольника

            for (int i = 0; i < 5; i++)
            {
                DrawPentagon(coordXForFirstPentagon - h * Math.Cos(angle + i * Math.PI * 2 / 5),
                    coordYForFirstPentagon - h * Math.Sin(angle + i * Math.PI * 2 / 5),
                    radius,
                    angle + Math.PI + i * Math.PI * 2 / 5);

                if (d > 0)
                {
                    DrawStar(coordXForFirstPentagon - h * Math.Cos(angle + i * Math.PI * 2 / 5),
                        coordYForFirstPentagon - h * Math.Sin(angle + i * Math.PI * 2 / 5),
                        radius / (2 * Math.Cos(Math.PI / 5) + 1), angle + Math.PI + (2 * i + 1) * Math.PI * 2 / 10,
                        d - 1);
                }
            }
            DrawPentagon(coordXForFirstPentagon, coordYForFirstPentagon, radius, angle);

            if (d > 0)
                DrawStar(coordXForFirstPentagon, coordYForFirstPentagon,
                    radius / (2 * Math.Cos(Math.PI / 5) + 1), angle + Math.PI, d - 1);
        }
    }
}
