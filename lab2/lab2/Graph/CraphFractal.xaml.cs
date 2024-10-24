using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using lab2.Fractals;

namespace lab2.Graph
{
    public partial class GraphFractal : Window
    {
        public GraphFractal()
        {
            InitializeComponent();
            DrawFractalTimeGraph();
        }

        private void DrawFractalTimeGraph()
        {
            // Канва для отрисовки фрактала (не будет отображаться, просто для измерения времени)
            Canvas canvas = new Canvas();
            DurersStar fractal = new DurersStar(canvas, new System.Drawing.Point(250, 250), System.Windows.Media.Brushes.Black, 2, 0);

            double[] ranks = { 0, 1, 2, 3, 4, 5, 6 }; // Ранги фрактала от 0 до 6
            double[] renderTimes = new double[ranks.Length];

            // Измеряем время отрисовки для каждого ранга
            for (int i = 0; i < ranks.Length; i++)
            {
                renderTimes[i] = fractal.MeasureRenderTime((int)ranks[i]);
            }

            // Создаем график зависимости ранга фрактала от времени отрисовки
            WpfPlot1.Plot.Add.Scatter(ranks, renderTimes);
            WpfPlot1.Plot.Title("Зависимость времени отрисовки от ранга фрактала");
            WpfPlot1.Plot.XLabel("Ранг фрактала");
            WpfPlot1.Plot.YLabel("Время отрисовки (мс)");
            
            // Настройка максимальных значений осей (динамически)
            WpfPlot1.Plot.Axes.Left.Max = ranks.Max();
            WpfPlot1.Plot.Axes.Bottom.Max = renderTimes.Max();

            // Отображаем график
            WpfPlot1.Refresh();
        }
    }
}