﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using lab2.Fractals;
using lab2.Graph;
 
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
     
    // Метод для добавления динамических элементов управления фракталами
    private void AddFractalControls()
    { 
        StackPanel panel = new StackPanel();
        { 
            Margin = new Thickness(0, 30, 0, 0); 
            HorizontalAlignment = HorizontalAlignment.Left;
        };
        
        TextBlock headerTextBlock = new TextBlock
        {
            Text = "Введите параметры тестирования", 
            HorizontalAlignment = HorizontalAlignment.Left,
            Style = (Style)_mainWindow.FindResource("HeaderTextBlockStyle")
        };
         
        TextBlock depthTextBlock = new TextBlock
        {
            Text = "Глубина фрактала",
            HorizontalAlignment = HorizontalAlignment.Left,
            Margin = new Thickness(0, 20, 0, 8),
            Style = (Style)_mainWindow.FindResource("TextBlockStyle") 
        };

        TextBox depthTextBox = new TextBox
        {
            Name = "tbForD",
            Width = 360,
            Margin = new Thickness(0, 0, 0, 20),
            HorizontalAlignment = HorizontalAlignment.Left,
            Style = (Style)_mainWindow.FindResource("RoundedTextBoxStyle") 
        };

        Button drawButton = new Button
        {
            Content = "Нарисовать фрактал",
            Width = 360,
            HorizontalAlignment = HorizontalAlignment.Left,
            Margin = new Thickness(0, 430, 0, 0),
            Style = (Style)_mainWindow.FindResource("RoundedButtonStyle")
        };
        drawButton.Click += Fractal_Click;
         
        Button graphButton = new Button
        {
            Content = "График зависимости",
            Width = 360,
            HorizontalAlignment = HorizontalAlignment.Left,
            Margin = new Thickness(0, 40, 0, 0),
            Style = (Style)_mainWindow.FindResource("RoundedButtonGraphStyle")
        };
        graphButton.Click += Graph_Click;
        
        panel.Children.Add(headerTextBlock); 
        panel.Children.Add(depthTextBlock);
        panel.Children.Add(depthTextBox);
        panel.Children.Add(drawButton);
        panel.Children.Add(graphButton);

        _mainWindow.PageContentControl.Content = panel;
    }
     
    // Метод для очистки динамических элементов
    public void ClearDynamicElements()
    { 
        _mainWindow.PageContentControl.Content = null;
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
         if (!int.TryParse(depthTextBox.Text, out depth) || depth < 0 || depth > 6)
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

     private void Graph_Click(object sender, RoutedEventArgs e)
     {
         GraphFractal graphWindow = new GraphFractal();
         graphWindow.Show();
     }
 
     private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
     {
 
     }
 }