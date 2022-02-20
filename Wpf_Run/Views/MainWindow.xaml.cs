using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Wpf_Run.Models;
using Wpf_Run.Services;
using LiveCharts.Wpf;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Configurations;

namespace Wpf_Run
{
    public partial class MainWindow : Window
    {


        private readonly string PATH = $"{Environment.CurrentDirectory}\\Data";
        private ViewsModuleData _viesmodeldata;
        private BindingList<RunnersAllData> _runersDateList;
        private List<BindingList<RunnersDayData>> _Files = new List<BindingList<RunnersDayData>>();
        private List<int> Dates = new List<int>();
        private int max, min;
        public SeriesCollection SeriesCollection { get; set; }
        public ChartValues<ObservablePoint> Values ;
        public Brush BrushRed { get; set; }
        public Brush BrushGreen { get; set; }
        public CartesianMapper<double> Mapper { get; set; }
        public MainWindow()
        {
            SeriesCollection = new SeriesCollection();
            InitializeComponent();
            DataContext = this;
            _viesmodeldata = new ViewsModuleData(PATH);
            Mapper = new CartesianMapper<double>()
                 .X((item, index) => index)
                 .Y((value, index) => value)
                 .Fill (item => (item) == max ? BrushRed : null)
                 .Stroke(item => (item) == max ? BrushRed : null);

            BrushRed = new SolidColorBrush(Color.FromRgb(255, 0, 0));
            BrushGreen = new SolidColorBrush(Color.FromRgb(0, 204, 0));
            DataContext = this;



        }
        public string[] Labels { get; set; }
        public Func<double, string> YFormatter { get; set; }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = this;
            _viesmodeldata.LoadDate();
            ShowDateGrid();
            Trigger trigger = new Trigger()
            {
                SourceName = "StapsGrid",
                Property = Grid.RowProperty,
                Value = true
            };
        }

        private void ShowDateGrid()
        {
            try
            {
                _runersDateList = _viesmodeldata.FillDateGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Close();
            }
            StapsGrid.ItemsSource = _runersDateList;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            _viesmodeldata.save_file(StapsGrid.SelectedItem as RunnersAllData);
        }

        private void StapsGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {

            SeriesCollection.Clear();
            min = 0;
            max = 0;
            try
            {
                Values = _viesmodeldata.GetChartValues(StapsGrid.SelectedItem as RunnersAllData, ref max, ref min);
            }
            catch(Exception t)
            {
                MessageBox.Show(t.Message);
            }
           
            SeriesCollection.Add(new LineSeries
            {
                Title = "Динамика",
                Values = Values,
                LineSmoothness = 0,
                PointGeometrySize = 10,
                PointForeground = Brushes.Gray,
                Configuration = "{Binding Mapper}"
            });
            
        }
    }
}
