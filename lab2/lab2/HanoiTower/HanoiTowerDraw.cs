using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace lab2.HanoiTower;

public class HanoiTowerDraw
{
    private const int RingHeight = 20;
    private const int RingWidthIncrement = 15;
    private Canvas HanoiCanvas;

    public HanoiTowerDraw(Canvas hanoiCanvas)
    { 
        HanoiCanvas = hanoiCanvas;
    }

    public void DrawTowers()
    { 
        HanoiCanvas.Children.Clear();
        double canvasWidth = HanoiCanvas.ActualWidth;
        double towerSpacing = canvasWidth / 3;

        for (int i = 0; i < 3; i++)
        { 
            double towerX = towerSpacing / 2 + i * towerSpacing;
            var tower_rod = new Line
            {
                X1 = towerX,
                Y1 = 200,
                X2 = towerX,
                Y2 = HanoiCanvas.ActualHeight - 50,
                Stroke = Brushes.Black, 
                StrokeThickness = 4
            };
            HanoiCanvas.Children.Add(tower_rod);

            var tower_base = new Line
            {
                X1 = towerX - 50, // Слева от центра башни
                Y1 = HanoiCanvas.ActualHeight - 50,
                X2 = towerX + 50, // Справа от центра башни
                Y2 = HanoiCanvas.ActualHeight - 50,
                Stroke = Brushes.Black,
                StrokeThickness = 4
            };
            HanoiCanvas.Children.Add(tower_base);
        }
    }

    public Rectangle[,] InitializeRings(int numberOfRings, Stack<int>[] towers)
    {
        Rectangle[,] diskRectangles = new Rectangle[numberOfRings, 3];

        for (int i = 0; i < numberOfRings; i++)
        {
            double width = RingWidthIncrement * (numberOfRings - i);
            double height = RingHeight;

            var shape = new Rectangle
            {
                Width = width,
                Height = height,
                Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1F77B4")),
                Stroke = Brushes.Black,
                StrokeThickness = 1
            };

            double leftPosition = HanoiCanvas.ActualWidth / 3 / 2 - width / 2;
            double topPosition = 655 - (i + 1) * height;

            Canvas.SetLeft(shape, leftPosition);
            Canvas.SetTop(shape, topPosition);
            Panel.SetZIndex(shape, 2);

            HanoiCanvas.Children.Add(shape);
            towers[0].Push(i);
            diskRectangles[i, 0] = shape;
        }

        return diskRectangles;
    }

    public async Task GenerateMoves(int n, int from_tower, int to_tower, int else_tower, Stack<int>[] towers, Rectangle[,] diskRectangles, CancellationToken token)
    {
        if (n > 0)
        {
            await GenerateMoves(n - 1, from_tower, else_tower, to_tower, towers, diskRectangles, token);
            await MoveDisk(from_tower, to_tower, towers, diskRectangles, token);
            await GenerateMoves(n - 1, else_tower, to_tower, from_tower, towers, diskRectangles, token);
        }
    }

    public async Task MoveDisk(int from, int to, Stack<int>[] towers, Rectangle[,] diskRectangles, CancellationToken token)
    {
        if (towers[from].Count == 0) return;

        token.ThrowIfCancellationRequested();

        int ring = towers[from].Pop();
        towers[to].Push(ring);

        Rectangle shape = diskRectangles[ring, from];
        diskRectangles[ring, to] = shape;

        double canvasWidth = HanoiCanvas.ActualWidth;
        double towerSpacing = canvasWidth / 3;
        double targetX = towerSpacing / 2 + to * towerSpacing - shape.Width / 2;
        double targetY = 655 - towers[to].Count * RingHeight;

        await AnimateDisk(shape, targetX, targetY);
    }

    public async Task AnimateDisk(Rectangle shape, double targetX, double targetY)
    {
        double currentX = Canvas.GetLeft(shape);
        double currentY = Canvas.GetTop(shape);
        int flag = 20;

        while (Math.Abs(currentX - targetX) > 1 || Math.Abs(currentY - targetY) > 1)
        { 
            // Анимация по оси Y вверх
            while (Math.Abs(currentY - 130) > 1)
            { 
                currentY += (130 - currentY) / (100.0 / flag / 1.5);
                Canvas.SetTop(shape, currentY);
                await Task.Delay(100 / flag);
            }
            
            // Анимация по оси X
            while (Math.Abs(currentX - targetX) > 1)
            {
                currentX += (targetX - currentX) / (100.0 / flag);
                Canvas.SetLeft(shape, currentX);
                await Task.Delay(100 / flag);
            }
            
            // Анимация по оси Y вниз
            while (Math.Abs(currentY - targetY) > 1)
            {
                currentY += (targetY - currentY) / (100.0 / flag / 1.5);
                Canvas.SetTop(shape, currentY);
                await Task.Delay(100 / flag);
            }
        }

        Canvas.SetLeft(shape, targetX);
        Canvas.SetTop(shape, targetY);
    }

    public void ClearCanvas()
    {
        HanoiCanvas.Children.Clear();
    }
}