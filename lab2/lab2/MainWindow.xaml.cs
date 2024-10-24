using System.Windows;
using lab2.Pages;

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
            // Очистить динамически добавленные элементы на текущей странице
            if (MainFrame.Content is FractalsPage fractalsPage)
            {
                fractalsPage.ClearDynamicElements();
            }
            MainFrame.Navigate(new FractalsPage(this));
        }

        // Переход на страницу Ханойской башни
        private void HanoiRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            // Очистить динамически добавленные элементы на текущей странице
            if (MainFrame.Content is FractalsPage fractalsPage)
            {
                fractalsPage.ClearDynamicElements();
            }
            
            MainFrame.Navigate(new HanoiTowerPage(this));
        }
        
    }
}