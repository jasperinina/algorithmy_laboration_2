using System.Net.NetworkInformation;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace lab2.Tests
{
    public partial class HanoiTowerPageTests : Page
    {
        private MainWindow _mainWindow;
        private const int num_of_towers = 3;
        private List<Tuple<int, int>> moves;

        public HanoiTowerPageTests(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
        }
        private void InitializeTowers(int numRings)
        {
            HanoiCanvas.Children.Clear();
            moves = new List<Tuple<int, int>>();
            DrawRings(CreateRing(numRings));
        }
        private void GenerateMoves(int n, int from_tower, int to_tower, int else_tower)
        {
            if (n > 0)
            {
                GenerateMoves(n - 1, from_tower, else_tower, to_tower);
                moves.Add(new Tuple<int, int>(n, to_tower));
                GenerateMoves(n - 1, else_tower, to_tower, from_tower);
            }
        }
        private void DrawTowers()
        {
            double canvasWidth = HanoiCanvas.ActualWidth;
            double towerSpacing = canvasWidth / num_of_towers;

            for (int i = 0; i < num_of_towers; i++)
            {
                double towerX = towerSpacing / 2 + i * towerSpacing;
                var tower_rod = new Line
                {
                    X1 = towerX,
                    Y1 = 50,
                    X2 = towerX,
                    Y2 = HanoiCanvas.ActualHeight - 50,
                    Stroke = Brushes.Black,
                    StrokeThickness = 5
                };
                HanoiCanvas.Children.Add(tower_rod);
                var tower_base = new Line
                {
                    X1 = towerX - towerSpacing / 3 - 50,
                    Y1 = HanoiCanvas.ActualHeight - 50,
                    X2 = towerX + towerSpacing / 3 + 50,
                    Y2 = HanoiCanvas.ActualHeight - 50,
                    Stroke = Brushes.Black,
                    StrokeThickness = 5
                };
                HanoiCanvas.Children.Add(tower_base);
            }
        }
        public List<Ring> CreateRing(int numberOfRings)
        {
            var rings = new List<Ring>(numberOfRings);
            for (int i = 0; i < numberOfRings; i++)
            {
                int ring_num = i + 1;
                double width = (HanoiCanvas.ActualWidth / 3 - 100) * ((numberOfRings - i) / (double)numberOfRings);
                Rectangle shape = new Rectangle
                {
                    Width = width,
                    Height = (HanoiCanvas.ActualHeight - 105) / numberOfRings,
                    Fill = Brushes.LightBlue,
                    Stroke = Brushes.Black,
                    StrokeThickness = 1
                };
                Ring ring = new Ring(ring_num, shape, 0);
                rings.Add(ring);
            }
            return rings;
        }
        public void DrawRings(List<Ring> rings)
        {
            HanoiCanvas.Children.Clear();
            DrawTowers();
            int base_level = 0;
            double towerSpacing = HanoiCanvas.ActualWidth / num_of_towers;
            foreach (var ring in rings)
            {
                Canvas.SetLeft(ring.Shape, towerSpacing / 2 + ring.TowerNumber * towerSpacing - ring.Shape.Width / 2);
                Canvas.SetTop(ring.Shape, HanoiCanvas.ActualHeight - 50 - (base_level + 1) * ((HanoiCanvas.ActualHeight - 105) / rings.Count));
                HanoiCanvas.Children.Add(ring.Shape);
                base_level++;
            }
        }
        public class Ring
        {
            public int Number { get; private set; }
            public Rectangle Shape { get; private set; }
            public int TowerNumber { get; set; }

            public Ring(int number, Rectangle shape, int initialTowerNumber)
            {
                Number = number;
                Shape = shape;
                TowerNumber = initialTowerNumber;
            }
        }
        private void Start_Click(object sender, RoutedEventArgs e)
        {
            int numRings = (int)DiskSlider.Value;
            InitializeTowers(numRings);
            GenerateMoves(numRings, 0, 2, 1);
        }

        private void GoBack_Click(object sender, RoutedEventArgs e)
        {
            // Возвращаемся в главное окно и показываем кнопки
            //_mainWindow.ShowButtons();
            _mainWindow.MainFrame.Content = null;  // Очистка Frame
        }
    }
}