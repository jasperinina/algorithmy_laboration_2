using lab2.Fractals;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace lab2.Pages;

public partial class FractalsPage : Page
{
    public FractalsPage()
    {
        InitializeComponent();
    }
    private void bt1_Click(object sender, RoutedEventArgs e)
    {
        //canvas - канва для отрисовки фрактала
        //tbForD - textBox для введения глубины фрактала
        //bt1 - кнопка для запуска отрисовки
        canvas.Children.Clear();
        int steps = Convert.ToInt32(tbForD.Text);
        DurersStar fractal = new DurersStar(
            canvas, new System.Drawing.Point((int)canvas.ActualWidth / 2, (int)canvas.ActualHeight / 2),
            Brushes.Black, 2, steps);
    }

    private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
    {

    }
}