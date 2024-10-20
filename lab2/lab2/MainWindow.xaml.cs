using System.Windows;
using System.Windows.Controls;
using lab2.Pages;
using lab2.Tests;

namespace lab2
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // Переход на страницу фракталов
        private void FractalRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new FractalsPage(this));
        }

        // Переход на страницу Ханойской башни
        private void HanoiRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new HanoiTowerPageTests(this));
        }
    }
}