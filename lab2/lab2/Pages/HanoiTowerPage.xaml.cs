using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using lab2.HanoiTower;

namespace lab2.Pages;

public partial class HanoiTowerPage : Page
{
    private readonly Stack<int>[] towers = new Stack<int>[3];
    private Rectangle[,] diskRectangles;
    private int animationSpeed = 20;
    private CancellationTokenSource cancellationTokenSource;
    private HanoiTowerDraw hanoiTowerDraw;
    
    private MainWindow _mainWindow;

    public HanoiTowerPage(MainWindow mainWindow)
    {
        InitializeComponent();
        InitializeTowers();
        _mainWindow = mainWindow;
        hanoiTowerDraw = new HanoiTowerDraw(HanoiCanvas);
        AddHanoiTowerControls();
    }
    
    private void InitializeTowers()
    {
        for (int i = 0; i < 3; i++)
        {
            towers[i] = new Stack<int>();
        }
    }

    // Метод для динамического добавления элементов управления
    private void AddHanoiTowerControls()
    {
        StackPanel panel = new StackPanel();
        {
            // Отступ от верхнего блока на 30 пикселей и от правого/левого краёв на 20 пикселей
            Margin = new Thickness(0, 30, 0, 0);
            HorizontalAlignment = HorizontalAlignment.Left; // Выравнивание по левому краю
        };
        
        // Заголовок
        TextBlock headerTextBlock = new TextBlock
        {
            Text = "Введите параметры тестирования",
            HorizontalAlignment = HorizontalAlignment.Left
        };
        // Применение стиля
        headerTextBlock.Style = (Style)_mainWindow.FindResource("HeaderTextBlockStyle");

        // Текстовое поле "Количество колец"
        TextBlock ringCountLabel = new TextBlock
        {
            Text = "Количество колец",
            HorizontalAlignment = HorizontalAlignment.Left,
            Margin = new Thickness(0, 20, 0, 8),
            Style = (Style)_mainWindow.FindResource("TextBlockStyle")
        };

        // Поле для ввода количества колец
        TextBox ringCountTextBox = new TextBox
        {
            Name = "ringCountTextBox",
            Width = 360,
            Margin = new Thickness(0, 0, 0, 20),
            HorizontalAlignment = HorizontalAlignment.Left,
            Style = (Style)_mainWindow.FindResource("RoundedTextBoxStyle") 
        };

        // Контейнер для кнопок "Старт" и "Стоп"
        StackPanel buttonsPanel = new StackPanel
        {
            Orientation = Orientation.Horizontal,
            HorizontalAlignment = HorizontalAlignment.Left
        };

        // Кнопка "Старт"
        Button startButton = new Button
        {
            Name = "startButton",
            Content = "Старт",
            Width = 170,
            HorizontalAlignment = HorizontalAlignment.Left,
            Margin = new Thickness(0, 510, 10, 0),
            Style = (Style)_mainWindow.FindResource("RoundedButtonStyle") 
        };
        startButton.Click += StartButton_Click;

        // Кнопка "Стоп"
        Button clearButton = new Button
        {
            Name = "clearButton",
            Content = "Стоп",
            Width = 170,
            HorizontalAlignment = HorizontalAlignment.Right,
            Margin = new Thickness(10, 510, 0, 0),
            Style = (Style)_mainWindow.FindResource("RoundedButtonStopStyle"),
            IsEnabled = false
        };
        clearButton.Click += ClearButton_Click;

        // Добавляем кнопки на панель
        buttonsPanel.Children.Add(startButton);
        buttonsPanel.Children.Add(clearButton);

        // Добавляем все элементы в левую панель
        panel.Children.Add(headerTextBlock);
        panel.Children.Add(ringCountLabel);
        panel.Children.Add(ringCountTextBox);
        panel.Children.Add(buttonsPanel);
        
        _mainWindow.PageContentControl.Content = panel;
    }
    
    private async void StartButton_Click(object sender, RoutedEventArgs e)
    {
        // Обработка нажатия кнопки Старт
        Button startButton = (Button)sender;
        TextBox ringCountTextBox = (TextBox)LogicalTreeHelper.FindLogicalNode(Application.Current.MainWindow, "ringCountTextBox");
        Button clearButton = (Button)LogicalTreeHelper.FindLogicalNode(Application.Current.MainWindow, "clearButton");


        startButton.IsEnabled = false;
        ringCountTextBox.IsEnabled = false;
        clearButton.IsEnabled = true;
        
        cancellationTokenSource = new CancellationTokenSource();
        var token = cancellationTokenSource.Token;
        if (int.TryParse(ringCountTextBox.Text, out int numberOfRings) && numberOfRings > 0 && numberOfRings <= 23)
        {
            InitializeTowers();
            hanoiTowerDraw.DrawTowers();
            diskRectangles = hanoiTowerDraw.InitializeRings(numberOfRings, towers);
            try
            {
                await hanoiTowerDraw.GenerateMoves(numberOfRings, 0, 1, 2, towers, diskRectangles, token);
            }
            catch (OperationCanceledException) { };
        }
        else
        {
            MessageBox.Show("Введите корректное количество колец");
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