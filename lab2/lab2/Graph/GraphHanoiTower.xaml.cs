using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using lab2.HanoiTower;
using ScottPlot;

namespace lab2.Graph;

public partial class GraphHanoiTower : Window
{
    public GraphHanoiTower()
    {
        InitializeComponent();
        PlotGraph();
    }

    private void PlotGraph()
    {
        List<double> diskCounts = new List<double>();
        List<double> times = new List<double>();

        for (int n = 3; n <= 30; n++)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            Hanoi(n, 'A', 'C', 'B');
            stopwatch.Stop();

            diskCounts.Add(n);
            times.Add(stopwatch.Elapsed.TotalMilliseconds);
        }

        var plt = wpfPlot.Plot;
        plt.Clear();

        // Добавление Scatter-графика в ScottPlot 5.x
        var scatter = plt.Add.Scatter(diskCounts.ToArray(), times.ToArray());
        scatter.MarkerSize = 5;
        scatter.MarkerShape = MarkerShape.FilledCircle;

        // Установка заголовка и подписей осей
        plt.Title("Зависимость времени работы от количества дисков");
        plt.XLabel("Количество дисков");
        plt.YLabel("Время работы (мс)");
        
        // Настройка максимальных значений осей (динамически)
        wpfPlot.Plot.Axes.Left.Max = diskCounts.Max();
        wpfPlot.Plot.Axes.Bottom.Max = times.Max();
        
        wpfPlot.Refresh();
    }

    private void Hanoi(int n, char from, char to, char aux)
    {
        if (n == 0)
            return;

        Hanoi(n - 1, from, aux, to);
        // Перемещение диска с 'from' на 'to' (логика перемещения)
        Hanoi(n - 1, aux, to, from);
    }
}
