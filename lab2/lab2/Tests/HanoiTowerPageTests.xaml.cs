using System.Windows;
using System.Windows.Controls;

namespace lab2.Tests
{
    public partial class HanoiTowerPageTests : Page
    {
        private MainWindow _mainWindow;

        public HanoiTowerPageTests(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
        }

        private void GoBack_Click(object sender, RoutedEventArgs e)
        {
            // Возвращаемся в главное окно и показываем кнопки
            _mainWindow.ShowButtons();
            _mainWindow.MainFrame.Content = null;  // Очистка Frame
        }
    }
}