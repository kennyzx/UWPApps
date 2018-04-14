using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using WinRTXamlToolkit.Controls.DataVisualization.Charting;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace UWPBank
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class UWPChartPage : Page
    {
        public UWPChartPage()
        {
            this.InitializeComponent();

            Collection<XYValues> ChartData = new Collection<XYValues>();
            byte[] yValueArray = new byte[550];
            new Random().NextBytes(yValueArray);

            for (int index = 0; index < yValueArray.Length; index++)
            {
                ChartData.Add(new XYValues
                {
                    XValue = index,
                    YValue = yValueArray[index]
                });
            }

            chart1.Series.Add(new LineSeries
            {
                Title = "Squiggly Line",
                IndependentValuePath = "xValue",
                DependentValuePath = "yValue",
                ItemsSource = ChartData,
                IndependentAxis = new LinearAxis
                {
                    Minimum = 0,
                    Maximum = yValueArray.Length,
                    Orientation = AxisOrientation.X,
                    Interval = 50
                }
            });
        }
    }

    public class XYValues
    {
        public int XValue { get; set; }
        public byte YValue { get; set; }
    }
}
