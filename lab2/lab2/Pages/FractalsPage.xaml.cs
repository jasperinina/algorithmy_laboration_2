using lab2.Fractals;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using lab2.Tests;

namespace lab2.Pages;

public partial class FractalsPage : Page
{
    private MainWindow _mainWindow;
    
    public FractalsPage(MainWindow mainWindow)
    {
        InitializeComponent();
        _mainWindow = mainWindow;
        AddFractalControls();
    }
    
    // Метод для добавления элементов управления фракталами
    private void AddFractalControls()
    {
        StackPanel panel = new StackPanel();

        TextBlock depthTextBlock = new TextBlock
        {
            Text = "Глубина фрактала:",
            FontSize = 16,
            Margin = new Thickness(0, 20, 0, 5),
            HorizontalAlignment = HorizontalAlignment.Left
        };

        TextBox depthTextBox = new TextBox
        {
            Name = "tbForD",
            Width = 100,
            Height = 30,
            Margin = new Thickness(0, 0, 0, 20),
            HorizontalAlignment = HorizontalAlignment.Left
        };

        Button drawButton = new Button
        {
            Content = "Нарисовать фрактал",
            Width = 150,
            Height = 30,
            HorizontalAlignment = HorizontalAlignment.Left
        };
        drawButton.Click += Fractal_Click;

        // Добавляем все элементы в StackPanel
        panel.Children.Add(depthTextBlock);
        panel.Children.Add(depthTextBox);
        panel.Children.Add(drawButton);

        // Устанавливаем созданную панель в ContentControl
        _mainWindow.PageContentControl.Content = panel;
    }
    
    private void Fractal_Click(object sender, RoutedEventArgs e)
    {
        //canvas - канва для отрисовки фрактала
        //tbForD - textBox для введения глубины фрактала
        //bt1 - кнопка для запуска отрисовки
        
        // Извлекаем текстовое поле из PageContentControl
        StackPanel panel = (StackPanel)_mainWindow.PageContentControl.Content;
        TextBox depthTextBox = panel.Children.OfType<TextBox>().FirstOrDefault();
        
        // Получаем значение глубины из текстового поля
        int depth;
        if (!int.TryParse(depthTextBox.Text, out depth) || depth < 0)
        {
            MessageBox.Show("Введите корректное значение для глубины фрактала.");
            return;
        }
        
        canvas.Children.Clear();
        //int steps = Convert.ToInt32(tbForD.Text);
        DurersStar fractal = new DurersStar(
            canvas, new System.Drawing.Point((int)canvas.ActualWidth / 2, (int)canvas.ActualHeight / 2),
            Brushes.Black, 2, depth);
    }

    private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
    {

    }
    

}