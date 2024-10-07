using System.Windows;
using System.Windows.Controls;
using lab2.Tests;

namespace lab2
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // Переход на страницу фракталов и скрытие кнопок
        private void NavigateToFractalsPage_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new FractalsPageTests(this));
            HideButtons();
        }

        // Переход на страницу Ханойской башни и скрытие кнопок
        private void NavigateToHanoiTowerPage_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new HanoiTowerPageTests(this));
            HideButtons();
        }

        // Метод для скрытия кнопок
        public void HideButtons()
        {
            ButtonsPanel.Visibility = Visibility.Collapsed;
        }

        // Метод для отображения кнопок
        public void ShowButtons()
        {
            ButtonsPanel.Visibility = Visibility.Visible;
        }
    }
}
