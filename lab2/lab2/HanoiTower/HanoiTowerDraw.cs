using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace lab2.HanoiTower;

public class HanoiTowerDraw
{
    private const int RingHeight = 20;
    private const int RingWidthIncrement = 15;
    private Canvas HanoiCanvas;
    public List<(int from, int to)> Moves { get; private set; } = new List<(int from, int to)>();
    
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
                X1 = towerX - 180,
                Y1 = HanoiCanvas.ActualHeight - 50,
                X2 = towerX + 180, 
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
            double topPosition = 750 - (i + 1) * height;

            Canvas.SetLeft(shape, leftPosition);
            Canvas.SetTop(shape, topPosition);
            Panel.SetZIndex(shape, 2);

            HanoiCanvas.Children.Add(shape);
            towers[0].Push(i);
            diskRectangles[i, 0] = shape;
        }

        return diskRectangles;
    }

    public async Task MoveDisk(int from, int to, Stack<int>[] towers, Rectangle[,] diskRectangles, CancellationToken token, bool animate = true)
    {
        if (towers[from].Count == 0) return;

        token.ThrowIfCancellationRequested();

        int ring = towers[from].Pop();
        towers[to].Push(ring);

        Rectangle shape = diskRectangles[ring, from];
        diskRectangles[ring, to] = shape;
        diskRectangles[ring, from] = null;

        double canvasWidth = HanoiCanvas.ActualWidth;
        double towerSpacing = canvasWidth / 3;
        double targetX = towerSpacing / 2 + to * towerSpacing - shape.Width / 2;
        double targetY = 750 - towers[to].Count * RingHeight;

        if (animate)
        {
            await AnimateDisk(shape, targetX, targetY);
        }
        else
        {
            Canvas.SetLeft(shape, targetX);
            Canvas.SetTop(shape, targetY);
        }
    }

    public async Task MoveDiskBack(int from, int to, Stack<int>[] towers, Rectangle[,] diskRectangles, CancellationToken token, bool animate = true)
    {
        if (towers[to].Count == 0) return;

        token.ThrowIfCancellationRequested();

        int ring = towers[to].Pop(); 
        towers[from].Push(ring);    

        Rectangle shape = diskRectangles[ring, to];
        diskRectangles[ring, from] = shape;
        diskRectangles[ring, to] = null;

        double canvasWidth = HanoiCanvas.ActualWidth;
        double towerSpacing = canvasWidth / 3;
        double targetX = towerSpacing / 2 + from * towerSpacing - shape.Width / 2;
        double targetY = 750 - towers[from].Count * RingHeight;

        if (animate)
        {
            await AnimateDisk(shape, targetX, targetY);
        }
        else
        {
            Canvas.SetLeft(shape, targetX);
            Canvas.SetTop(shape, targetY);
        }
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
    
    public void GenerateMovesList(int n, int from_tower, int to_tower, int else_tower)
    {
        if (n > 0)
        {
            GenerateMovesList(n - 1, from_tower, else_tower, to_tower);
            Moves.Add((from_tower, to_tower));
            GenerateMovesList(n - 1, else_tower, to_tower, from_tower);
        }
    }

    public void ClearCanvas()
    {
        HanoiCanvas.Children.Clear();
    }
}