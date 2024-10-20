using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace lab2.Pages;

public partial class HanoiTowerPage : Page
{
    private const int num_of_towers = 3;
    private const int RingHeight = 20;
    private const int RingWidthIncrement = 15;
    private readonly Stack<int>[] towers = new Stack<int>[num_of_towers];
    private Rectangle[,] diskRectangles;
    private int animationSpeed = 20;
    private CancellationTokenSource cancellationTokenSource;

    public HanoiTowerPage()
    {
        InitializeComponent();
        InitializeTowers();
    }
    private void InitializeTowers()
    {
        for (int i = 0; i < num_of_towers; i++)
        {
            towers[i] = new Stack<int>();
        }
    }

    private void DrawTowers()
    {
        HanoiCanvas.Children.Clear();
        double canvasWidth = HanoiCanvas.ActualWidth;
        double towerSpacing = canvasWidth / num_of_towers;

        for (int i = 0; i < num_of_towers; i++)
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
                X1 = towerX - towerSpacing / 3 - 50,
                Y1 = HanoiCanvas.ActualHeight - 50,
                X2 = towerX + towerSpacing / 3 + 50,
                Y2 = HanoiCanvas.ActualHeight - 50,
                Stroke = Brushes.Black,
                StrokeThickness = 4
            };
            HanoiCanvas.Children.Add(tower_base);
        }
    }

    private void InitializeRings(int numberOfRings)
    {
        diskRectangles = new Rectangle[numberOfRings, num_of_towers];

        for (int i = 0; i < numberOfRings; i++)
        {
            double width = RingWidthIncrement * (numberOfRings - i);
            double height = RingHeight;

            var shape = new Rectangle
            {
                Width = width,
                Height = height,
                Fill = Brushes.LightBlue,
                Stroke = Brushes.Black,
                StrokeThickness = 1
            };

            double leftPosition = HanoiCanvas.ActualWidth / num_of_towers / 2 - width / 2;
            double topPosition = 655 - (i + 1) * height;

            Canvas.SetLeft(shape, leftPosition);
            Canvas.SetTop(shape, topPosition);

            Panel.SetZIndex(shape, 2);

            HanoiCanvas.Children.Add(shape);

            towers[0].Push(i);
            diskRectangles[i, 0] = shape;
        }
    }

    private async Task GenerateMoves(int n, int from_tower, int to_tower, int else_tower, CancellationToken token)
    {
        if (n > 0)
        {
            await GenerateMoves(n - 1, from_tower, else_tower, to_tower, token);
            await MoveDisk(from_tower, to_tower, token);
            await GenerateMoves(n - 1, else_tower, to_tower, from_tower, token);
        }
    }

    private async Task MoveDisk(int from, int to, CancellationToken token)
    {
        if (towers[from].Count == 0) return;

        token.ThrowIfCancellationRequested();

        int ring = towers[from].Pop();
        towers[to].Push(ring);

        Rectangle Shape = diskRectangles[ring, from];
        diskRectangles[ring, to] = Shape;

        double canvasWidth = HanoiCanvas.ActualWidth;
        double towerSpacing = canvasWidth / num_of_towers;
        double towerCenterX = towerSpacing / 2 + to * towerSpacing;
        double targetX = towerCenterX - Shape.Width / 2;
        double targetY = 655 - towers[to].Count * RingHeight;

        await AnimateDisk(Shape, targetX, targetY);
    }

    private async Task AnimateDisk(Rectangle shape, double targetX, double targetY)
    {
        double currentX = Canvas.GetLeft(shape);
        double currentY = Canvas.GetTop(shape);
        int flag = animationSpeed;

        while (Math.Abs(currentX - targetX) > 1 || Math.Abs(currentY - targetY) > 1)
        {
            while (Math.Abs(currentY - 130) > 1)
            {
                currentY += (130 - currentY) / (100.0 / animationSpeed / 1.5);
                Canvas.SetTop(shape, currentY);
                await Task.Delay(100 / flag);
            }
            while (Math.Abs(currentX - targetX) > 1)
            {
                currentX += (targetX - currentX) / (100.0 / animationSpeed);
                Canvas.SetLeft(shape, currentX);
                await Task.Delay(100 / flag);
            }
            while (Math.Abs(currentY - targetY) > 1)
            {
                currentY += (targetY - currentY) / (100.0 / animationSpeed / 1.5);
                Canvas.SetTop(shape, currentY);
                await Task.Delay(100 / flag);
            }
        }

        Canvas.SetLeft(shape, targetX);
        Canvas.SetTop(shape, targetY);
    }
    private async void StartButton_Click(object sender, RoutedEventArgs e)
    {
        startButton.IsEnabled = false;
        ringCountTextBox.IsEnabled = false;
        clearButton.IsEnabled = true;
        cancellationTokenSource = new CancellationTokenSource();
        var token = cancellationTokenSource.Token;
        if (int.TryParse(ringCountTextBox.Text, out int numberOfRings) && numberOfRings > 0 && numberOfRings <= 23)
        {
            InitializeTowers();
            DrawTowers();
            InitializeRings(numberOfRings);
            try
            {
                await GenerateMoves(numberOfRings, 0, 1, 2, token);
            }
            catch (OperationCanceledException) { };
        }
        else
        {
            MessageBox.Show("введите корректное количество колец");
        }
        startButton.IsEnabled = true;
        ringCountTextBox.IsEnabled = true;
        clearButton.IsEnabled = false;
    }

    private void ClearButton_Click(object sender, RoutedEventArgs e)
    {
        cancellationTokenSource?.Cancel();
        HanoiCanvas.Children.Clear();
    }

}