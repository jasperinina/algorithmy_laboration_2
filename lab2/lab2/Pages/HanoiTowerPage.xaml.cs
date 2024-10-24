using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using lab2.Graph;
using lab2.HanoiTower;

namespace lab2.Pages;

public partial class HanoiTowerPage : Page
{
    private readonly Stack<int>[] towers = new Stack<int>[3];
    private Rectangle[,] diskRectangles;
    private int animationSpeed = 20;
    private CancellationTokenSource cancellationTokenSource;
    private HanoiTowerDraw hanoiTowerDraw;
    
    private int currentMoveIndex = -1;

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
            Margin = new Thickness(0, 30, 0, 0);
            HorizontalAlignment = HorizontalAlignment.Left;
        };
        
        // Заголовок
        TextBlock headerTextBlock = new TextBlock
        {
            Text = "Введите параметры тестирования",
            HorizontalAlignment = HorizontalAlignment.Left,
            Style = (Style)_mainWindow.FindResource("HeaderTextBlockStyle")
        };

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
            Margin = new Thickness(0, 370, 10, 0),
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
            Margin = new Thickness(10, 370, 0, 0),
            Style = (Style)_mainWindow.FindResource("RoundedButtonStopStyle"),
            IsEnabled = false
        };
        clearButton.Click += ClearButton_Click;
        
        // Контейнер для кнопок "Шаг вперед" и "Шаг назад"
        StackPanel buttonsPanel1 = new StackPanel
        {
            Orientation = Orientation.Horizontal,
            HorizontalAlignment = HorizontalAlignment.Left
        };
        
        // Кнопка "Шаг вперед"
        Button stepForwardButton = new Button
        {
            Name = "stepForwardButton",
            Content = "Шаг вперед",
            Width = 170,
            HorizontalAlignment = HorizontalAlignment.Left,
            Margin = new Thickness(0, 20, 10, 0),
            Style = (Style)_mainWindow.FindResource("RoundedButtonStepsStyle") 
        };
        stepForwardButton.Click += StepForwardButton_Click;

        // Кнопка "Шаг назад"
        Button stepBackButton = new Button
        {
            Name = "stepBackButton",
            Content = "Шаг назад",
            Width = 170,
            HorizontalAlignment = HorizontalAlignment.Right,
            Margin = new Thickness(10, 20, 0, 0),
            Style = (Style)_mainWindow.FindResource("RoundedButtonStepsStyle"),
            IsEnabled = false
        };
        stepBackButton.Click += StepBackButton_Click;
        
        Button graphButton = new Button
        {
            Content = "График зависимости",
            Width = 360,
            HorizontalAlignment = HorizontalAlignment.Left,
            Margin = new Thickness(0, 40, 0, 0),
            Style = (Style)_mainWindow.FindResource("RoundedButtonGraphStyle")
        };
        graphButton.Click += Graph_Click;

        // Добавляем кнопки на панель
        buttonsPanel.Children.Add(startButton);
        buttonsPanel.Children.Add(clearButton);
        
        // Добавляем кнопки на панель
        buttonsPanel1.Children.Add(stepForwardButton);
        buttonsPanel1.Children.Add(stepBackButton);
        
        // Добавляем все элементы в левую панель
        panel.Children.Add(headerTextBlock);
        panel.Children.Add(ringCountLabel);
        panel.Children.Add(ringCountTextBox);
        panel.Children.Add(buttonsPanel);
        panel.Children.Add(buttonsPanel1);
        panel.Children.Add(graphButton);
        
        _mainWindow.PageContentControl.Content = panel;
    }
    
    private async Task PlayMovesAsync(CancellationToken token)
    {
        for (currentMoveIndex = 0; currentMoveIndex < hanoiTowerDraw.Moves.Count; currentMoveIndex++)
        {
            token.ThrowIfCancellationRequested();

            var move = hanoiTowerDraw.Moves[currentMoveIndex];
            await hanoiTowerDraw.MoveDisk(move.from, move.to, towers, diskRectangles, token, animate: true);

            // Задержка между ходами (настраиваемая)
            await Task.Delay(500, token);
        }
    }
    
    private async void StartButton_Click(object sender, RoutedEventArgs e)
    {
        // Получаем ссылки на элементы управления
        Button startButton = (Button)sender;
        TextBox ringCountTextBox = (TextBox)LogicalTreeHelper.FindLogicalNode(Application.Current.MainWindow, "ringCountTextBox");
        Button clearButton = (Button)LogicalTreeHelper.FindLogicalNode(Application.Current.MainWindow, "clearButton");
        Button stepForwardButton = (Button)LogicalTreeHelper.FindLogicalNode(Application.Current.MainWindow, "stepForwardButton");
        Button stepBackButton = (Button)LogicalTreeHelper.FindLogicalNode(Application.Current.MainWindow, "stepBackButton");

        // Отключаем/включаем кнопки
        startButton.IsEnabled = false;
        ringCountTextBox.IsEnabled = false;
        clearButton.IsEnabled = true;
        stepForwardButton.IsEnabled = false;
        stepBackButton.IsEnabled = false;

        cancellationTokenSource = new CancellationTokenSource();
        var token = cancellationTokenSource.Token;

        if (int.TryParse(ringCountTextBox.Text, out int numberOfRings) && numberOfRings > 0 && numberOfRings <= 23)
        {
            InitializeTowers();
            hanoiTowerDraw.ClearCanvas();
            hanoiTowerDraw.DrawTowers();
            diskRectangles = hanoiTowerDraw.InitializeRings(numberOfRings, towers);

            // Генерируем список ходов
            hanoiTowerDraw.Moves.Clear();
            hanoiTowerDraw.GenerateMovesList(numberOfRings, 0, 1, 2);

            currentMoveIndex = -1; // Сбрасываем индекс текущего хода

            // Запускаем автоматическую анимацию
            try
            {
                await PlayMovesAsync(token);
            }
            catch (OperationCanceledException)
            {
                // Анимация была остановлена
            }

            // После завершения анимации
            startButton.IsEnabled = true;
            ringCountTextBox.IsEnabled = true;
            clearButton.IsEnabled = false;
            stepForwardButton.IsEnabled = false; // Нет шагов вперед после завершения
            stepBackButton.IsEnabled = currentMoveIndex >= 0;
        }
        else
        {
            MessageBox.Show("Введите корректное количество колец");
            startButton.IsEnabled = true;
            ringCountTextBox.IsEnabled = true;
            clearButton.IsEnabled = false;
        }
    }
    
    private async void StepForwardButton_Click(object sender, RoutedEventArgs e)
    {
        if (currentMoveIndex + 1 < hanoiTowerDraw.Moves.Count)
        {
            currentMoveIndex++;
            var move = hanoiTowerDraw.Moves[currentMoveIndex];
            await hanoiTowerDraw.MoveDisk(move.from, move.to, towers, diskRectangles, CancellationToken.None, animate: true);

            UpdateStepButtons();
        }
    }

    private async void StepBackButton_Click(object sender, RoutedEventArgs e)
    {
        if (currentMoveIndex >= 0)
        {
            var move = hanoiTowerDraw.Moves[currentMoveIndex];
            await hanoiTowerDraw.MoveDiskBack(move.from, move.to, towers, diskRectangles, CancellationToken.None, animate: true);
            currentMoveIndex--;

            UpdateStepButtons();
        }
    }
    
    private void UpdateStepButtons()
    {
        Button stepForwardButton = (Button)LogicalTreeHelper.FindLogicalNode(Application.Current.MainWindow, "stepForwardButton");
        Button stepBackButton = (Button)LogicalTreeHelper.FindLogicalNode(Application.Current.MainWindow, "stepBackButton");

        stepForwardButton.IsEnabled = currentMoveIndex + 1 < hanoiTowerDraw.Moves.Count;
        stepBackButton.IsEnabled = currentMoveIndex >= 0;
    }
    
    private void Graph_Click(object sender, RoutedEventArgs e)
    {
        // Открытие нового окна с графиком
        GraphHanoiTower graphWindow = new GraphHanoiTower();
        graphWindow.Show();
    }

    private void ClearButton_Click(object sender, RoutedEventArgs e)
    {
        cancellationTokenSource?.Cancel();

        // Получаем ссылки на элементы управления
        Button startButton = (Button)LogicalTreeHelper.FindLogicalNode(Application.Current.MainWindow, "startButton");
        TextBox ringCountTextBox = (TextBox)LogicalTreeHelper.FindLogicalNode(Application.Current.MainWindow, "ringCountTextBox");
        Button clearButton = (Button)LogicalTreeHelper.FindLogicalNode(Application.Current.MainWindow, "clearButton");
        Button stepForwardButton = (Button)LogicalTreeHelper.FindLogicalNode(Application.Current.MainWindow, "stepForwardButton");
        Button stepBackButton = (Button)LogicalTreeHelper.FindLogicalNode(Application.Current.MainWindow, "stepBackButton");

        // Отключаем кнопки
        clearButton.IsEnabled = false;

        // Включаем возможность использовать шаги
        stepForwardButton.IsEnabled = currentMoveIndex + 1 < hanoiTowerDraw.Moves.Count;
        stepBackButton.IsEnabled = currentMoveIndex >= 0;

        startButton.IsEnabled = true;
        ringCountTextBox.IsEnabled = true;
    }
}